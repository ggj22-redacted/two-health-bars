using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Common.Entities;
using UnityEngine.UI;

namespace Game.Common.UI
{
    public class TittleScreenBehaviour : MonoBehaviour
    {

        [Inject]
        private EntityRespawner _entityRespawner;
        [Inject]
        private UISystemEntity _uiSystemEntity;
        [Inject]
        private EntityState _playerState;

        private CanvasGroup CanvasGroup;

        public Button startButton;
        private bool _activeScreen;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            _activeScreen = true;
        }

        void RoundStart()
        {
            _entityRespawner.RestartStage();
            DeactivateScreen();
        }

        private void ActivateScreen(UISystemEntity state)
        {
            _activeScreen = true;
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            _uiSystemEntity.ActivateMenu();
        }

        private void DeactivateScreen()
        {
            _activeScreen = false;
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            _uiSystemEntity.DeactivateMenu();
        }

        private void OnEnable()
        {
            _uiSystemEntity.OnStart += ActivateScreen;
            startButton.onClick.AddListener(RoundStart);
        }

        private void OnDisable()
        {
            _uiSystemEntity.OnStart -= ActivateScreen;
            startButton.onClick.RemoveListener(RoundStart);
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