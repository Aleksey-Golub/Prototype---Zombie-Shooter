using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiMover : MonoBehaviour
{
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private float _defaultSpeed;

    private void Start()
    {
        _defaultSpeed = Random.Range(_minSpeed, _maxSpeed);
    }

    public void Move(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * _defaultSpeed * Time.deltaTime;
    }
}
