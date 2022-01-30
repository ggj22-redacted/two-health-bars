using System.Collections.Generic;
using Game.Common.Game;
using UnityEngine;
using Zenject;

namespace Game.Common.Enemies
{
    public class EnemySystem : MonoBehaviour
    {
        [Inject]
        private GameStateSystem _gameStateSystem;

        [Inject]
        private EntityState _playerState;

        private readonly List<EntityState> _enemies = new List<EntityState>();

        public int EnemiesAliveCount
        {
            get
            {
                int alive = 0;
                foreach (var e in _enemies)
                    if (e.Health > 0)
                        alive++;

                return alive;
            }
        }

        private void OnEnable ()
        {
            _gameStateSystem.OnGameRoundChanged += SpawnEnemies;
            _playerState.OnRespawned += DestroyEnemies;
        }

        private void OnDisable ()
        {
            _gameStateSystem.OnGameRoundChanged -= SpawnEnemies;
            _playerState.OnRespawned -= DestroyEnemies;

            foreach (var enemy in _enemies)
                if (enemy)
                    Destroy(enemy.gameObject);
            _enemies.Clear();
        }

        private void SpawnEnemies (GameRound gameRound)
        {
            _enemies.Clear();
            if (_enemies.Capacity < gameRound.enemyCount)
                _enemies.Capacity = gameRound.enemyCount;

            var parent = _gameStateSystem.CurrentStage.transform;
            for (int i = 0; i < gameRound.enemyCount; i++) {
                EntityState enemy =
                    Instantiate(gameRound.enemyType.EntityState, _gameStateSystem.CurrentStage.SpawnLocation, parent.rotation, parent);
                _enemies.Add(enemy);
            }
        }

        private void DestroyEnemies(EntityState state)
        {
            foreach (var enemy in _enemies)
                if (enemy)
                    Destroy(enemy.gameObject);
            _enemies.Clear();
        }
    }
}