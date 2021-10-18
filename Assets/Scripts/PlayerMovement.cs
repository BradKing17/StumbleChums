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

    public bool isTopDown = false;

    public Sprite[] sprites;
    private SpriteRenderer spr;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        spr = GetComponentInChildren<SpriteRenderer>();
        if (isTopDown)
        {
            spr.transform.Rotate(new Vector3(90, 0, 0));
        }
    }

    void Update()
    {
        bool grounded = IsGrounded();
        // Horizontal
        float horizontal = Input.GetAxis("Horizontal") * (grounded ? moveSpeed : airSpeed);
        rb.AddForce(transform.right * horizontal, ForceMode.Force);

        spr.sprite = sprites[0];
        Vector3 vel = rb.velocity;
        if(vel.x >= -vel.z && vel.x >= 0.5F)
        {
            spr.sprite = sprites[2];
        }
        else if (vel.x <=vel.z && vel.x <= -0.5)
        {
            spr.sprite = sprites[1];
        }
        if (vel.z >= vel.x && vel.z >= 0.5F)
        {
            spr.sprite = sprites[3];
        }
        
        if(isTopDown)
        {
            float vertical = Input.GetAxis("Vertical") * (grounded ? moveSpeed : airSpeed);
            rb.AddForce(transform.forward * vertical, ForceMode.Force);
        }
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
