using UnityEngine;

namespace Code.Infrastructure.Services.SpawnPointGenerator
{
    public class SpawnPointGenerator : ISpawnPointGenerator
    {
        private readonly Vector3 _center;
        private readonly float _radius;

        public SpawnPointGenerator(Vector3 center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        public Vector3 GetRandomPosition()
        {
            Vector3 random = Random.insideUnitSphere * _radius;
            random.y = 0;
            return _center + random;
        }
    }
}
