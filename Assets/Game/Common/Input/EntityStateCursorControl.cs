using StarterAssets;
using UnityEngine;
using Zenject;
using Game.Common.UI;

namespace Game.Common.Input
{
    public class EntityStateCursorControl : MonoBehaviour
    {
        [SerializeField]
        private StarterAssetsInputs starterAssetsInputs;

        [Inject]
        private EntityState _entityState;

        [Inject]
        private UISystemEntity _uiSystemEntity;

        private void OnEnable ()
        {
            _uiSystemEntity.OnMenu += DisableMouseLock;
            _uiSystemEntity.OffMenu += EnableMouseLock;
        }

        private void OnDisable ()
        {
            _uiSystemEntity.OnMenu -= DisableMouseLock;
            _uiSystemEntity.OffMenu -= EnableMouseLock;
        }

        private void EnableMouseLock(UISystemEntity _uiSystemEntity)
        {
            starterAssetsInputs.CursorLocked = !_entityState.IsDead;
        }

        private void DisableMouseLock(UISystemEntity _uiSystemEntity)
        {
            starterAssetsInputs.CursorLocked = false;
        }
    }
}