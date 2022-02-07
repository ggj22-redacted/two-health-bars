using System.Collections;
using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Player
{
    public class PlayerHitControl : MonoBehaviour, IHittable
    {
        [SerializeField]
        private float invincibilityTime;

        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private Coroutine _hitHandler;

        public void OnHit (ProjectileState state)
        {
            _hitHandler ??= StartCoroutine(HandleHit());
        }

        private IEnumerator HandleHit ()
        {
            characterController.detectCollisions = false;

            yield return new WaitForSeconds(invincibilityTime);

            characterController.detectCollisions = true;

            _hitHandler = null;
        }
    }
}