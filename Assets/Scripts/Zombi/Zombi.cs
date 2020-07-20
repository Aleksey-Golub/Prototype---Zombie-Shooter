using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombi : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _minHealth;
    [SerializeField] private int _meleeDamage;
    [SerializeField] private float _delayAttack;
    [SerializeField] private ParticleSystem _deathEffect;
    [SerializeField] private GameObject _destroidZombi;

    private int _currentHealth;

    public int MeleeDamage => _meleeDamage;
    public float DelayAttack => _delayAttack;
    public event UnityAction<Zombi> Died;

    private void Start()
    {
        _currentHealth = Random.Range(_minHealth, _maxHealth + 1);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        var w = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(w.gameObject, 1);

        Destroy(gameObject);
        Instantiate(_destroidZombi, transform.position, transform.rotation);


        Died?.Invoke(this);
    }
}
