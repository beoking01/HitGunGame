using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        playerUI.UpdateText("");
        // create a ray at the center of the camera, shooting outwards.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfor; // variable to store our collision information 

        if (Physics.Raycast(ray, out hitInfor, distance, mask))
        {
            if (hitInfor.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfor.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.OnLook());

                if (inputManager.OnFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
