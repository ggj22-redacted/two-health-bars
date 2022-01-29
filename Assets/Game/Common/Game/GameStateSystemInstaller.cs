using UnityEngine;
using Zenject;

namespace Game.Common.Game
{
    public class GameStateSystemInstaller : MonoInstaller<GameStateSystemInstaller>
    {
        [SerializeField]
        private GameStateSystem gameStateSystem;

        public override void InstallBindings()
        {
            Container.BindInstance(gameStateSystem).AsSingle();
        }
    }
}