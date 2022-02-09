using UnityEngine;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityStateProvider : MonoBehaviour, IEntityStateProvider
    {
        [SerializeField]
        private EntityState entityState;

        public EntityState Get () => entityState;
    }
}