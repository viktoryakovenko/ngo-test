using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public class PlayerFollow : NetworkBehaviour
    {
        public NavMeshAgent Agent;

        [SerializeField] private float _searchRadius = 30f;

        private Transform _heroTransform;

        private void Update() =>
            FindClosestPlayer();

        private void SetDestinationForAgent()
        {
            if (_heroTransform)
                Agent.destination = _heroTransform.position;
        }

        private void FindClosestPlayer()
        {
            float minDistance = Mathf.Infinity;
            Transform closest = null;

            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var player = client.PlayerObject;

                if (player == null)
                    continue;

                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < minDistance && distance <= _searchRadius)
                {
                    minDistance = distance;
                    closest = player.transform;
                }
            }

            if (closest)
                Agent.destination = closest.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);
        }
    }
}
