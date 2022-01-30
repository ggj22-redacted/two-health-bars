using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class ShieldBarControl : MonoBehaviour
    {
        [SerializeField]
        public Transform shieldBar;

        [SerializeField]
        public TMP_Text shieldLabel;

        [Inject]
        private EntityState _entityState;

        private void Start () {
            UpdateShield(0f, _entityState.Shield);
        }

        private void OnEnable ()
        {
            _entityState.OnShieldChanged += UpdateShield;
        }

        private void OnDisable ()
        {
            _entityState.OnShieldChanged -= UpdateShield;
        }

        private void UpdateShield (float previous, float current)
        {
            if (shieldBar) {
                Vector3 scale = shieldBar.localScale;
                scale.x = current / _entityState.MaxShield;
                shieldBar.localScale = scale;
            }

            if(shieldLabel)
                shieldLabel.text = $"{_entityState.Shield}/{_entityState.MaxShield}";
        }
    }
}