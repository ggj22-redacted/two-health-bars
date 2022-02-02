using StarterAssets;
using UnityEngine;
using Zenject;

namespace Game.Common.Input
{
    public class EntityStateCursorControl : MonoBehaviour
    {
        [SerializeField]
        private StarterAssetsInputs starterAssetsInputs;

        [Inject]
        private EntityState _entityState;

        private void OnEnable ()
        {
            _entityState.OnDied += DisableMouseLock;
            _entityState.OnRespawned += EnableMouseLock;
        }

        private void OnDisable ()
        {
            _entityState.OnDied -= DisableMouseLock;
            _entityState.OnRespawned -= EnableMouseLock;
        }

        private void EnableMouseLock(EntityState entityState)
        {
            starterAssetsInputs.CursorLocked = !_entityState.IsDead;
        }

        private void DisableMouseLock(EntityState entityState)
        {
            starterAssetsInputs.CursorLocked = false;
        }
    }
}