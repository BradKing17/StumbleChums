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

    public bool GMisTopDown;
    //public bool isTopDown = false;
    
    public Rigidbody rb;
    public CapsuleCollider cc;
    public SpriteRenderer spr;
    public SphereCollider grabRange;
    public List<Collider> grabbables;
    public SpringJoint joint;
    public CapsuleCollider triggerCapsuleCollider;

    private Vector2 moveInput;
    private float grabInput;
    public bool grounded;
    private bool canDive = true;
    private bool canGrab = true;
    public bool ragdoll = false;
    public SpriteRenderer spriteRenderer;
    private bool ragdollFlashing;

    private RigidbodyConstraints standardContraint;
    private RigidbodyConstraints unfreezeZConstraint;
    public LayerMask groundLayer;
    
    
    private void OnEnable()
    {
        // This will check if the GAME MANAGER object says whether the scene is Top Down
        GMisTopDown = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>().topDown;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        // So if it is top down the PosZ constraint is unfrozen
        if (GMisTopDown)
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
        if (GMisTopDown)
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

        if (ragdoll && grounded)
        {
            StartCoroutine(RagdollCatch());
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * (cc.height / 2.0F + 0.05F), Color.green);
        return Physics.Raycast(transform.position, Vector3.down, cc.height / 2.0F + 0.05F, groundLayer);
    }

    public void SetPlayerType(PlayerType _type)
    {
        playerType = _type;
        spr.sprite = _type.sprites.front;
    }

    public void SetTopDown(bool _topDown)
    {
        GMisTopDown = _topDown;
        rb.constraints = GMisTopDown ? 
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
        if (!GMisTopDown && _value.phase == InputActionPhase.Started && IsGrounded())
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

    private void OnCollisionEnter(Collision other)
    {
        //print("ding");
        if (other.gameObject.CompareTag("ragdoll"))
        {
            StartCoroutine(StartRagdoll());
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("ragdoll") && grounded)
        {
            StartCoroutine(EndRagdoll());
        }
    }

    IEnumerator EndRagdoll()
    {
        yield return new WaitForSeconds(.6F);
        rb.AddForce(transform.up, ForceMode.Force);
        gameObject.transform.rotation = new Quaternion(0, 0, 0,0);
        rb.constraints = standardContraint;
        spriteRenderer.color = Color.white;
        ragdoll = false;
    }

    IEnumerator StartRagdoll()
    {
        ragdoll = true;
        spriteRenderer.color = Color.red;
        rb.constraints = unfreezeZConstraint;
        yield return new WaitForSeconds(.1F);
    }

    // This should(?) catch whether the chum is on the ground and ragdolling, but not
    // getting back up
    IEnumerator RagdollCatch()
    {
        yield return new WaitForSeconds(.3F);
        StartCoroutine(RagdollCatch());
    }
}
