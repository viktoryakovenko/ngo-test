using System.Collections;
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

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                _attackLoop = StartCoroutine(AttackRoutine());
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
                var health = closest.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(_damage);
                    Debug.Log($"Player {health.GetComponent<NetworkObject>().OwnerClientId} current HP - {health.Current}");
                }
            }
        }
    }
}
