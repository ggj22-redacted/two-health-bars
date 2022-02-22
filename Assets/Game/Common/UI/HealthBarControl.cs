using System;
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

        private void Awake () => InitializeEntityState();

        private void Start () {
            UpdateHealth(0f, _entityState.Health);
        }

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
                healthLabel.text = $"{_entityState.Health}|{_entityState.MaxHealth}";
        }

        private void InitializeEntityState ()
        {
            if (!_entityState)
                _entityState = GetComponentInParent<EntityState>();
        }
    }
}