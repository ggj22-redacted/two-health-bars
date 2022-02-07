using System.Collections;
using Game.Common.Enemies;
using Game.Common.Projectiles;
using Game.Common.Shooting;
using UnityEngine;
using Zenject;

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

        [Inject]
        private EntityState _entityState;

        private Coroutine _hitHandler;

        public void OnHit (ProjectileState state)
        {
            if (_hitHandler == null && gameObject.activeSelf && gameObject.activeInHierarchy && !_entityState.IsDead)
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