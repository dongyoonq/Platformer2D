using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpTile : MonoBehaviour
{
    public UnityEvent OnDowned;
    public float jumpForce = 13f;

    private BoxCollider2D colliderBox;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colliderBox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "FootRange")
        {
            animator.SetBool("OnDown", true);
            OnDowned?.Invoke();
            ApplyForce(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "FootRange")
        {
            animator.SetBool("OnDown", false);
        }
    }

    private void ApplyForce(GameObject gameObject)
    {
        Rigidbody2D rb = gameObject.GetComponentInParent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
