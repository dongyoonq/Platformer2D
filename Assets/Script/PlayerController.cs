using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

    [SerializeField] private LayerMask groundMask;
    [SerializeField] Transform chkPos;

    private Vector2 perp;
    private float angle;
    private bool isGround;
    private bool isSlope;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        chkPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RayChk();
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

    public float maxAngle;
    private void Move()
    {
        //if (moveInput.x == 0)
        //    player.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        //else
        //    player.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (!isSlope && LeftMaxSpeedChk())
        {
            player.AddForce(moveInput.x * Vector2.right * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        else if (!isSlope && RightMaxSpeedChk())
        {
            player.AddForce(moveInput.x * Vector2.right * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        else if (isSlope && Mathf.Abs(angle) < maxAngle)
        {
            player.AddForce(moveInput.x * moveSpeed * Time.deltaTime * -perp, ForceMode2D.Impulse);
        }
        else if (isSlope && Mathf.Abs(angle) > maxAngle)
        {
            player.AddForce(moveInput.x * Time.deltaTime * -perp, ForceMode2D.Impulse);
        }
    }

    private bool LeftMaxSpeedChk()
    {
        return moveInput.x < 0 && player.velocity.x > -maxSpeed;
    }

    private bool RightMaxSpeedChk()
    {
        return moveInput.x > 0 && player.velocity.x < maxSpeed;
    }

    private void RayChk()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(chkPos.position, Vector2.down, 10f, groundMask);

        if (hit)
        {
            angle = Vector2.Angle(hit.normal, Vector2.up);
            perp = Vector2.Perpendicular(hit.normal).normalized;

            if (angle != 0)
                isSlope = true;
            else
                isSlope = false;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
            Debug.DrawLine(hit.point, hit.point + perp, Color.blue);
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
