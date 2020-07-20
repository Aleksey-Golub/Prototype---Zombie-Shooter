using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class MouseTracker : MonoBehaviour
{
    [SerializeField] private Transform _gun;
    [SerializeField] private LayerMask _layerMask;

    private Camera _camera;
    private Vector3 _lookAtTarget = new Vector3();
    private PlayerInput _input;

    /*// test markers //можно включить для визуализации точек прицеливания
    [SerializeField] private Transform _test1;
    [SerializeField] private Transform _test2;*/

    private void Start()
    {
        _camera = Camera.main;
        _input = GetComponent<PlayerController>().PlayerInput;
    }

    private void Update()
    {
        Vector2 screenMousePosition = _input.Player.MousePosition.ReadValue<Vector2>();
        Ray ray = _camera.ScreenPointToRay(screenMousePosition);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue, _layerMask);

        _lookAtTarget.x = hit.point.x;
        _lookAtTarget.y = transform.position.y;
        _lookAtTarget.z = hit.point.z;

        transform.LookAt(_lookAtTarget);

        if (Vector3.Distance(_lookAtTarget, _gun.position) > 3)
            _gun.LookAt(_lookAtTarget);

        /*// test
        _test1.position = hit.point;
        _test2.position = _lookAtTarget;*/
    }
}
