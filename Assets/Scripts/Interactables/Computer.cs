using UnityEngine;

public class Computer : Interactable
{
    [SerializeField] private Camera computerCamera;
    [SerializeField] private Camera playerCamera;
    private bool isUsingComputerCamera;

    private void Start()
    {
        if (computerCamera != null)
        {
            computerCamera.enabled = false;
        }

        if (playerCamera != null)
        {
            playerCamera.enabled = true;
        }
    }

    protected override void Interact()
    {
        if (computerCamera != null && playerCamera != null)
        {
            isUsingComputerCamera = !isUsingComputerCamera;
            computerCamera.enabled = isUsingComputerCamera;
            playerCamera.enabled = !isUsingComputerCamera;
        }
    }
}