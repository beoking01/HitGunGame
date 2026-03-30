using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook playerLook;
    private PlayerItemActionController itemActionController;
    private GameManager gameManager;

    public PlayerInput.OnFootActions OnFoot { get => onFoot; set => onFoot = value; }

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        itemActionController = GetComponent<PlayerItemActionController>();

        gameManager = GameManager.Instance;

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();

        onFoot.Shoot.performed += ctx => itemActionController?.UsePrimary();
        onFoot.Reload.performed += ctx => itemActionController?.ReloadCurrent();
    }

    void FixedUpdate()
    {
        if (gameManager.isGamePlay())
        {
            Vector2 moveInput = onFoot.Movement.ReadValue<Vector2>();
            motor.ProcessMove(moveInput);
        }         
        
    }
    void LateUpdate()
    {
        if(gameManager.isGamePlay())
            playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
