using UnityEngine;
using Zenject;

namespace Game.Common.Areas
{
    public class AreaSystemInstaller : MonoInstaller<AreaSystemInstaller>
    {
        [SerializeField]
        private AreaSystem areaSystem;

        public override void InstallBindings()
        {
            Container.BindInstance(areaSystem).AsSingle();
        }
    }
}