using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 _movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float _period = 2f;
    [SerializeField] float _movementOffset = 0.5f;

    float movementFactor; // 0 for not moved, 1 for fully moved.
    Vector3 startingPos;

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_period <= Mathf.Epsilon) { return; } // protect against period is zero
        float cycles = Time.time / _period; // grows continually from 0

        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

        movementFactor = rawSinWave / 2f + _movementOffset;
        Vector3 offset = movementFactor * _movementVector;
        transform.position = startingPos + offset;
    }
}
