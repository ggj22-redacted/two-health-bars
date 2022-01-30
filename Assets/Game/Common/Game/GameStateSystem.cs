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

        public event Action OnTimerEnd;

        [SerializeField]
        private GameRoundContainer[] gameRounds;

        private readonly List<GameStage> _gameStages = new List<GameStage>();

        private int _round = -1;

        private bool _isCountingTime;

        private float _roundEndMoment = float.MaxValue;

        [Inject]
        public EntityState PlayerState { get; private set; }

        public float CurrentTime
        {
            get
            {
                if (_isCountingTime)
                    return Mathf.Max(_roundEndMoment - Time.time, 0);

                if (Time.time >= _roundEndMoment)
                    return 0;

                return CurrentRound.duration;
            }
        }

        public bool IsLastRound => _round == gameRounds.Length - 1;

        public bool IsInRound => _isCountingTime;

        public int CurrentRoundNumber => Mathf.Clamp(_round, 0, gameRounds.Length - 1) + 1;

        public GameRound CurrentRound => gameRounds[Mathf.Clamp(_round, 0, gameRounds.Length - 1)].GameRound;

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

        private void Update ()
        {
            if (!_isCountingTime || Time.time <= _roundEndMoment)
                return;

            _isCountingTime = false;
            OnTimerEnd?.Invoke();
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

            GameRound gameRound = CurrentRound;
            OnGameRoundChanged?.Invoke(CurrentRound);
            OnGameStageChanged?.Invoke(CurrentStage);

            _roundEndMoment = Time.time + gameRound.duration;
            _isCountingTime = true;
        }

        public void RestartRound ()
        {
            if (_round < 0)
                return;

            GameRound gameRound = CurrentRound;
            OnGameRoundChanged?.Invoke(gameRound);
            OnGameStageChanged?.Invoke(CurrentStage);

            _roundEndMoment = Time.time + gameRound.duration;
            _isCountingTime = true;
        }

        public void StopRound ()
        {
            _roundEndMoment = float.MaxValue;
            _isCountingTime = false;
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