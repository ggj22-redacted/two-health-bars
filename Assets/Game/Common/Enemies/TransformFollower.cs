using System;
using UnityEngine;

namespace Game.Common.Enemies
{
    [RequireComponent(typeof(Collider))]
    public class TransformFollower : MonoBehaviour
    {
        [SerializeField]
        private MovementControl movementControl;

        [SerializeField, Min(0)]
        private float minDistance;

        [SerializeField, Min(0)]
        private float maxDistance;

        private void OnTriggerStay (Collider other)
        {
            Vector3 targetPosition = other.transform.position;
            Vector3 displacement = targetPosition - transform.position;
            float distance = displacement.magnitude;
            Vector3 direction = displacement / distance;

            if (distance > maxDistance) {
                movementControl.MoveTo(targetPosition + displacement - direction * minDistance);
                return;
            }

            if (distance < minDistance)
                movementControl.MoveTo(targetPosition - direction * minDistance);
        }
    }
}