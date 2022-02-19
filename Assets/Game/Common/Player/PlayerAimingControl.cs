using System;
using UnityEngine;
using Zenject;

namespace Game.Common.Player
{
    public class PlayerAimingControl : MonoBehaviour
    {
        [SerializeField]
        private Transform aimingTransform;

        [Header("Settings")]
        [SerializeField, Min(0)]
        private float minTargetDistance;

        [SerializeField, Min(0)]
        private float maxTargetDistance;

        [SerializeField, Range(1, 20)]
        private float rotationSpeed = 1;

        [SerializeField, Min(0)]
        private int velocityDampening;

        [Inject]
        private Camera _mainCamera;

        [Inject]
        private EntityState _entityState;

        private Vector3 _previousPosition;

        private Vector3 _offsetDirection;

        private float _offset;

        private Vector3[] _velocities = Array.Empty<Vector3>();

        public Vector3 TargetPosition { get; private set; }

        private static Vector3 GetPredictedTargetPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
        {
            Vector3 displacement = targetPosition - shooterPosition;
            float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
            //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
            if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
                return targetPosition;
            //also Sine Formula
            float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);

            return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
        }

        private void Start ()
        {
            _offsetDirection = transform.position - aimingTransform.position;
            _offset = _offsetDirection.magnitude;
            _offsetDirection /= _offset;
            _velocities = new Vector3[velocityDampening];
        }

        private void Update ()
        {
            //Vector3 position = reference.position;

            Vector3 origin = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))
                             + _mainCamera.transform.forward * minTargetDistance;
            //Vector3 velocity = (position - _previousPosition) / Time.fixedDeltaTime; //characterController.velocity;
            Vector3 hitPosition;
            if (Physics.Raycast(origin, _mainCamera.transform.forward, out RaycastHit hit, maxTargetDistance, LayerMask.GetMask("Enemy", "Obstacles"))) {
                hitPosition = hit.point;

                /*
                 NavMeshAgent agent = hit.transform.GetComponent<NavMeshAgent>();
                if (agent)
                    velocity += agent.velocity;
                */
            } else {
                hitPosition = origin + _mainCamera.transform.forward * maxTargetDistance;
            }

            Vector3 velocity = Vector3.zero, currentVelocity = (hitPosition - _previousPosition) / Time.deltaTime;
            if (_velocities.Length > 0) {
                velocity = _velocities[0];

                for (int i = 1; i < velocityDampening; i++) {
                    _velocities[i - 1] = _velocities[i];
                    velocity += _velocities[i];
                }

                _velocities[velocityDampening - 1] = currentVelocity;
            }

            velocity = (velocity + currentVelocity) / (velocityDampening + 1f);

            Vector3 shootPosition = aimingTransform.position;
            TargetPosition = GetPredictedTargetPosition(hitPosition, shootPosition, velocity, _entityState.ProjectileState.Speed);

            Vector3 direction = TargetPosition - shootPosition;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(_offset, distance);

            direction /= distance;
            Quaternion rotation = Quaternion.AngleAxis(angle, aimingTransform.right) * Quaternion.LookRotation(direction);

            aimingTransform.rotation = Quaternion.Slerp(aimingTransform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);

            //_previousPosition = position;
            _previousPosition = hitPosition;
        }

        private void OnValidate ()
        {
            Vector3[] newVelocities = new Vector3[velocityDampening];
            if (velocityDampening <= _velocities.Length) {
                int offset = _velocities.Length - velocityDampening;
                for (int i = 0; i < velocityDampening; i++)
                    newVelocities[i] = _velocities[i + offset];
            } else {
                int offset = velocityDampening - _velocities.Length;
                for (int i = 0; i < _velocities.Length; i++)
                    newVelocities[i + offset] = _velocities[i];
            }
        }

        private void OnDrawGizmos ()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TargetPosition, 1);
        }
    }
}