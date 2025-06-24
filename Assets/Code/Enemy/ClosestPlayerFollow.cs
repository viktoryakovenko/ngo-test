using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(TargetSelector))]
    public class ClosestPlayerFollow : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private TargetSelector _targetSelector;

        private void Update()
        {
            if (!IsServer) return;

            if (_targetSelector == null) return;

            _targetSelector.FindClosestTarget(transform.position);

            if (_targetSelector.CurrentTarget != null)
                _agent.destination = _targetSelector.CurrentTarget.position;
        }
    }
}
