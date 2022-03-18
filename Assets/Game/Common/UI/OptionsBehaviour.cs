using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Game.Common.GameSettings;

namespace Game.Common.UI
{
    public class OptionsBehaviour : MonoBehaviour
    {
        [Inject]
        private GameSettingsEntity _gameSettings;
        public Toggle screenToggle;
        public Slider screenSize;

        // Start is called before the first frame update
        void Start()
        {
            SetScreenToggle(screenToggle.isOn);
        }

        void SetScreenToggle(bool value)
        {
            screenSize.gameObject.SetActive(value);
            _gameSettings.SetRes(value);
        }

        private void OnEnable()
        {
            screenToggle.onValueChanged.AddListener(SetScreenToggle);
            screenSize.onValueChanged.AddListener(_gameSettings.SetSize);
        }

        private void OnDisable()
        {
            screenToggle.onValueChanged.RemoveListener(SetScreenToggle);
            screenSize.onValueChanged.RemoveListener(_gameSettings.SetSize);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}