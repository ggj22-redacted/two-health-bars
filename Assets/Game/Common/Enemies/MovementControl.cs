using UnityEngine;
using UnityEngine.AI;

namespace Game.Common.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementControl : MonoBehaviour
    {
        private NavMeshAgent _agent;
    
        private bool _isNavigationAllowed = true;

        public bool IsNavigationAllowed
        {
            get => _isNavigationAllowed;
            set
            {
                if (!value && _isNavigationAllowed && _agent.enabled && _agent.isOnNavMesh)
                    _agent.destination = _agent.nextPosition;
                _isNavigationAllowed = value;
            }
        }

        private void Awake ()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo (Vector3 position)
        {
            if (_agent.enabled && _agent.isOnNavMesh && _isNavigationAllowed)
                _agent.SetDestination(position);
        }

        public void Stop ()
        {
            if (_agent.enabled && _agent.isOnNavMesh && _isNavigationAllowed)
                _agent.SetDestination(_agent.nextPosition);
        }
    }
}