using Unity.Netcode;
using UnityEngine;

namespace Code.Enemy
{
    public class TargetSelector : NetworkBehaviour
    {
        [SerializeField] private float searchRadius;

        private Transform _currentTarget;

        public Transform CurrentTarget => _currentTarget;

        public void FindClosestTarget(Vector3 position)
        {
            float minDistance = Mathf.Infinity;
            Transform closest = null;

            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var player = client.PlayerObject;
                if (player == null)
                    continue;

                float distance = Vector3.Distance(position, player.transform.position);
                if (distance < minDistance && distance <= searchRadius)
                {
                    minDistance = distance;
                    closest = player.transform;
                }
            }

            _currentTarget = closest;
        }
    }
}
