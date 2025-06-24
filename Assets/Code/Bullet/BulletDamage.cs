using Code.Player;
using Unity.Netcode;
using UnityEngine;

namespace Code.Bullet
{
    public class BulletDamage : NetworkBehaviour
    {
        [SerializeField] private int _damage;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;

            if (other.TryGetComponent(out PlayerHealth health))
            {
                health.TakeDamage(_damage);
                NetworkObject.Despawn();
            }
        }
    }
}
