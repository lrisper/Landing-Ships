using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _rcsThrust = 100;
    [SerializeField] float _mainThrust = 100;
    [SerializeField] float _levelLoadDelay = 2f;

    [SerializeField] AudioClip _mainEngine;
    [SerializeField] AudioClip _success;
    [SerializeField] AudioClip _death;

    [SerializeField] ParticleSystem _mainEngineParticles;
    [SerializeField] ParticleSystem _successParticles;
    [SerializeField] ParticleSystem _deathParticles;

    Rigidbody _rigidbody;
    AudioSource _audioSource;

    bool _isTransitioning = false;
    bool _collisionsDisabled = false;

    enum State
    {
        Alive,
        Dieing,
        Transcending

    }
    State _state = State.Alive;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;

        }

    }

    private void StartDeathSequence()
    {
        _state = State.Dieing;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_death);
        _deathParticles.Play();
        Invoke("LoadFirstLevel", 1);
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_success);
        _successParticles.Play();
        Invoke("LoadNextLevel", 1f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            _audioSource.Stop();
            _mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rigidbody.AddRelativeForce(Vector3.up * _mainThrust);

        if (!_audioSource.isPlaying) // so it don't layer
        {
            _audioSource.PlayOneShot(_mainEngine);
        }
        _mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
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









