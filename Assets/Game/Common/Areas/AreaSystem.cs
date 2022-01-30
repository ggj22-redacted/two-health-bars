using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Common.Areas
{
    public class AreaSystem : MonoBehaviour
    {
        public event Action OnPlayerEntered;

        public event Action OnPlayerLeft;

        public event Action<Stat, float> OnStatUpdated;

        [Inject]
        private EntityState _playerState;

        private int _playerAreaPresence;

        private readonly List<Area> _areas = new List<Area>();

        public bool IsPlayerInArea => _playerAreaPresence > 0;

        private void OnEnable ()
        {
            _playerState.OnRespawned += OnPlayerRespawn;

            foreach (var area in _areas)
                area.OnStatUpdated += OnStatUpdated;
        }

        private void OnDisable ()
        {
            _playerState.OnRespawned -= OnPlayerRespawn;

            foreach (var area in _areas)
                area.OnStatUpdated -= OnStatUpdated;
        }

        public void ResetAllAreas ()
        {
            foreach (var area in _areas) {
                if (!area.CanBeDestroyed)
                    continue;

                _areas.Remove(area);
                Destroy(area.gameObject);
            }
        }

        public void AddArea(Area area)
        {
            if (area == null || _areas.Contains(area))
                return;
            _areas.Add(area);

            area.OnStatUpdated += NotifyStatUpdated;
            area.OnPlayerEntered += OnPlayerAreaEnter;
            area.OnPlayerLeft += OnPlayerAreaExit;
        }

        public void RemoveArea(Area area)
        {
            if (area == null || !_areas.Contains(area))
                return;
            _areas.Remove(area);

            area.OnStatUpdated -= NotifyStatUpdated;
            area.OnPlayerEntered -= OnPlayerAreaEnter;
            area.OnPlayerLeft -= OnPlayerAreaExit;
        }

        private void OnPlayerRespawn (EntityState state) => ResetAllAreas();

        private void OnPlayerAreaEnter () => _playerAreaPresence++;

        private void OnPlayerAreaExit () => _playerAreaPresence--;

        private void NotifyStatUpdated(Stat stat, float value) => OnStatUpdated?.Invoke(stat, value);
    }
}