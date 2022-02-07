using System.Collections;
using Game.Common.Enemies;
using Game.Common.Projectiles;
using Game.Common.Shooting;
using UnityEngine;

namespace Game.Common.Entities
{
    public class EntityHitControl : MonoBehaviour, IHittable
    {
        [SerializeField, Min(0)]
        private float invulnerabilityTime;

        [SerializeField]
        private new Collider collider;

        [SerializeField]
        private MovementControl movementControl;

        [SerializeField]
        private ShootingControl shootingControl;

        private Coroutine _hitHandler;

        public void OnHit (ProjectileState state)
        {
            if (_hitHandler == null && gameObject.activeSelf && gameObject.activeInHierarchy)
                _hitHandler = StartCoroutine(HandleHit());
        }

        private IEnumerator HandleHit ()
        {
            shootingControl.IsShootingAllowed = false;
            movementControl.IsNavigationAllowed = false;
            collider.enabled = false;

            yield return new WaitForSeconds(invulnerabilityTime);

            shootingControl.IsShootingAllowed = true;
            movementControl.IsNavigationAllowed = true;
            collider.enabled = true;

            _hitHandler = null;
        }
    }
}