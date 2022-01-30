using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class HealthBarControl : MonoBehaviour
    {
        [SerializeField]
        public Transform healthBar;

        [SerializeField]
        public TMP_Text healthLabel;

        [Inject]
        private EntityState _entityState;

        private void OnEnable ()
        {
            _entityState.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable ()
        {
            _entityState.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth (float previous, float current)
        {
            if (healthBar) {
                Vector3 scale = healthBar.localScale;
                scale.x = current / _entityState.MaxHealth;
                healthBar.localScale = scale;
            }

            if(healthLabel)
                healthLabel.text = $"{_entityState.Health}/{_entityState.MaxHealth}";
        }
    }
}