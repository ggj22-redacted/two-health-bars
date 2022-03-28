using System.Collections;
using Game.Common.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Game.Common.UI
{
    public class DeathMessage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float timeToShow;

        [SerializeField]
        private Text[] textLabel;

        [SerializeField]
        private TMP_Text bigTextLabel;

        [SerializeField]
        private CanvasGroup statMessageCanvas;

        [Header("Buttons")]
        [SerializeField]
        private CanvasGroup[] buttonsCanvasGroups;

        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Button restartButton;

        [Inject]
        private EntityState _playerState;

        [Inject]
        private EntityRespawner _entityRespawner;

        [Inject]
        private UISystemEntity _uiSystemEntity;

        private CanvasGroup _screenCanvasGroup;

        private GameObject createdWindow;

        public GameObject leaveWindow;
        public CanvasGroup[] hideCanvas;

        private bool _activeScreen;

        private float _timeElapsed;

        private static string ChooseMessage (string sceneName)
        {
            string message = "";

            switch (sceneName) {
                case "ChaosScene":
                    message = "- Order Will Reign Supreme -";
                    break;
                case "OrderScene":
                    message = "- Chaos Will Consume All -";
                    break;
            }

            return message;
        }

        private void Awake ()
        {
            _screenCanvasGroup = gameObject.GetComponent<CanvasGroup>();
            textLabel = gameObject.GetComponentsInChildren<Text>();
            buttonsCanvasGroups = gameObject.GetComponentsInChildren<CanvasGroup>();

            _activeScreen = false;
            _timeElapsed = 0;
        }

        void CallWindow()
        {
            createdWindow = Instantiate(leaveWindow, gameObject.transform.parent);
            createdWindow.GetComponent<WindowBehaviour>().recoverCanvas = _screenCanvasGroup;
            _screenCanvasGroup.interactable = false;
            _screenCanvasGroup.blocksRaycasts = false;
        }

        private void OnEnable ()
        {
            _playerState.OnDied += ActivateScreen;
            _playerState.OnRespawned += DeactivateScreen;
            restartButton.onClick.AddListener(RestartStage);
            backButton.onClick.AddListener(CallWindow);
        }

        private void OnDisable ()
        {
            _playerState.OnDied -= ActivateScreen;
            _playerState.OnRespawned -= DeactivateScreen;
            restartButton.onClick.RemoveListener(RestartStage);
            backButton.onClick.RemoveListener(CallWindow);
        }

        private void Update ()
        {
            if (_activeScreen)
                ShowCanvas();
        }

        private void ShowCanvas ()
        {
            _screenCanvasGroup.interactable = true;
            _screenCanvasGroup.blocksRaycasts = true;
            if (_screenCanvasGroup.alpha < 1) {
                _screenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, _timeElapsed / timeToShow);
                _timeElapsed += Time.deltaTime;
            } else {
                _activeScreen = false;
            }
        }

        private void ActivateScreen (EntityState state)
        {
            _activeScreen = true;
            for (int x = 0; x < hideCanvas.Length; x++)
            {
                hideCanvas[x].alpha = 0;
            }
            bigTextLabel.text = "MISSION FAILED";
            textLabel[0].text = ChooseMessage(SceneManager.GetActiveScene().name);
            textLabel[1].text = ChooseMessage(SceneManager.GetActiveScene().name);
            StartCoroutine(ShowButtons(timeToShow));
            _uiSystemEntity.ActivateMenu();
        }

        private void DeactivateScreen (EntityState state)
        {
            for (int x = 0; x < hideCanvas.Length; x++)
            {
                hideCanvas[x].alpha = 1;
            }
            _screenCanvasGroup.interactable = false;
            _screenCanvasGroup.blocksRaycasts = false;
            _screenCanvasGroup.alpha = 0f;
            statMessageCanvas.alpha = 1f;
            _timeElapsed = 0;
            foreach (CanvasGroup canvasGroup in buttonsCanvasGroups) {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            _uiSystemEntity.DeactivateMenu();
        }

        private IEnumerator ShowButtons (float time)
        {
            yield return new WaitForSeconds(time);
            foreach (CanvasGroup canvasGroup in buttonsCanvasGroups) {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            statMessageCanvas.alpha = 1f;

        }

        private void RestartStage ()
        {
            _entityRespawner.RestartStage();
        }
    }
}