using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Services.EnemyFactory;
using Code.Infrastructure.Services.SpawnPointGenerator;
using Unity.Netcode;
using UnityEngine;

namespace Code.Logic
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private float spawnInterval;
        [SerializeField] private float spawnRadius;

        private ISpawnPointGenerator _spawnPointGenerator;
        private IEnemyFactory _enemyFactory;
        private NetworkObject _enemyPrefab;
        private float _timer;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            _enemyPrefab = Resources.Load<NetworkObject>(AssetPath.EnemyPrefabPath);
            if (_enemyPrefab == null)
                enabled = false;

            _enemyFactory = new EnemyFactory(_enemyPrefab);
            _spawnPointGenerator = new SpawnPointGenerator(transform.position, spawnRadius);
        }

        private void Update()
        {
            if (!IsServer) return;

            _timer += Time.deltaTime;
            if (_timer >= spawnInterval)
            {
                _timer = 0f;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPos = _spawnPointGenerator.GetRandomPosition();

            NetworkObject enemyInstance = _enemyFactory.Create(spawnPos);
            enemyInstance.Spawn();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
