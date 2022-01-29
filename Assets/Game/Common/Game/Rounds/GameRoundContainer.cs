using UnityEngine;

namespace Game.Common.Game
{
    [CreateAssetMenu(fileName = "GameRound", menuName = "Game/Rounds/Round", order = 0)]
    public class GameRoundContainer : ScriptableObject
    {
        [SerializeField]
        private GameRound gameRound;

        public GameRound GameRound => gameRound;
    }
}