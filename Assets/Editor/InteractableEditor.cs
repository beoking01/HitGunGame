using UnityEditor;
// is attribute tick that class InteractableEditor is for Interactable or sub class of Interactable
[CustomEditor(typeof(Interactable), true)] 
public class InteractableEditor : Editor
{
    // get called every time unity update the editor interface
    public override void OnInspectorGUI()
    {
        // target is object being edited, because target is object so need to cast
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.promtMessage = EditorGUILayout.TextField("Promt Message", interactable.promtMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvent. ", MessageType.Info);
            if (interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvents)
            {
                if (interactable.GetComponent<InteractionEvent>() == null)
                    interactable.gameObject.AddComponent<InteractionEvent>();
            }
            else
            {
                // we are not using events. Remove the component.
                if (interactable.GetComponent<InteractionEvent>() != null)
                    DestroyImmediate(interactable.GetComponent<InteractionEvent>());
            }
        }
    }
}
