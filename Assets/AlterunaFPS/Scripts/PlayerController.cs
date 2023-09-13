using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;

    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity = -9.8f;

    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundCheckSize;

    public bool IsOnGround { get; private set; }
    public bool IsJumping { get; private set; }


    Vector2 moveDirection;
    Vector3 fallingVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 1f);
    }
    private void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();

        IsOnGround = Physics.CheckSphere(groundCheckPoint.position, groundCheckSize,groundMask);
        if(IsOnGround && fallingVelocity.y < 0) // fallingVelocity < 0 tức là đang rơi
        {
            fallingVelocity.y = -2f;
        }
        if (IsOnGround)
        {
            IsJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            Jump();
        }
   
    }
    private void FixedUpdate()
    {
        Move();
        // ================ Gravity ============//
        // v thay đổi sau 1 khoảng thời gian t: v = v0 + g*t
        fallingVelocity.y += gravity * Time.fixedDeltaTime; //  v = g*t
        // quãng đường di chuyển trong thời gian t:  s = v*t
        characterController.Move(fallingVelocity * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckPoint.position,groundCheckSize);
    }
    void Move()
    {
        Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        characterController.Move(move * speed * Time.deltaTime);
        //animator.SetFloat("Speed", move.magnitude * speed);
    }
    void Jump()
    {
        // công thức vận tốc cần đạt để nhảy đến độ cao nhất định:
        // v = sqrt(2*h*g)
        fallingVelocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity); // do gravity đang để âm nên phải đảo dấu
        IsJumping = true;
    }

    bool CanJump()
    {
        return IsOnGround;
    }
}
