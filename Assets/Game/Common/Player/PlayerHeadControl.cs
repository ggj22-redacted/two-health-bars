using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Common.Player
{
    public class PlayerHeadControl : MonoBehaviour
    {
        [SerializeField]
        private Transform reference;

        [SerializeField]
        private Transform directionReference;

        [SerializeField]
        private Transform headPivot;

        [SerializeField, Min(0)]
        private float minTargetDistance;

        [SerializeField, Min(0)]
        private float maxTargetDistance;

        [Inject]
        private Camera _mainCamera;

        [Inject]
        private EntityState _entityState;

        private Vector3 _target;

        private Vector3 _previousPosition;

        private Vector3 _offsetDirection;

        private float _offset;

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
            _offsetDirection = transform.position - headPivot.position;
            _offset = _offsetDirection.magnitude;
            _offsetDirection /= _offset;
        }

        private void Update ()
        {
            Vector3 position = reference.position;

            Vector3 origin = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))
                             + _mainCamera.transform.forward * minTargetDistance;
            Vector3 velocity = Vector3.zero;
            if (Physics.Raycast(origin, _mainCamera.transform.forward, out RaycastHit hit, maxTargetDistance, LayerMask.GetMask("Enemy", "Obstacles"))) {
                _target = hit.point;
                NavMeshAgent agent = hit.transform.GetComponent<NavMeshAgent>();
                if (agent)
                    velocity = agent.velocity;
            } else {
                _target = origin + _mainCamera.transform.forward * maxTargetDistance;
                velocity = (position - _previousPosition) / Time.deltaTime;
            }

            _target = GetPredictedTargetPosition(_target, _entityState.transform.position, velocity, _entityState.ProjectileState.Speed);

            Vector3 direction = _target - headPivot.position;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(_offset, distance);

            direction /= distance;
            Quaternion rotation = Quaternion.AngleAxis(angle, headPivot.right /* Vector3.Cross(direction, _offsetDirection) */);

            headPivot.rotation = rotation * Quaternion.LookRotation(direction);
            //transform.rotation = rotation * headPivot.rotation;
            _previousPosition = position;
        }
    }
}