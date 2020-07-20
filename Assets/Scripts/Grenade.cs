using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private int _grenadeDamage;

    private float _countdown;
    private bool _isExploded = false;

    private void Start()
    {
        _countdown = _delay;
    }

    private void Update()
    {
        _countdown -= Time.deltaTime;

        if (_countdown <= 0 && !_isExploded)
        {
            MakeBoom();
            _isExploded = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    private void MakeBoom()
    {
        var explosionEffect = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosionEffect.gameObject, 3f);

        // find obj to damage and damage it
        Collider[] collidersToDamage = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var c in collidersToDamage)
        {
            var zombi = c.GetComponent<Zombi>();
            if (zombi != null)
            {
                zombi.TakeDamage(_grenadeDamage);
            }
        }

        // find obj to move and move
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var c in collidersToMove)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
