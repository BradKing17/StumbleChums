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
    public SphereCollider grabRange;
    public List<Collider> grabbables;
    public SpringJoint joint;

    private Vector2 moveInput;
    private float grabInput;
    private bool grounded;
    private bool canDive = true;
    private bool canGrab = true;
    

    void Update()
    {
        grounded = IsGrounded();
        // Horizontal
        rb.AddForce(transform.right * (moveInput.x * (grounded ? moveSpeed : airSpeed)), ForceMode.Acceleration);
        if (isTopDown)
        {
            rb.AddForce(transform.forward * (moveInput.y * (grounded ? moveSpeed : airSpeed)), ForceMode.Acceleration);
        }
        Debug.Log(grabInput);
        if(grabInput > 0)
        {
            joint.connectedBody = grabbables[0].gameObject.GetComponent<Rigidbody>();
            joint.spring = 100;
        }
        else
        {
            joint.spring = 0;
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
        return Physics.Raycast(transform.position, Vector3.down, cc.height / 2.0F + 0.05F);
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
            canDive = true;
            rb.AddForce(Vector2.up * jumpStrength, ForceMode.VelocityChange);
        }
    }

    public void Grab(InputAction.CallbackContext _value)
    {
       if(!grounded && canDive)
        {
            canDive = false;
            rb.AddForce(Vector2.right * rb.velocity.x * (jumpStrength/2), ForceMode.VelocityChange);
        }
       else
       {
            grabInput = _value.ReadValue<float>();
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject != other.gameObject)
            {
                
                grabbables.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject != other.gameObject)
            {
                grabbables.Remove(other);
            }
        };
    }
}
