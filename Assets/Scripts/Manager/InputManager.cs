using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook playerLook;
    private PlayerWeapons playerWeapons;
    private GameManager gameManager;

    public PlayerInput.OnFootActions OnFoot { get => onFoot; set => onFoot = value; }

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        playerWeapons = GetComponent<PlayerWeapons>();

        gameManager = GameManager.Instance;

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();

        onFoot.Shoot.performed += ctx => playerWeapons.Shoot();
        onFoot.Reload.performed += ctx => playerWeapons.ReloadBullet();
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
