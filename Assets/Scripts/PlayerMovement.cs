using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public PlayerManager playerManager;
    
    public float moveSpeed = 1.0F;
    public float airSpeed = 0.5F;
    public float jumpStrength = 1.0F;

    public bool isTopDown = false;
    
    private Rigidbody rb;
    private CapsuleCollider cc;
    private SpriteRenderer spriteRenderer;
    public SphereCollider grabRange;
    public List<Collider> grabbables;
    public SpringJoint joint;

    private Vector2 moveInput;
    private float grabInput;
    public bool grounded;
    private bool canDive = true;
    private bool canGrab = true;
    public bool ragdoll = false;
    private bool ragdollFlashing;

    private RigidbodyConstraints standardContraint;
    private RigidbodyConstraints unfreezeZConstraint;
    public LayerMask groundLayer;
    public float getUpTimer = 0;
    
    
    private void OnEnable()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        
        // So if it is top down the PosZ constraint is unfrozen
        if (isTopDown)
        {
            Debug.Log("Ding");
            
            standardContraint =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | 
                                 RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            Debug.Log("Dong");

            standardContraint = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                                RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
        
        unfreezeZConstraint = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ |
                              RigidbodyConstraints.FreezeRotationY;
        rb.constraints = standardContraint;
        
    }

    void Update()
    {
        grounded = IsGrounded();
        // Horizontal
        rb.AddForce(transform.right * (moveInput.x * (grounded ? moveSpeed : airSpeed)), ForceMode.Acceleration);
        if (isTopDown)
        {
            rb.AddForce(transform.forward * (moveInput.y * (grounded ? moveSpeed : airSpeed)), ForceMode.Acceleration);
        }
        //Debug.Log(grabInput);
        if(grabInput > 0)
        {
            joint.connectedBody = grabbables[0].gameObject.GetComponent<Rigidbody>();
            joint.spring = 100;
        }
        else
        {
            joint.spring = 0;
        }

        spriteRenderer.sprite = playerManager.type.sprites.front;
        Vector3 vel = rb.velocity;
        if(vel.x >= -vel.z && vel.x >= 0.5F)
        {
            spriteRenderer.sprite = playerManager.type.sprites.right;
        }
        else if (vel.x <=vel.z && vel.x <= -0.5)
        {
            spriteRenderer.sprite = playerManager.type.sprites.left;
        }
        if (vel.z >= vel.x && vel.z >= 0.5F)
        {
            spriteRenderer.sprite = playerManager.type.sprites.back;
        }

        if (ragdoll && grounded)
        {
            if (getUpTimer < 1F)
            {
                getUpTimer += Time.deltaTime % 60;
            }
            else
            {
                getUpTimer = 0;
                EndRagdoll();
            }
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * (cc.height / 2.0F + 0.05F), Color.green);
        return Physics.Raycast(transform.position, Vector3.down, cc.height / 2.0F + 0.05F, groundLayer);
    }

    public void Move(InputAction.CallbackContext _value)
    {
        if (!ragdoll)
        {
            moveInput = _value.ReadValue<Vector2>();
        }
        else
        {
            moveInput = _value.ReadValue<Vector2>()/8;
        }
    }

    public void Jump(InputAction.CallbackContext _value)
    {
        if (!isTopDown && _value.phase == InputActionPhase.Started && IsGrounded() && !ragdoll)
        {
            canDive = true;
            rb.AddForce(Vector2.up * jumpStrength, ForceMode.VelocityChange);
        }
    }

    public void Grab(InputAction.CallbackContext _value)
    {
       if(!grounded && canDive && !ragdoll) 
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

    private void OnCollisionEnter(Collision other)
    {
        //print("ding");
        if (other.gameObject.CompareTag("ragdoll"))
        {
            getUpTimer = 0;
            StartRagdoll();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("ragdoll"))
        {
            getUpTimer = 0;
        }
    }
    

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("ragdoll") && grounded)
        {
            getUpTimer = 0;
        }
    }

    void EndRagdoll()
    {
        rb.AddForce(transform.up, ForceMode.Force);
        gameObject.transform.rotation = new Quaternion(0, 0, 0,0);
        rb.constraints = standardContraint;
        spriteRenderer.color = Color.white;
        ragdoll = false;
    }

    void StartRagdoll()
    {
        ragdoll = true;
        spriteRenderer.color = Color.red;
        rb.constraints = unfreezeZConstraint;
    }

}
