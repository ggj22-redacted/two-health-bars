using UnityEngine;
using UnityEngine.AI;

namespace Game.Common.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementControl : MonoBehaviour
    {
        private NavMeshAgent _agent;

        public bool IsNavigationAllowed { get; set; } = true;

        private void Awake ()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo (Vector3 position)
        {
            if (_agent.enabled && _agent.isOnNavMesh && IsNavigationAllowed)
                _agent.SetDestination(position);
        }

        public void Stop ()
        {
            if (_agent.enabled && _agent.isOnNavMesh && IsNavigationAllowed)
                _agent.SetDestination(_agent.nextPosition);
        }
    }
}