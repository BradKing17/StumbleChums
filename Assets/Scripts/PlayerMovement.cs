using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerType playerType;
    
    
    public float moveSpeed = 10.0F;
    public float airSpeed = 5.0F;
    public float jumpStrength = 1.0F;

    public bool isTopDown = false;
    
    public Rigidbody rb;
    public CapsuleCollider cc;
    public SpriteRenderer spr;

    private Vector2 moveInput;

    void Update()
    {
        bool grounded = IsGrounded();
        // Horizontal
        rb.AddForce(transform.right * (moveInput.x * (grounded ? moveSpeed : airSpeed)), ForceMode.Acceleration);
        if (isTopDown)
        {
            rb.AddForce(transform.forward * (moveInput.y * (grounded ? moveSpeed : airSpeed)), ForceMode.Acceleration);
        }

        spr.sprite = playerType.sprites.front;
        Vector3 vel = rb.velocity;
        if(vel.x >= -vel.z && vel.x >= 0.5F)
        {
            spr.sprite = playerType.sprites.right;
        }
        else if (vel.x <=vel.z && vel.x <= -0.5)
        {
            spr.sprite = playerType.sprites.left;
        }
        if (vel.z >= vel.x && vel.z >= 0.5F)
        {
            spr.sprite = playerType.sprites.back;
        }
    }

    private bool IsGrounded()
    {
        return rb.velocity.y <= 0 && Physics.Raycast(transform.position, Vector3.down, cc.height / 2.0F + 0.05F);
    }

    public void SetPlayerType(PlayerType _type)
    {
        playerType = _type;
        spr.sprite = _type.sprites.front;
    }

    public void SetTopDown(bool _topDown)
    {
        isTopDown = _topDown;
        rb.constraints = isTopDown ? 
            RigidbodyConstraints.FreezeRotation:
            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ ;
        if (_topDown)
        {
            spr.transform.Rotate(new Vector3(90, 0, 0));
        }
    }

    public void Move(InputAction.CallbackContext _value)
    {
        moveInput = _value.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext _value)
    {
        if (!isTopDown && _value.phase == InputActionPhase.Started && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode.VelocityChange);
        }
    }

}
