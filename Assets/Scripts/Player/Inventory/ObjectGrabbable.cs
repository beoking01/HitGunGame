using UnityEngine;

public class ObjectGrabbable : MonoBehaviour {
    private Rigidbody objectRigidbody;
    private Collider col;
    private Transform objectGrabPointTransform;

    private void Awake() {
        objectRigidbody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();    
        
        // Đảm bảo Rigidbody được thiết lập để di chuyển mượt
        if (objectRigidbody != null) {
            objectRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            objectRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void Grab(Transform grabPoint) {
        this.objectGrabPointTransform = grabPoint;

        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true; 

        // Biến vật thể thành con của điểm cầm
        transform.SetParent(grabPoint);
        // Gán vị trí và góc xoay về 0 để khớp hoàn toàn với điểm cầm
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        if (col != null) {
            col.enabled = false;
        }

        SetLayerRecursively(transform, LayerMask.NameToLayer("Weapon"));

    }

    public void Drop() {
        this.objectGrabPointTransform = null;

        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false; 
        transform.SetParent(null);
        
        if (col != null) {
            col.enabled = true;
        }

        SetLayerRecursively(transform, LayerMask.NameToLayer("Default"));
    }

    private void SetLayerRecursively(Transform root, int layer) {
        root.gameObject.layer = layer;

        for (int i = 0; i < root.childCount; i++) {
            SetLayerRecursively(root.GetChild(i), layer);
        }
    }

    private void LateUpdate() {
        if (objectGrabPointTransform != null) {
            transform.position = objectGrabPointTransform.position;
            transform.rotation = objectGrabPointTransform.rotation;
        }
    }
}