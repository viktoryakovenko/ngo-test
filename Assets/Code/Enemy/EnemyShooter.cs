using System.Collections;
using Code.Infrastructure.Services.BulletFactory;
using Unity.Netcode;
using UnityEngine;

namespace Code.Enemy
{
    [RequireComponent(typeof(TargetSelector))]
    public class EnemyShooter : NetworkBehaviour
    {
        [SerializeField] private TargetSelector _targetSelector;
        [SerializeField] private float _attackInterval;
        [SerializeField] private float _attackRadius;
        [SerializeField] private int _damage;

        private Coroutine _attackLoop;
        private IBulletFactory _bulletFactory;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _bulletFactory = new BulletFactory();
                _attackLoop = StartCoroutine(AttackRoutine());
            }
        }

        public override void OnNetworkDespawn()
        {
            if (_attackLoop != null)
                StopCoroutine(_attackLoop);
        }

        private IEnumerator AttackRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_attackInterval);
                TryShoot();
            }
        }

        private void TryShoot()
        {
            Transform closest = _targetSelector.CurrentTarget;
            if (closest == null) return;

            float distance = Vector3.Distance(transform.position, closest.position);
            if (distance <= _attackRadius)
            {
                Vector3 direction = (closest.position - transform.position).normalized;
                Shoot(direction);
            }
        }

        private void Shoot(Vector3 direction)
        {
            Vector3 spawnPosition = transform.position + direction.normalized;
            _bulletFactory.Create(spawnPosition, direction);
        }
    }
}
