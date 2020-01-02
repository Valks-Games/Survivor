using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public float MaxSpeed = 5.0f;

    private Rigidbody rb;
    private Animator animator;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        float speedPercent = rb.velocity.magnitude / MaxSpeed;
        animator.SetFloat("speedPercent", speedPercent, 0.1f, Time.deltaTime);
    }
}
