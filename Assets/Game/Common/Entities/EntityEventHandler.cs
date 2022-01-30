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
        private UnityEntityEvent onDeath;

        [SerializeField]
        private UnityEntityEvent onRespawn;

        [SerializeField]
        private UnityStatEvent onHealthChanged;

        private void Awake () => InitializeEntityState();

        private void OnEnable ()
        {
            _entityState.OnHealthChanged += onHealthChanged.Invoke;
            _entityState.OnDied += onDeath.Invoke;
            _entityState.OnRespawned += onRespawn.Invoke;
        }

        private void OnDisable ()
        {
            _entityState.OnHealthChanged -= onHealthChanged.Invoke;
            _entityState.OnDied -= onDeath.Invoke;
            _entityState.OnRespawned -= onRespawn.Invoke;
        }

        private void InitializeEntityState ()
        {
            if (!_entityState)
                _entityState = GetComponentInParent<EntityState>();
        }
    }

    [Serializable]
    public class UnityStatEvent : UnityEvent<float, float> { }

    [Serializable]
    public class UnityEntityEvent : UnityEvent<EntityState> { }
}