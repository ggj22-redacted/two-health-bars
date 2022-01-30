using UnityEngine;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityGameObjectDisabler : MonoBehaviour
    {
        [Inject]
        private EntityState _entityState;

        public void DisableGameObject()
        {
            if (_entityState.Health <= 0)
                _entityState.gameObject.SetActive(false);
        }
    }
}