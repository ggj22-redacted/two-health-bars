using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;


namespace Game.Common.UI
{
    public class PauseCanvasControl : MonoBehaviour
    {

        private CanvasGroup groupCanvas;
        private bool activeScreen;

        [Inject]
        private UISystemEntity _uiSystemEntity;

        // Start is called before the first frame update
        void Start()
        {
            groupCanvas = gameObject.GetComponent<CanvasGroup>();
            activeScreen = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0 && activeScreen == false)
            {
                activeScreen = true;
                groupCanvas.interactable = true;
                groupCanvas.alpha = 1;
                groupCanvas.blocksRaycasts = true;
                _uiSystemEntity.ActivateMenu();
            }
            if (Time.timeScale == 1 && activeScreen == true)
            {
                activeScreen = false;
                groupCanvas.interactable = false;
                groupCanvas.alpha = 0;
                groupCanvas.blocksRaycasts = false;
                _uiSystemEntity.DeactivateMenu();
            }
        }
    }
}