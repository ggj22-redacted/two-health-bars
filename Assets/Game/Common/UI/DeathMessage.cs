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
        private Text textLabel;

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

        private CanvasGroup _screenCanvasGroup;

        private bool _activeScreen;

        private float _timeElapsed;

        private static string ChooseMessage (string sceneName)
        {
            string message = "Death Comes\n";

            switch (sceneName) {
                case "ChaosScene":
                    message += "!Order Reigns Supreme!";
                    break;
                case "OrderScene":
                    message += "!Chaos Consumes All!";
                    break;
            }

            return message;
        }

        private void Awake ()
        {
            _screenCanvasGroup = gameObject.GetComponent<CanvasGroup>();
            textLabel = gameObject.GetComponentInChildren<Text>();
            buttonsCanvasGroups = gameObject.GetComponentsInChildren<CanvasGroup>();

            _activeScreen = false;
            _timeElapsed = 0;
        }

        private void OnEnable ()
        {
            _playerState.OnDied += ActivateScreen;
            _playerState.OnRespawned += DeactivateScreen;
            restartButton.onClick.AddListener(RestartStage);
        }

        private void OnDisable ()
        {
            _playerState.OnDied -= ActivateScreen;
            _playerState.OnRespawned -= DeactivateScreen;
            restartButton.onClick.RemoveListener(RestartStage);
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
            textLabel.text = ChooseMessage(SceneManager.GetActiveScene().name);
            StartCoroutine(ShowButtons(timeToShow));
        }

        private void DeactivateScreen (EntityState state)
        {
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