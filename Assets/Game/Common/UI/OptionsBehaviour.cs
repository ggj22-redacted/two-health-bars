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
        public Slider masterAudio;
        public Slider musicAudio;
        public Slider effectAudio;
        public CanvasGroup thisGroup;

        // Start is called before the first frame update
        void Start()
        {
            SetScreenToggle(screenToggle.isOn);
            thisGroup = gameObject.GetComponent<CanvasGroup>();
        }

        void SetScreenToggle(bool value)
        {
            screenSize.gameObject.SetActive(value);
            _gameSettings.SetRes(value);
        }

        void SetVolumeMaster(float value)
        {
            if (thisGroup.interactable)
                _gameSettings.SetAudio(value, "master");
        }

        void SetVolumeMusic(float value)
        {
            if (thisGroup.interactable)
                _gameSettings.SetAudio(value, "music");
        }

        void SetVolumeEffect(float value)
        {
            if (thisGroup.interactable)
                _gameSettings.SetAudio(value, "effect");
        }

        public void SetSliders()
        {
            masterAudio.value = _gameSettings.masterAudio;
            musicAudio.value = _gameSettings.musicAudio;
            effectAudio.value = _gameSettings.effectAudio;
        }

        private void OnEnable()
        {
            screenToggle.onValueChanged.AddListener(SetScreenToggle);
            screenSize.onValueChanged.AddListener(_gameSettings.SetSize);
            masterAudio.onValueChanged.AddListener(SetVolumeMaster);
            musicAudio.onValueChanged.AddListener(SetVolumeMusic);
            effectAudio.onValueChanged.AddListener(SetVolumeEffect);
        }

        private void OnDisable()
        {
            screenToggle.onValueChanged.RemoveListener(SetScreenToggle);
            screenSize.onValueChanged.RemoveListener(_gameSettings.SetSize);
            masterAudio.onValueChanged.RemoveListener(SetVolumeMaster);
            musicAudio.onValueChanged.RemoveListener(SetVolumeMusic);
            effectAudio.onValueChanged.RemoveListener(SetVolumeEffect);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}