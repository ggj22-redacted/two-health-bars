using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Common.Areas
{
    public class AreaSystem : MonoBehaviour
    {
        public event Action<Stat, float> OnStatUpdated;

        [Inject]
        private EntityState _playerState;

        private readonly List<Area> _areas = new List<Area>();

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
            foreach (var area in _areas)
                Destroy(area.gameObject);
            _areas.Clear();
        }

        public void AddArea(Area area)
        {
            if (area == null || _areas.Contains(area))
                return;
            _areas.Add(area);

            area.OnStatUpdated += OnStatUpdated;
        }

        public void RemoveArea(Area area)
        {
            if (area == null || !_areas.Contains(area))
                return;
            _areas.Remove(area);

            area.OnStatUpdated -= OnStatUpdated;
        }

        private void OnPlayerRespawn (EntityState state) => ResetAllAreas();

        private void NotifyStatUpdated(Stat stat, float value) => OnStatUpdated?.Invoke(stat, value);
    }
}