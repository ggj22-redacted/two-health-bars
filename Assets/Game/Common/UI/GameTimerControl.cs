using Game.Common.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class GameTimerControl : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text timerLabel;

        [Inject]
        private GameStateSystem _gameStateSystem;

        private void Update ()
        {
            float time = _gameStateSystem.CurrentTime + 0.999f;
            int seconds = Mathf.FloorToInt(time % 60);
            int minutes = Mathf.FloorToInt(time / 60);
            timerLabel.text = $"{minutes:00}:{seconds:00}";
        }
    }
}