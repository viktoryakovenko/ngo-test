using Unity.Netcode;
using UnityEngine;

namespace Code.Infrastructure.Services.EnemyFactory
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly NetworkObject _prefab;

        public EnemyFactory(NetworkObject prefab)
        {
            _prefab = prefab;
        }

        public NetworkObject Create(Vector3 position)
        {
            NetworkObject enemy = Object.Instantiate(_prefab, position, Quaternion.identity);
            return enemy;
        }
    }
}
