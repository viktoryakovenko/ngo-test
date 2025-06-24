using UnityEngine;

namespace Code.Infrastructure.Services.BulletFactory
{
    public interface IBulletFactory
    {
        void Create(Vector3 spawnPosition, Vector3 direction);
    }
}
