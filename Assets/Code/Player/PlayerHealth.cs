using System;
using Code.Infrastructure.AssetManagement;
using Code.NetworkData;
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
            if (!IsOwner) return;

            var config = Resources.Load<PlayerConfig>(AssetPath.HeroConfigPath);
            InitializeHealthServerRpc(config.MaxHealth, config.MaxHealth);
            _health.OnValueChanged += HandleHealthChanged;
        }

        public override void OnNetworkDespawn() =>
            _health.OnValueChanged -= HandleHealthChanged;

        [ServerRpc]
        public void TakeDamageServerRpc(int amount)
        {
            var current = _health.Value;
            current.Current = Mathf.Max(0, current.Current - amount);
            _health.Value = current;
        }

        private void HandleHealthChanged(Health previous, Health current) =>
            OnHealthChanged?.Invoke(current.Current);

        [ServerRpc]
        private void InitializeHealthServerRpc(int current, int max)
        {
            _health.Value = new Health
            {
                Max = current,
                Current = max
            };
        }
    }
}
