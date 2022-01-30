using System;
using System.Collections.Generic;
using Game.Common.Enemies.Types;
using UnityEngine;
using Zenject;

namespace Game.Common.Game
{
    public class GameStateSystem : MonoBehaviour
    {
        public event Action OnReset;

        public event Action<GameRound> OnGameRoundChanged;
        public event Action<GameStage> OnGameStageChanged;

        [SerializeField]
        private GameRoundContainer[] gameRounds;

        private readonly List<GameStage> _gameStages = new List<GameStage>();

        private int _round = -1;

        [Inject]
        public EntityState PlayerState { get; private set; }

        public bool IsLastRound => _round == gameRounds.Length - 1;

        public bool IsInRound => _round >= 0;

        public GameRound CurrentRound => _round < 0 ? new GameRound() : gameRounds[_round].GameRound;

        public GameRound NextRound
        {
            get
            {
                int nextRound = _round + 1;
                return nextRound >= gameRounds.Length
                    ? gameRounds[gameRounds.Length - 1].GameRound
                    : gameRounds[nextRound].GameRound;
            }
        }

        public GameStage CurrentStage
        {
            get
            {
                if (!IsInRound)
                    return null;

                EnemyType enemyType = CurrentRound.enemyType;
                foreach (GameStage gameStage in _gameStages)
                    if (gameStage.EnemyType == enemyType)
                        return gameStage;

                return null;
            }
        }

        public GameStage NextStage
        {
            get
            {
                EnemyType enemyType = NextRound.enemyType;
                foreach (GameStage gameStage in _gameStages)
                    if (gameStage.EnemyType == enemyType)
                        return gameStage;

                return null;
            }
        }

        public void Reset ()
        {
            _round = -1;

            OnReset?.Invoke();
        }

        public void TriggerNextRound ()
        {
            if (++_round >= gameRounds.Length)
                _round = gameRounds.Length - 1;

            OnGameRoundChanged?.Invoke(CurrentRound);
            OnGameStageChanged?.Invoke(CurrentStage);
        }

        public void RestartRound ()
        {
            if (_round < 0)
                return;

            OnGameRoundChanged?.Invoke(CurrentRound);
            OnGameStageChanged?.Invoke(CurrentStage);
        }

        public void RegisterStage (GameStage gameStage)
        {
            if (!_gameStages.Contains(gameStage))
                _gameStages.Add(gameStage);
        }

        public void UnregisterStage (GameStage gameStage)
        {
            if (_gameStages.Contains(gameStage))
                _gameStages.Remove(gameStage);
        }
    }
}