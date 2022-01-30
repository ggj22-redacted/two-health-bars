using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Entities
{
    public class EntityHealthControl : MonoBehaviour, IHittable
    {
        [SerializeField]
        private EntityState entityState;

        void IHittable.OnHit (ProjectileState state)
        {
            HandleHealth(state);
        }

        private void HandleHealth (ProjectileState state)
        {
            if (entityState.Shield > 0)
            {
                float overDamage = state.Damage - entityState.Shield;
                entityState.Shield -= state.Damage;

                if (overDamage > 0)
                    state.Damage = overDamage;
                else
                    return;
            }

            entityState.Health -= state.Damage;
        }
    }
}