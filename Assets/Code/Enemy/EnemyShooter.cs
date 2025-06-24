using System.Collections;
using Code.Bullet;
using Code.Infrastructure.AssetManagement;
using Code.Player;
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
        private NetworkObject _bulletPrefab;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _bulletPrefab = Resources.Load<NetworkObject>(AssetPath.BulletPrefab);
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
            Vector3 spawnPos = transform.position + direction.normalized;
            var bulletInstance = Instantiate(_bulletPrefab, spawnPos, Quaternion.LookRotation(direction));
            bulletInstance.Spawn();

            bulletInstance.GetComponent<MoveForward>().Initialize(direction);
        }
    }
}
