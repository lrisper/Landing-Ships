using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // move ship
            print("Thrusting");
            _rigidbody.AddRelativeForce(Vector3.up);
        }

        if (Input.GetKey(KeyCode.A))
        {
            // rotate left
            Debug.Log("Rotating Left");
        }

        else if (Input.GetKey(KeyCode.D))
        {
            // rotate right
            Debug.Log("Rotating Right");
        }
    }
}
