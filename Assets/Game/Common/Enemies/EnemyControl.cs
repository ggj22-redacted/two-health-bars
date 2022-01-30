﻿using UnityEngine;
using Game.Common.Shooting;

namespace Game.Common.Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyControl : MonoBehaviour
    {
        [SerializeField]
        private Transform referenceTransform;

        [SerializeField]
        private EntityState entityState;

        [SerializeField]
        private MovementControl movementControl;

        [SerializeField, Min(0)]
        private float minDistance;

        [SerializeField, Min(0)]
        private float maxDistance;

        [SerializeField]
        private float accuracyUpdateRate;

        private Vector3 _previousTargetPosition;

        private Vector3 _previousPredictedPosition;

        private Transform _target;

        private CharacterController _characterController;

        private ShootingControl _shootingControl;

        private float accuracy;

        private static Vector3 GetPredictedTargetPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
        {
            Vector3 displacement = targetPosition - shooterPosition;
            float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
            //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
            if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
            {
                Debug.Log("Position prediction is not feasible.");
                return targetPosition;
            }
            //also Sine Formula
            float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);

            return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
        }

        private void Start()
        {
            _shootingControl = gameObject.GetComponent<ShootingControl>();
        }

        private void OnTriggerStay (Collider other)
        {
            _target = other.transform;
            _characterController = other.GetComponent<CharacterController>();

            Vector3 targetPosition = _target.position;
            Vector3 displacement = targetPosition - transform.position;
            float distance = displacement.magnitude;
            Vector3 direction = displacement / distance;

            if (distance > maxDistance) {
                movementControl.MoveTo(targetPosition + displacement - direction * minDistance);
                return;
            }

            _shootingControl.SetShooting(entityState.allowfire);

            if (distance < minDistance)
                movementControl.MoveTo(targetPosition - direction * minDistance);
        }

        private void OnTriggerExit (Collider other)
        {
            _target = null;

            _shootingControl.SetShooting(false);
        }

        private void FixedUpdate ()
        {
            HandleRotation();
        }

        private void HandleRotation ()
        {
            if (!_target)
                return;

            Vector3 targetPosition;
            if (_characterController) {
                targetPosition = _target.position + _target.up * _characterController.height;
            } else {
                targetPosition = _target.position;
            }

            Vector3 position = referenceTransform.position;
            Vector3 displacement = targetPosition - _previousTargetPosition;
            Vector3 velocity = displacement / Time.fixedDeltaTime;
            Vector3 predictedTargetPosition = GetPredictedTargetPosition(targetPosition, position, velocity, entityState.ProjectileState.Speed);

            Vector3 lookPosition = Vector3.LerpUnclamped(targetPosition, predictedTargetPosition, accuracy);
            referenceTransform.rotation = Quaternion.LookRotation(lookPosition - position);

            UpdateAccuracy(_previousTargetPosition, targetPosition, predictedTargetPosition);

            _previousTargetPosition = targetPosition;
            _previousPredictedPosition = predictedTargetPosition;
        }

        private void UpdateAccuracy (Vector3 startPosition, Vector3 actualPosition, Vector3 predictedPosition)
        {
            Vector3 displacement = actualPosition - startPosition;
            Vector3 predictedDisplacement = predictedPosition - startPosition;
            float dot = Vector3.Dot(displacement, predictedDisplacement);

            accuracy += dot * accuracyUpdateRate;
            accuracy = Mathf.Clamp(accuracy, 0, 1);
        }
    }
}