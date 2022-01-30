using Game.Common.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class RoundCountControl : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text roundCountLabel;

        [Inject]
        private GameStateSystem _gameStateSystem;

        private void Update ()
        {
            roundCountLabel.text = _gameStateSystem.CurrentRoundNumber.ToString("D2");
        }
    }
}