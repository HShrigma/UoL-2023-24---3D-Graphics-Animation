using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [NonSerialized] public bool Standing;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Standing = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfStanding();
    }

    private void CheckIfStanding()
    {
        if (Standing)
        {
            if (rb.rotation.x.ToString("0.0") != "-0.7")
            {
                Standing = false;
            }
        }

    }
}
