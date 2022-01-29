using Zenject;

namespace Game.Common.Game
{
    public class GameStageInstaller : MonoInstaller<GameStageInstaller>
    {
        public override void InstallBindings()
        {
            GameStage[] gameStages = GetComponentsInChildren<GameStage>();

            Container.Bind<GameStage[]>().FromInstance(gameStages).AsSingle();
        }
    }
}