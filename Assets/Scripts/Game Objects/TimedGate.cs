using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    public GameObject gate;
    public float delay = 0.0F;
    public float openTime = 1.5F;
    public float closeTime = 1.5F;
    private float timer = 0.0F;
    private bool open;

    private void Start()
    {
        timer = -delay;
        open = !gate.activeSelf;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = open ? openTime : closeTime;
        if (timer >= t)
        {
            open = !open;
            gate.SetActive(!open);
            timer -= t;
        }
    }
}
