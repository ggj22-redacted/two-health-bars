using StarterAssets;
using UnityEngine;

namespace Game.Common.Player
{
    public class PropertiesControl : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        [SerializeField]
        private ThirdPersonController controller;

        private void OnEnable ()
        {
            entityState.OnSpeedChanged += UpdateMovement;
            entityState.OnJumpHeightChanged += UpdateJumpHeight;
            entityState.OnGravityChanged += UpdateGravity;
        }

        private void OnDisable ()
        {
            entityState.OnSpeedChanged -= UpdateMovement;
            entityState.OnJumpHeightChanged -= UpdateJumpHeight;
            entityState.OnGravityChanged -= UpdateGravity;
        }

        private void UpdateMovement (float previousSpeed, float newSpeed)
        {
            controller.MoveSpeed = entityState.Speed;
            controller.SpeedChangeRate = entityState.Acceleration;
        }

        private void UpdateJumpHeight (float previousJumpHeight, float newJumpHeight)
        {
            controller.JumpHeight = entityState.JumpHeight;
        }

        private void UpdateGravity (float previousGravity, float newGravity)
        {
            controller.Gravity = entityState.Gravity;
        }
    }
}