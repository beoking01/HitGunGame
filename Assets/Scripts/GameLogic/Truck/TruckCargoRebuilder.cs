using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class TruckCargoRebuilder : MonoBehaviour
{
    [SerializeField] private Transform cargoRoot;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;
    [SerializeField] private bool clearExistingChildren = true;
    [SerializeField] private bool applyInCargoPhysics = true;

    private bool rebuilt;

    private void Reset()
    {
        cargoRoot = transform;
    }

    private void Awake()
    {
        if (cargoRoot == null)
            cargoRoot = transform;
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        if (rebuilt)
            return;

        rebuilt = true;

        if (TruckStateManager.Instance == null || prefabDatabase == null || cargoRoot == null)
            return;

        if (clearExistingChildren)
        {
            for (int i = cargoRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(cargoRoot.GetChild(i).gameObject);
            }
        }

        var items = TruckStateManager.Instance.State.Items;

        for (int i = 0; i < items.Count; i++)
        {
            TruckItemState state = items[i];
            if (state == null)
                continue;

            if (!prefabDatabase.TryGetPrefab(state.itemId, out GameObject prefab))
            {
                Debug.LogWarning("TruckCargoRebuilder: Missing prefab for itemId " + state.itemId);
                continue;
            }

            Vector3 spawnWorldPosition = cargoRoot.TransformPoint(state.localPosition);
            Quaternion spawnWorldRotation = cargoRoot.rotation * state.localRotation;

            GameObject spawned = Instantiate(prefab, spawnWorldPosition, spawnWorldRotation, cargoRoot);
            ItemRuntimeUtility.ApplyLocalTransform(spawned.transform, state.localPosition, state.localRotation, state.localScale);
            ApplyCargoPhysics(spawned);

            // Re-apply once after Update and once after physics to prevent other systems
            // from briefly resetting spawned items to cargo root.
            StartCoroutine(ItemRuntimeUtility.ReapplyTransformAfterSpawn(spawned.transform, state.localPosition, state.localRotation, state.localScale));

            ItemRuntimeUtility.SetupLootAndWorldItem(spawned, state.itemId, state.cargoItemId, prefabDatabase);
        }
    }

    private void ApplyCargoPhysics(GameObject spawned)
    {
        if (spawned == null)
            return;

        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (!applyInCargoPhysics)
        {
            rb.isKinematic = true;
        }

        rb.Sleep();
    }

}
