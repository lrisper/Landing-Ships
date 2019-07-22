using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _rcsThrust = 100;
    [SerializeField] float _mainThrust = 100;

    Rigidbody _rigidbody;
    AudioSource _audioSource;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                print("Fuel");
                SceneManager.LoadScene(1);
                break;
            default:
                print("Dead");
                SceneManager.LoadScene(0);
                break;

        }

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidbody.AddRelativeForce(Vector3.up * _mainThrust);

            if (!_audioSource.isPlaying) // so it don't layer
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }


    private void Rotate()
    {
        _rigidbody.freezeRotation = true; // take manual control
        float rotationThisFrame = _rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        _rigidbody.freezeRotation = false;
    }
}









