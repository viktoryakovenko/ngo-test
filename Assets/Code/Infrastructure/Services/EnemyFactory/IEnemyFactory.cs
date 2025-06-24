using Unity.Netcode;
using UnityEngine;

namespace Code.Infrastructure.Services.EnemyFactory
{
    public interface IEnemyFactory
    {
        NetworkObject Create(Vector3 position);
    }
}
