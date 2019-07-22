using System;
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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!_isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _collisionsDisabled = !_collisionsDisabled; // toggle
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_isTransitioning || _collisionsDisabled) { return; }

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
        _isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_death);
        _deathParticles.Play();
        Invoke("LoadFirstLevel", _levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        _isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_success);
        _successParticles.Play();
        Invoke("LoadNextLevel", _levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // loop back to start
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        _audioSource.Stop();
        _mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        _rigidbody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);

        if (!_audioSource.isPlaying) // so it don't layer
        {
            _audioSource.PlayOneShot(_mainEngine);
        }
        _mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(_rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-_rcsThrust * Time.deltaTime);
        }
    }
    private void RotateManually(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true; // take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        _rigidbody.freezeRotation = false; // resume physics control of rotation
    }
}








