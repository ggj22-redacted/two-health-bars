using System;
using Game.Common.Enemies.Types;
using UnityEngine;

namespace Game.Common.Game
{
    [Serializable]
    public struct GameRound
    {
        [Min(1)]
        public int enemyCount;

        public EnemyType enemyType;
    }
}