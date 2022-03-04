using UnityEngine;

namespace Game.Common.Game
{
    public class GameRoundStarter : MonoBehaviour
    {
        [SerializeField]
        private GameStateSystem gameStateSystem;

        [SerializeField]
        private float roundStartDelay = 4.3f;

        private float _countdownEndMoment;

        public float TimeToStart => _countdownEndMoment - Time.time;

        private void Start ()
        {
            _countdownEndMoment = Time.time + roundStartDelay;
        }

        private void Update ()
        {
            if (Time.time < _countdownEndMoment)
                return;

            gameStateSystem.TriggerNextRound();
            enabled = false;
        }
    }
}