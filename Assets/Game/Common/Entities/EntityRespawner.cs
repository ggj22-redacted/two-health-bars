using System;
using Cysharp.Threading.Tasks;
using Game.Common.Game;
using UnityEngine;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityRespawner : MonoBehaviour
    {

        public event Action<EntityRespawner> OnRespawn;

        [SerializeField]
        private EntityState entityState;

        [SerializeField]
        private Transform respawnPosition;

        [SerializeField]
        private float restartDelay;

        [Inject]
        private GameStateSystem _gameStateSystem;

        private void OnEnable()
        {
            _gameStateSystem.OnTimerEnd += RestartOnTimerEnd;
        }

        private void OnDisable()
        {
            _gameStateSystem.OnTimerEnd -= RestartOnTimerEnd;
        }

        private void RestartOnTimerEnd ()
        {
            entityState.Health -= float.MaxValue;
        }

        public async void RestartStage ()
        {
            if (!entityState || !_gameStateSystem)
                return;

            _gameStateSystem.StopRound();

            Respawn(entityState);

            OnRespawn?.Invoke(this);

            await UniTask.Delay(TimeSpan.FromSeconds(restartDelay));

            if (_gameStateSystem)
                _gameStateSystem.RestartRound();
        }

        private void Respawn(EntityState entity)
        {
            entity.transform.position = respawnPosition.position;
            entity.gameObject.SetActive(true);
            entity.ResetStats();
        }
    }
}