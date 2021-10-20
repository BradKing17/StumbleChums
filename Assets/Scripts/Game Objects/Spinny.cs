using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinny : MonoBehaviour
{
    public float rotationSpeed = 1.0F;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.angularVelocity = Vector3.up * rotationSpeed;
    }
}
