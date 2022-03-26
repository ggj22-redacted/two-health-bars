using System;
using Game.Common.Game;
using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class HealthBarDisplayControl : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve alphaOverDistance;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Transform referenceTransform;

        [Inject]
        private GameStateSystem _gameStateSystem;

        private void Awake ()
        {
            InitializeGameStateSystem();
        }

        private void Update ()
        {
            if (Time.timeScale == 0)
            {
                canvasGroup.alpha = 0;
            }
            if (Time.timeScale == 1)
            {
                var playerState = _gameStateSystem.PlayerState;
                var distance = Vector3.Distance(playerState.transform.position, referenceTransform.position);
                var alpha = alphaOverDistance.Evaluate(distance);
                canvasGroup.alpha = alpha;
            }
        }

        private void InitializeGameStateSystem ()
        {
            if (!_gameStateSystem)
                _gameStateSystem = FindObjectOfType<GameStateSystem>();
        }
    }
}