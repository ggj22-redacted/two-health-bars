using UnityEngine;

namespace Game.Common.Enemies.Types
{
    [CreateAssetMenu(fileName = "EnemyType", menuName = "Game/Enemies/Types", order = 0)]
    public class EnemyType : ScriptableObject
    {
        [SerializeField]
        private EntityState entityState;
    }
}