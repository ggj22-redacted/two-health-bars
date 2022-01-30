using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Common.Entities
{
    public class EntityEventHandler : MonoBehaviour
    {
        [Inject]
        private EntityState _entityState;

        [SerializeField]
        private UnityEntityEvent onHealthChanged;

        private void OnEnable ()
        {
            _entityState.OnHealthChanged += onHealthChanged.Invoke;
        }

        private void OnDisable ()
        {
            _entityState.OnHealthChanged -= onHealthChanged.Invoke;
        }
    }

    [Serializable]
    public class UnityEntityEvent : UnityEvent<float, float> { }
}