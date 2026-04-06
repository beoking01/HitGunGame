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
        if (col != null) {
            col.enabled = false;
        }

        SetLayerRecursively(transform, LayerMask.NameToLayer("Weapon"));

    }

    public void Drop() {
        this.objectGrabPointTransform = null;

        objectRigidbody.useGravity = true;

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

    private void FixedUpdate() {
        if (objectGrabPointTransform != null) {

            float lerpSpeed = 40f;

            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            Quaternion newRotation = Quaternion.Lerp(transform.rotation, objectGrabPointTransform.rotation, Time.deltaTime * lerpSpeed);
            objectRigidbody.MoveRotation(newRotation);

        }
    }
}