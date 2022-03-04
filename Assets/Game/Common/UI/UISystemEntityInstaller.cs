using UnityEngine;
using Zenject;

namespace Game.Common.UI
{
    public class UISystemEntityInstaller : MonoInstaller<UISystemEntityInstaller>
    {
        [SerializeField]
        private UISystemEntity uISystemEntity;

        public override void InstallBindings()
        {
            Container.Bind<UISystemEntity>().FromInstance(uISystemEntity).AsSingle();
        }
    }
}