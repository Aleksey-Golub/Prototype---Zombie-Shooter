using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Vector3 _newPosition;

    private void Start()
    {
        _newPosition = transform.position;
    }

    private void Update()
    {
        transform.position = _target.position + _offset;
    }
}
