using System;
using Cysharp.Threading.Tasks;
using Game.Common.Game;
using UnityEngine;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityRespawner : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        [SerializeField]
        private Transform respawnPosition;

        [SerializeField]
        private float respawnDelay;

        [SerializeField]
        private float restartDelay;

        [Inject]
        private GameStateSystem _gameStateSystem;

        private void OnEnable()
        {
            entityState.OnDied += RestartOnDeath;
            _gameStateSystem.OnTimerEnd += RestartOnTimerEnd;
        }

        private void OnDisable()
        {
            entityState.OnDied -= RestartOnDeath;
            _gameStateSystem.OnTimerEnd -= RestartOnTimerEnd;
        }

        private void RestartOnTimerEnd ()
        {
            entityState.Health -= float.MaxValue;
        }

        private async void RestartOnDeath (EntityState state)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(respawnDelay));

            if (!entityState || !_gameStateSystem)
                return;

            _gameStateSystem.StopRound();

            entityState.transform.position = respawnPosition.position;
            entityState.gameObject.SetActive(true);
            entityState.ResetStats();

            await UniTask.Delay(TimeSpan.FromSeconds(restartDelay));

            if (_gameStateSystem)
                _gameStateSystem.RestartRound();
        }
    }
}