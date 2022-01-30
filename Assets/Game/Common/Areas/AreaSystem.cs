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

        private List<Area> _areas = new List<Area>();

        private readonly List<Area> _areasPresent = new List<Area>();

        public bool IsPlayerInArea => _areasPresent.Count > 0;

        public Area CurrentArea => _areasPresent.Count > 0 ? _areasPresent[0] : null;

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
            List<Area> areas = new List<Area>(_areas.Count);
            for (int i = _areas.Count - 1; i >= 0; i--) {
                Area area = _areas[i];
                if (!area.CanBeDestroyed) {
                    areas.Add(area);
                    continue;
                }

                Destroy(area.gameObject);
            }

            _areas = areas;

            _areasPresent.Clear();
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

        private void OnPlayerAreaEnter (Area area)
        {
            if (!_areasPresent.Contains(area))
                _areasPresent.Add(area);
        }

        private void OnPlayerAreaExit (Area area)
        {
            if (_areasPresent.Contains(area))
                _areasPresent.Remove(area);
        }

        private void NotifyStatUpdated(Stat stat, float value) => OnStatUpdated?.Invoke(stat, value);
    }
}