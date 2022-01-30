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
        }

        private void OnDisable()
        {
            entityState.OnDied -= RestartOnDeath;
        }

        private async void RestartOnDeath (EntityState state)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(respawnDelay));

            if (!entityState)
                return;

            entityState.transform.position = respawnPosition.position;
            entityState.gameObject.SetActive(true);
            entityState.ResetStats();

            await UniTask.Delay(TimeSpan.FromSeconds(restartDelay));

            if (_gameStateSystem)
                _gameStateSystem.RestartRound();
        }
    }
}