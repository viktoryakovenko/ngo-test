using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrolAgent : NetworkBehaviour
    {
        [SerializeField] private float _patrolRadius;
        [SerializeField] private float _idleTime;
        [SerializeField] private float _reachThreshold;
        [SerializeField] private NavMeshAgent _agent;

        private Vector3 _startPosition;
        private float _idleTimer;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            _agent = GetComponent<NavMeshAgent>();
            _startPosition = transform.position;

            SetNewDestination();
        }

        private void Update()
        {
            if (!IsServer) return;

            if (!_agent.pathPending && _agent.remainingDistance <= _reachThreshold)
            {
                _idleTimer += Time.deltaTime;
                if (_idleTimer >= _idleTime)
                {
                    SetNewDestination();
                    _idleTimer = 0f;
                }
            }
        }

        private void SetNewDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _patrolRadius;
            randomDirection += _startPosition;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }
    }
}
