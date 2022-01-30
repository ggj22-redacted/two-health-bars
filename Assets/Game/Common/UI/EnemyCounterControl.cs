using Game.Common.Enemies;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class EnemyCounterControl : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        [Inject]
        private EnemySystem _enemySystem;

        private void Update ()
        {
            label.text = _enemySystem.EnemiesAliveCount.ToString("D2"); // $"{_enemySystem.EnemiesAliveCount:D2}";
        }
    }
}