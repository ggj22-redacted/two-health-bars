using Game.Common.Areas;
using UnityEngine;
using Zenject;

namespace Game.Common.Game
{
    public class GameStageController : MonoBehaviour
    {
        [Inject]
        private EntityState _playerState;

        [Inject]
        private AreaSystem _areaSystem;

        [Inject]
        private GameStateSystem _gameStateSystem;

        private float _nextUpdateMoment;

        private void Update ()
        {
            if (_playerState.IsDead || _areaSystem.IsPlayerInArea || !_gameStateSystem.IsInRound || Time.time < _nextUpdateMoment)
                return;

            _gameStateSystem.CurrentStage.Area.HandleStatMutation(_playerState);
            _gameStateSystem.CurrentStage.Area.HandleStatUpdate(_playerState);

            _nextUpdateMoment = Time.time + 1f / _gameStateSystem.CurrentStage.ShieldUpdateRate;
        }
    }
}