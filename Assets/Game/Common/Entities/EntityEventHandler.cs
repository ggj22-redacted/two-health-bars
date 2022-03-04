using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Game.Common.UI;

namespace Game.Common.Entities
{
    public class EntityEventHandler : MonoBehaviour
    {
        [Inject]
        private EntityState _entityState;

        [Inject]
        private UISystemEntity _uiSystemEntity;

        [SerializeField]
        private UnityEntityEvent onDeath;

        [SerializeField]
        private UnityEntityEvent onRespawn;

        [SerializeField]
        private UnityStatEvent onHealthChanged;

        [SerializeField]
        private UnityUIEvent onMenu;

        [SerializeField]
        private UnityUIEvent offMenu;

        private void Awake () => InitializeEntityState();

        private void OnEnable ()
        {
            _entityState.OnHealthChanged += onHealthChanged.Invoke;
            _entityState.OnDied += onDeath.Invoke;
            _entityState.OnRespawned += onRespawn.Invoke;
            _uiSystemEntity.OnMenu += onMenu.Invoke;
            _uiSystemEntity.OffMenu += offMenu.Invoke;
        }

        private void OnDisable ()
        {
            _entityState.OnHealthChanged -= onHealthChanged.Invoke;
            _entityState.OnDied -= onDeath.Invoke;
            _entityState.OnRespawned -= onRespawn.Invoke;
            _uiSystemEntity.OnMenu -= onMenu.Invoke;
            _uiSystemEntity.OffMenu -= offMenu.Invoke;
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

    [Serializable]
    public class UnityUIEvent : UnityEvent<UISystemEntity> { }
}