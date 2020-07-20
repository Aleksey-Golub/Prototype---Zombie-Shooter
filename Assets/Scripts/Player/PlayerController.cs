using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerInput PlayerInput;

    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private ParticleSystem _hitEffectZombi;
    [SerializeField] private ParticleSystem _hitEffectWalls;
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _grenadeThrowDelay;

    private float _moveSpeed;
    private Player _player;
    private bool _canThrowGrenade = true;
    private UIManager _uIManager;

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        _player = GetComponent<Player>();
        _uIManager = FindObjectOfType<UIManager>();

        PlayerInput.Player.QuiteGame.performed += context => Application.Quit();
        PlayerInput.Player.HelpMenu.performed += context => _uIManager.OpenHelpMenu();
        PlayerInput.Player.Shoot.performed += context => Shoot();
        PlayerInput.Player.ThrowGrenade.performed += context => {
            if (_canThrowGrenade)
                StartCoroutine(ThrowGrenadeCor(_grenadeThrowDelay));
        };
    }

    private void OnEnable()
    {
        PlayerInput.Enable();    
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
    }

    private void Update()
    {
        Vector2 moveDirection = PlayerInput.Player.Move.ReadValue<Vector2>();

        Move(moveDirection);
    }

    private void Move(Vector2 direction)
    {
        _moveSpeed = (PlayerInput.Player.Sprint.activeControl != null) ? _moveSpeed = _runningSpeed : _moveSpeed = _defaultSpeed;

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        transform.position += _moveSpeed * Time.deltaTime * moveDirection;
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent(out Zombi zombi))
            {
                ShowHitEffect(_hitEffectZombi, hit.point);

                zombi.TakeDamage(_player.RangeDamage);
            }
            else
            {
                ShowHitEffect(_hitEffectWalls, hit.point);
            }
        }
    }

    private IEnumerator ThrowGrenadeCor(float delay)
    {
        var d = new WaitForSeconds(delay);

        ThrowGrenade();

        _canThrowGrenade = false;
        yield return d;
        _canThrowGrenade = true;
    }

    private void ThrowGrenade()
    {
        GameObject grenade = Instantiate(_grenadePrefab, _shootPoint.position, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce((_shootPoint.forward + _shootPoint.up) * _throwForce, ForceMode.VelocityChange);
    }

    private void ShowHitEffect(ParticleSystem particle, Vector3 spawnPosition)
    {
        var w = Instantiate(particle, spawnPosition, Quaternion.LookRotation(transform.position - spawnPosition));
        Destroy(w.gameObject, 1);
    }
}
