using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 moveInput;
    [SerializeField] private UnityEvent OnMoved;
    [SerializeField] private UnityEvent OnJumped;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float maxSpeed = 3.4f;
    [SerializeField] private float jumpForce = 8f;

    private bool isGround;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnMove(InputValue value)
    {
        moveInput.x = value.Get<Vector2>().x;
        moveInput.y = value.Get<Vector2>().y;
        animator.SetFloat("MoveSpeed", Mathf.Abs(moveInput.x));
        if (moveInput.x > 0)
            render.flipX = false;
        else if (moveInput.x < 0)
            render.flipX = true;

        OnMoved?.Invoke();
    }

    private void OnJump(InputValue value)
    {
        if (isGround)
        {
            Jump();
            OnJumped?.Invoke();
            isGround = false;
        }
    }

    private void Move()
    {
        if (moveInput.x < 0 && player.velocity.x > -maxSpeed)
        {
            player.AddForce(moveInput.x * Vector2.right * moveSpeed, ForceMode2D.Force);
        }
        else if (moveInput.x > 0 && player.velocity.x < maxSpeed)
        {
            player.AddForce(moveInput.x * Vector2.right * moveSpeed, ForceMode2D.Force);
        }
    }

    private void Jump()
    {
        player.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        OnJumped?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("IsGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsGround", false);
    }
}
