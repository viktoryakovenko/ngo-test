using System;
using Code.Infrastructure.AssetManagement;
using Code.StaticData;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        public event Action<int> OnHealthChanged;

        private NetworkVariable<Health> _health = new NetworkVariable<Health>();

        public override void OnNetworkSpawn()
        {
            var config = Resources.Load<PlayerConfig>(AssetPath.HeroPath);

            _health.Value = new Health
            {
                Max = config.MaxHealth,
                Current = config.MaxHealth,
            };

            _health.OnValueChanged += HandleHealthChanged;
        }

        public override void OnNetworkDespawn() =>
            _health.OnValueChanged -= HandleHealthChanged;

        private void HandleHealthChanged(Health previous, Health current) =>
            OnHealthChanged?.Invoke(current.Current);

        [ServerRpc]
        public void TakeDamageServerRpc(int amount)
        {
            var current = _health.Value;
            current.Current = Mathf.Max(0, current.Current - amount);
            _health.Value = current;
        }
    }
}
