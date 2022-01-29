using UnityEngine;
using Zenject;

namespace Game.Common.Cameras
{
    public class CameraInstaller : MonoInstaller<CameraInstaller>
    {
        [SerializeField]
        private Camera mainCamera;

        public override void InstallBindings()
        {
            Container.BindInstance(mainCamera).AsSingle();
        }
    }
}