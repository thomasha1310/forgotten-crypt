using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithAnimationController : MonoBehaviour
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
        
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }
}
