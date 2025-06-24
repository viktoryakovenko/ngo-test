using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDeath : NetworkBehaviour
    {
        public event Action OnDeath;

        public PlayerHealth Health;

        private bool _isDead;

        public override void OnNetworkSpawn() =>
            Health.OnHealthChanged += HealthChanged;

        public override void OnNetworkDespawn()
        {
            if (Health != null)
                Health.OnHealthChanged -= HealthChanged;
        }

        private void HealthChanged(int current)
        {
            if (!_isDead && current <= 0)
                Die();
        }

        private void Die()
        {
            if (!IsServer) return;

            Debug.Log($"{name} died.");
            OnDeath?.Invoke();
        }
    }
}
