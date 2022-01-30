using Game.Common.Projectiles;
using UnityEngine;
using TMPro;

namespace Game.Common.Entities
{
    public class EntityHealthControl : MonoBehaviour, IHittable
    {
        [SerializeField]
        private EntityState entityState;
        public Transform HPBar;

        public TMP_Text HPText;

        void IHittable.OnHit (ProjectileState state)
        {
            HandleHealth(state);
        }

        private void Start () {
            if(HPBar)
                HPBar.localScale = new Vector3(entityState.Health / entityState.MaxHealth, 0.7f, 1f);

            if(HPText)
                HPText.text = entityState.Health + "/" + entityState.MaxHealth;
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

            if(HPBar)
                HPBar.localScale = new Vector3(entityState.Health / entityState.MaxHealth, 0.7f, 1f);

            if(HPText)
                HPText.text = entityState.Health + "/" + entityState.MaxHealth;

            // TODO: Add some kind of animation
            if (entityState.Health <= 0)
                entityState.gameObject.SetActive(false);
        }
    }
}