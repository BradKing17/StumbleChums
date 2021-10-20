using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinny : MonoBehaviour
{
    [System.Serializable]
    public struct SpinnerPart
    {
        public MeshRenderer part;
        public Material[] stages;
    }
    public float rotationSpeed = 1.0F;
    public SpinnerPart[] parts;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.angularVelocity = Vector3.up * rotationSpeed;
        float rot = transform.rotation.eulerAngles.y;
        float index = rot / 360.0F;
        foreach (SpinnerPart part in parts)
        {
            int num = part.stages.Length;
            float normalized = ((rotationSpeed > 0 ? 1 : -1) * index);
            part.part.material = part.stages[(uint) (normalized * num) % num];
        }
    }
}
