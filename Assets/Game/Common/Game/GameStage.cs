using Game.Common.Enemies.Types;
using UnityEngine;

namespace Game.Common.Game
{
    public class GameStage : MonoBehaviour
    {
        [SerializeField]
        private EnemyType enemyType;

        public EnemyType EnemyType => enemyType;
    }
}