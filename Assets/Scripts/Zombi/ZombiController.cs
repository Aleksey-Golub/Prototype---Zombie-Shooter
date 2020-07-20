using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZombiMover), typeof(Zombi))]
public class ZombiController : MonoBehaviour
{
    [SerializeField] private float _attackDistance;

    private ZombiMover _mover;
    private Zombi _zombi;
    private float _distantToTarget;
    private bool _canAttack = true;

    [HideInInspector] public Transform Target;

    private void Start()
    {
        _mover = GetComponent<ZombiMover>();
        _zombi = GetComponent<Zombi>();
    }

    private void Update()
    {
        _distantToTarget = Vector3.Distance(transform.position, Target.position);

        transform.LookAt(Target.position);

        if (_distantToTarget >= _attackDistance)
        {
            _mover.Move(Target.position);
        }
        else
        {
            if (_canAttack)
            {
                StartCoroutine(AttackCor(Target));
            }
        }
    }

    private IEnumerator AttackCor(Transform target)
    {
        var d = new WaitForSeconds(_zombi.DelayAttack);

        Attack(target);

        _canAttack = false;
        yield return d;
        _canAttack = true;
    }

    private void Attack(Transform target)
    {
        if (target.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(_zombi.MeleeDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
