using System;
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

        [Inject]
        private GameStage[] _gameStages;

        private int _round = -1;

        public bool IsLastRound => _round == gameRounds.Length - 1;

        public bool IsInRound => _round >= 0;

        public GameRound CurrentRound => _round < 0 ? new GameRound() : gameRounds[_round].GameRound;

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
    }
}