using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Mouse : MonoBehaviour
{
    private Rigidbody2D mouse;
    private Animator animator;

    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float moveSpeed;

    private void Awake()
    {
        mouse = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        if (!IsGroundExist())
        {
            Turn();
        }
    }

    private void Move()
    {
        mouse.velocity = new Vector2(transform.right.x * -moveSpeed, mouse.velocity.y);
    }

    private void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }

    private bool IsGroundExist()
    {
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 1f, groundMask);
    }
}
