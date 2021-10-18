using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider cc;
    
    public float moveSpeed = 10.0F;
    public float airSpeed = 5.0F;
    public float jumpStrength = 1.0F;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        bool grounded = IsGrounded();
        // Horizontal
        float horizontal = Input.GetAxis("Horizontal") * (grounded ? moveSpeed : airSpeed);
        rb.AddForce(transform.right * horizontal, ForceMode.Force);
        // Jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return rb.velocity.y <= 0 && Physics.Raycast(transform.position, Vector3.down, cc.height / 2.0F + 0.05F);
    }

}
