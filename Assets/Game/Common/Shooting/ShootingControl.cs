using UnityEngine;
using Zenject;

namespace Game.Common.Shooting
{
    public class ShootingControl : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        private bool shooting = false;

        [Inject]
        private ProjectileSystem _projectileSystem;

        public void OnShoot()
        {
            shooting = !shooting;
        }

        public void Update() {
            if (shooting)
                _projectileSystem.OnShoot(entityState);
        }
    }
}