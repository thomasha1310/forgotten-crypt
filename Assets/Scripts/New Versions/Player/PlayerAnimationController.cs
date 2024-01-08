using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetBool("IsJumping", rb.velocity.y > 0.01f);
        animator.SetBool("IsFalling", rb.velocity.y < -0.01f);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    public void PlayerAttacks(int i, Collider2D[] c)
    {
        animator.SetTrigger("Attack");
    }
}
