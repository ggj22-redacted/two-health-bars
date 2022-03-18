using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Game.Common.UI
{
    public class PauseCanvasControl : MonoBehaviour
    {

        private CanvasGroup groupCanvas;
        private bool activeScreen;
        public CanvasGroup optionCanvas;
        public Button optionButton;
        public Button optionBackButton;

        [Inject]
        private UISystemEntity _uiSystemEntity;


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
            activeScreen = false;
        }

        void CallOptions()
        {
            ShowCanvas(optionCanvas, true, 1);
        }

        void CloseOptions()
        {
            ShowCanvas(optionCanvas, false, 0);
        }

        private void OnEnable()
        {
            optionButton.onClick.AddListener(CallOptions);
            optionBackButton.onClick.AddListener(CloseOptions);
        }

        private void OnDisable()
        {
            optionButton.onClick.RemoveListener(CallOptions);
            optionBackButton.onClick.RemoveListener(CloseOptions);
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