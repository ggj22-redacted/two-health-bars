using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Common.Entities;
using UnityEngine.UI;
using Game.Common.GameSettings;

namespace Game.Common.UI
{
    public class TittleScreenBehaviour : MonoBehaviour
    {

        [Inject]
        private UISystemEntity _uiSystemEntity;

        public CanvasGroup[] CanvasGroup;
        private Animator thisAnim;

        public Button[] MenuButtons;
        public Button[] CloseButton;
        private bool _activeScreen;

        [Inject]
        private GameSettingsEntity _gameSettings;

        private void Awake()
        {
            thisAnim = GetComponent<Animator>();
            _activeScreen = true;
        }

        void ExitGame()
        {
            Application.Quit();
        }

        private void ActivateTittleScreen(UISystemEntity state)
        {
            ActivateScreen(CanvasGroup[0]);
        }

        private void DeactivateScreen(CanvasGroup screen)
        {
            screen.alpha = 0;
            screen.interactable = false;
            screen.blocksRaycasts = false;
            _uiSystemEntity.DeactivateMenu();
        }

        void CallLeaderboardScreen()
        {
            if (CanvasGroup[1].alpha < 1)
            {
                ActivateScreen(CanvasGroup[1]);
            }
            else
            {
                DeactivateScreen(CanvasGroup[1]);
            }
        }

        void CallOptionScreen()
        {
            if (CanvasGroup[2].alpha < 1)
            {
                CanvasGroup[2].gameObject.GetComponent<OptionsBehaviour>().SetSliders();
                ActivateScreen(CanvasGroup[2]);
            }
            else
            {
                _gameSettings.SettingsSave();
                DeactivateScreen(CanvasGroup[2]);
            }
        }

        void CallCreditsScreen()
        {
            if (CanvasGroup[3].alpha < 1)
            {
                ActivateScreen(CanvasGroup[3]);
            }
            else
            {
                DeactivateScreen(CanvasGroup[3]);
            }
        }

        void ActivateScreen(CanvasGroup screen)
        {
            screen.alpha = 1;
            screen.interactable = true;
            screen.blocksRaycasts = true;
            _uiSystemEntity.ActivateMenu();
        }

        private void OnEnable()
        {
            _uiSystemEntity.OnStart += ActivateTittleScreen;
            //startButton.onClick.AddListener();
            MenuButtons[1].onClick.AddListener(CallLeaderboardScreen);
            MenuButtons[2].onClick.AddListener(CallOptionScreen);
            MenuButtons[3].onClick.AddListener(CallCreditsScreen);
            CloseButton[0].onClick.AddListener(CallLeaderboardScreen);
            CloseButton[1].onClick.AddListener(CallOptionScreen);
            CloseButton[2].onClick.AddListener(CallCreditsScreen);
            MenuButtons[4].onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            _uiSystemEntity.OnStart -= ActivateTittleScreen;
            //startButton.onClick.RemoveListener()
            MenuButtons[1].onClick.RemoveListener(CallLeaderboardScreen);
            MenuButtons[2].onClick.RemoveListener(CallOptionScreen);
            MenuButtons[3].onClick.RemoveListener(CallCreditsScreen);
            CloseButton[0].onClick.RemoveListener(CallLeaderboardScreen);
            CloseButton[1].onClick.RemoveListener(CallOptionScreen);
            CloseButton[2].onClick.RemoveListener(CallCreditsScreen);
            MenuButtons[4].onClick.RemoveListener(ExitGame);
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}