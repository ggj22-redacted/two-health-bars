using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Common.Shooting
{
    public class ShootingControl : MonoBehaviour
    {
        [SerializeField]
        private EntityState entityState;

        [SerializeField]
        private UnityEvent onShoot;

        private bool _isShooting = false;

        [Inject]
        private ProjectileSystem _projectileSystem;

        public bool IsShootingAllowed { get; set; } = true;

        private void Awake () => InitializeProjectileSystem();

        public void Update()
        {
            if (!_isShooting || !IsShootingAllowed)
                return;

            bool shot = _projectileSystem.OnShoot(entityState);

            if (shot)
                onShoot.Invoke();
        }

        public void OnShoot()
        {
            _isShooting = !_isShooting;
        }

        public void SetShooting(bool value)
        {
            _isShooting = value;
        }

        private void InitializeProjectileSystem ()
        {
            if (!_projectileSystem)
                _projectileSystem = FindObjectOfType<ProjectileSystem>();
        }
    }
}