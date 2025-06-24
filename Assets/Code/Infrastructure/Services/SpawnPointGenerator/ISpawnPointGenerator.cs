using UnityEngine;

namespace Code.Infrastructure.Services.SpawnPointGenerator
{
    public interface ISpawnPointGenerator
    {
        Vector3 GetRandomPosition();
    }
}
