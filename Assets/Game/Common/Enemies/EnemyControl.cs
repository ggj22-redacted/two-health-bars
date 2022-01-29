using UnityEngine;

namespace Game.Common.Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyControl : MonoBehaviour
    {
        [SerializeField]
        private Transform referenceTransform;

        [SerializeField]
        private MovementControl movementControl;

        [SerializeField, Min(0)]
        private float minDistance;

        [SerializeField, Min(0)]
        private float maxDistance;

        private Transform _target;

        private void OnTriggerStay (Collider other)
        {
            _target = other.transform;

            Vector3 targetPosition = _target.position;
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

        private void OnTriggerExit (Collider other)
        {
            _target = null;
        }

        private void Update ()
        {
            HandleRotation();
        }

        private void HandleRotation ()
        {
            if (!_target)
                return;

            referenceTransform.rotation = Quaternion.LookRotation(_target.position - referenceTransform.position);
        }
    }
}