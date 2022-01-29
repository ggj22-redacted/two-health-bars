using System;
using Game.Common.Enemies.Types;
using UnityEngine;
using Zenject;

namespace Game.Common.Game
{
    public class GameStateSystem : MonoBehaviour
    {
        public event Action<GameRound> OnGameRoundChanged;
        public event Action<GameStage> OnGameStateChanged;

        [SerializeField]
        private GameRoundContainer[] gameRounds;

        [Inject]
        private GameStage[] gameStages;

        private int _round = 0;

        public bool IsLastRound => _round == gameRounds.Length - 1;

        public GameRound CurrentRound => gameRounds[_round].GameRound;

        public GameStage CurrentStage
        {
            get
            {
                EnemyType enemyType = CurrentRound.enemyType;
                foreach (GameStage gameStage in gameStages)
                    if (gameStage.EnemyType == enemyType)
                        return gameStage;

                return null;
            }
        }

        public void TriggerNextRound ()
        {
            if (++_round >= gameRounds.Length)
                _round = gameRounds.Length - 1;

            OnGameRoundChanged?.Invoke(CurrentRound);
            OnGameStateChanged?.Invoke(CurrentStage);
        }
    }
}