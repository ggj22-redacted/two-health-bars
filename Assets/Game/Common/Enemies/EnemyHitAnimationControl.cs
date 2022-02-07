using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Enemies
{
    public class EnemyHitAnimationControl : MonoBehaviour, IHittable
    {
        private static readonly int Hit = Animator.StringToHash("Hit");

        [SerializeField]
        private Animator animator;

        [SerializeField, Min(0)]
        private float cooldown;

        private float _nextAllowedTriggerMoment = -1;

        public void OnHit (ProjectileState state)
        {
            if (Time.time < _nextAllowedTriggerMoment)
                return;

            animator.SetTrigger(Hit);

            _nextAllowedTriggerMoment = Time.time + cooldown;
        }
    }
}