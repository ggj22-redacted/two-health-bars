using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Common.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementControl : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake ()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo (Vector3 position)
        {
            _agent.SetDestination(position);
        }

        public void Stop ()
        {
            _agent.SetDestination(_agent.nextPosition);
        }
    }
}