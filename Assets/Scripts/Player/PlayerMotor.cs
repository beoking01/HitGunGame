using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isMove = false;
    public bool isGrounded;
    public float speed = 5f;
    public float gravirty = -9.8f;
    public float jumpHight = 3f;
    public bool lerpCrouch = false;
    public float crouchTimer = 0;
    public bool crouching = false;
    public bool sprinting = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Update status of isGrounded per frames
        isGrounded = controller.isGrounded;
        ProcessCrouch();
    }
    public void ProcessMove(Vector2 input)
    {
        if (input.magnitude > 0.1f && isGrounded)
        {
            if (!isMove)
            {   
                if(speed == 5)
                    SoundManager.Instance.PlayLoop("Walk");
                if (speed == 8)
                    SoundManager.Instance.PlayLoop("Run");
                isMove = true;
            }
        }
        else
        {
            if (isMove)
            {
                SoundManager.Instance.StopLoop();
                isMove = false;
            }
        }
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime * speed);
        playerVelocity.y += gravirty * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    public void ProcessCrouch()
    {
        if (lerpCrouch)
        {
            // add deltaTime to charactor not lag during change height 
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1; // là tỉ lệ thời gian đã trôi qua 0 -> 1
            p *= p; // là hiệu ứng chậm rồi nhanh ease-in

            // nội suy từ controller.height đến 1 (2) với tỷ lệ p
            if (crouching)
            {
                // giảm chiều cao về 1 đơn vị
                controller.height = Mathf.Lerp(controller.height, 1, p);
                speed = 3f;
            }
            else
            {
                // tăng chiều cao lên 2 đơn vị
                controller.height = Mathf.Lerp(controller.height, 2, p);
                speed = 5f;
            }
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            SoundManager.Instance.PlaySound("Jump");
            playerVelocity.y = Mathf.Sqrt(jumpHight * -3.0f * gravirty);
        }
    }
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }
    }
}
