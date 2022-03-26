using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Game.Common.GameSettings;


namespace Game.Common.UI
{
    public class PauseCanvasControl : MonoBehaviour
    {

        private CanvasGroup groupCanvas;
        private bool activeScreen;
        public GameObject optionCanvas;
        private CanvasGroup optionCanvasGroup;
        public Button optionButton;
        public Button optionBackButton;
        public Button titleButton;
        public GameObject leaveWindow;

        private GameObject createdWindow;

        [Inject]
        private UISystemEntity _uiSystemEntity;

        [Inject]
        private GameSettingsEntity _gameSettings;

        void ShowCanvas(CanvasGroup showGroup, bool value, int alpha)
        {
            showGroup.interactable = value;
            showGroup.alpha = alpha;
            showGroup.blocksRaycasts = value;
        }

        // Start is called before the first frame update
        void Start()
        {
            groupCanvas = gameObject.GetComponent<CanvasGroup>();
            optionCanvasGroup = optionCanvas.GetComponent<CanvasGroup>();
            activeScreen = false;
        }

        void CallOptions()
        {
            optionCanvas.GetComponent<OptionsBehaviour>().SetSliders();
            ShowCanvas(optionCanvasGroup, true, 1);
        }

        void CallWindow()
        {
            createdWindow = Instantiate(leaveWindow, gameObject.transform.parent);
            createdWindow.GetComponent<WindowBehaviour>().recoverCanvas = groupCanvas;
            groupCanvas.interactable = false;
        }

        void CloseOptions()
        {
            _gameSettings.SettingsSave();
            ShowCanvas(optionCanvasGroup, false, 0);
        }

        private void OnEnable()
        {
            optionButton.onClick.AddListener(CallOptions);
            optionBackButton.onClick.AddListener(CloseOptions);
            titleButton.onClick.AddListener(CallWindow);
        }

        private void OnDisable()
        {
            optionButton.onClick.RemoveListener(CallOptions);
            optionBackButton.onClick.RemoveListener(CloseOptions);
            titleButton.onClick.RemoveListener(CallWindow);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0 && activeScreen == false)
            {
                activeScreen = true;
                ShowCanvas(groupCanvas, true, 1);
                _uiSystemEntity.ActivateMenu();
            }
            if (Time.timeScale == 1 && activeScreen == true)
            {
                activeScreen = false;
                ShowCanvas(groupCanvas, false, 0);
                _uiSystemEntity.DeactivateMenu();
            }
        }
    }
}