using UnityEngine;

public class KeyPad : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;

    // this function is where we will design interaction using code
    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}
