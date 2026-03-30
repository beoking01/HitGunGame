using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Add or remove an InteractionEvent component to this gameObject
    [SerializeField]
    public bool useEvents;
    // message displayed to player when looking at an interactable
    [SerializeField]
    public string promtMessage;

    public virtual string OnLook()
    {
        return promtMessage;
    }
    // this function will be called from our player
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {
        // we wont have any code written in this function
        // this is a template function to be overriden by our subclasses
    }
}
