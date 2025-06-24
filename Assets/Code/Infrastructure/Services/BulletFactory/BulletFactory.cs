using Code.Bullet;
using Code.Infrastructure.AssetManagement;
using Unity.Netcode;
using UnityEngine;

namespace Code.Infrastructure.Services.BulletFactory
{
    public class BulletFactory : IBulletFactory
    {
        private NetworkObject _bulletPrefab;

        public BulletFactory() =>
            _bulletPrefab = Resources.Load<NetworkObject>(AssetPath.BulletPrefab);

        public void Create(Vector3 spawnPosition, Vector3 direction)
        {
            var bulletInstance = Object.Instantiate(_bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
            bulletInstance.Spawn();

            bulletInstance.GetComponent<MoveForward>().Initialize(direction);
        }
    }
}
