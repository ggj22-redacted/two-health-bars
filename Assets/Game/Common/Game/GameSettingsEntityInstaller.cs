using UnityEngine;
using Zenject;

namespace Game.Common.GameSettings
{
    public class GameSettingsEntityInstaller : MonoInstaller<GameSettingsEntityInstaller>
    {
        [SerializeField]
        private GameSettingsEntity gameSettingsEntity;

        public override void InstallBindings()
        {
            Container.Bind<GameSettingsEntity>().FromInstance(gameSettingsEntity).AsSingle();
        }
    }
}