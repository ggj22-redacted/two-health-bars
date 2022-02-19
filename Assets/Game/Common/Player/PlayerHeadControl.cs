using UnityEngine;

namespace Game.Common.Player
{
    public class PlayerHeadControl : MonoBehaviour
    {
        [SerializeField]
        private Transform pivot;

        [SerializeField]
        private PlayerAimingControl aimingControl;

        [SerializeField, Range(1, 20)]
        private float rotationSpeed = 1;

        private void Update ()
        {
            Vector3 targetPosition = aimingControl.TargetPosition;
            Vector3 direction = (targetPosition - pivot.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            pivot.rotation = Quaternion.Slerp(pivot.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}