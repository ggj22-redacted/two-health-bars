using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Entities
{
    public class EntityHealthControl : MonoBehaviour, IHittable
    {
        [SerializeField]
        private EntityState entityState;
        public Transform HPBar;

        void IHittable.OnHit (ProjectileState state)
        {
            HandleHealth(state);
        }

        private void HandleHealth (ProjectileState state)
        {
            entityState.Health -= state.Damage;


            // TODO: Add some kind of animation
            if (entityState.Health <= 0)
                entityState.gameObject.SetActive(false);
        }
    }
}