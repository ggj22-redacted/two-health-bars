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

        

        private void Update ()
        {
            if (_playerState.IsDead || !_gameStateSystem.IsInRound)
            {
                _gameStateSystem.CurrentStage.MusicController(true);
                return;
            }
            if (_areaSystem.IsPlayerInArea) {
                _areaSystem.CurrentArea.HandleStatMutation(_playerState);
                _areaSystem.CurrentArea.HandleStatUpdate(_playerState);

                _gameStateSystem.CurrentStage.Area.UpdateMoments();
                _gameStateSystem.CurrentStage.MusicController(false);
                return;
            }

            _gameStateSystem.CurrentStage.Area.HandleStatMutation(_playerState);
            _gameStateSystem.CurrentStage.Area.HandleStatUpdate(_playerState);
            _gameStateSystem.CurrentStage.MusicController(true);
        }
    }
}