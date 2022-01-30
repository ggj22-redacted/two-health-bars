using UnityEngine;
using Zenject;

namespace Game.Common.Enemies
{
    public class EnemySystemInstaller : MonoInstaller<EnemySystemInstaller>
    {
        [SerializeField]
        private EnemySystem enemySystem;

        public override void InstallBindings()
        {
            Container.BindInstance(enemySystem).AsSingle();
        }
    }
}