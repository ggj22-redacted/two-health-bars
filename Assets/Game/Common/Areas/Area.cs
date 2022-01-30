using System;
using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Areas
{
    public class Area : MonoBehaviour
    {
        public event Action<Stat, float> OnStatUpdated;

        [SerializeField]
        private float updateRate;

        [SerializeField]
        private float mutationRate;

        [Header("Movement Speed")]
        [SerializeField]
        private float speedMin;

        [SerializeField]
        private float speedMax;

        [SerializeField]
        private float speedMutationDelta;

        [Header("Jump Height")]
        [SerializeField]
        private float jumpHeightMin;

        [SerializeField]
        private float jumpHeightMax;

        [SerializeField]
        private float jumpHeightMutationDelta;

        [Header("Gravity")]
        [SerializeField]
        private float gravityMin;

        [SerializeField]
        private float gravityMax;

        [SerializeField]
        private float gravityMutationDelta;

        [Header("Projectiles")]
        [SerializeField]
        private float projectileSpeedMin;

        [SerializeField]
        private float projectileSpeedMax;

        [SerializeField]
        private float projectileSpeedMutationDelta;

        [SerializeField]
        private float projectileDamageMin;

        [SerializeField]
        private float projectileDamageMax;

        [SerializeField]
        private float projectileDamageMutationDelta;

        [SerializeField]
        private float projectileRangeMin;

        [SerializeField]
        private float projectileRangeMax;

        [SerializeField]
        private float projectileRangeMutationDelta;

        [SerializeField]
        private float projectileFireRateMin;

        [SerializeField]
        private float projectileFireRateMax;

        [SerializeField]
        private float projectileFireRateMutationDelta;

        [SerializeField]
        private float projectileSpreadMin;

        [SerializeField]
        private float projectileSpreadMax;

        [SerializeField]
        private float projectileSpreadMutationDelta;

        private AreaSystem _areaSystem;

        private EntityState _currentEntityState;

        private IStatProvider _statProvider;

        private IStatMutator _statMutator;

        private IStatUpdater _statUpdater;

        private float _nextMutationMoment;

        private float _nextUpdateMoment;

        private void OnTriggerEnter (Collider other)
        {
            _currentEntityState = other.GetComponent<EntityState>();

            _nextMutationMoment = Time.time + 1f / mutationRate;
            _nextUpdateMoment = Time.time + 1f / updateRate;
        }

        private void OnTriggerExit (Collider other)
        {
            _currentEntityState = null;
        }

        private void Awake ()
        {
            _areaSystem = FindObjectOfType<AreaSystem>();
            _areaSystem.AddArea(this);

            _statProvider = GetComponent<IStatProvider>();
            _statMutator = GetComponent<IStatMutator>();
            _statUpdater = GetComponent<IStatUpdater>();
        }

        private void OnDestroy ()
        {
            _areaSystem.RemoveArea(this);
        }

        private void Update ()
        {
            if (!_currentEntityState)
                return;

            HandleStatMutation();

            if (Time.time < _nextUpdateMoment)
                return;

            _statUpdater.UpdateStats(_currentEntityState);
            _nextUpdateMoment = Time.time + 1f / updateRate;
        }

        private void HandleStatMutation ()
        {
            if (Time.time < _nextMutationMoment)
                return;

            Stat stat = _statProvider.GetStat(_currentEntityState);

            float delta = 0f;
            ProjectileState projectileState = _currentEntityState.ProjectileState;
            switch (stat) {
                case Stat.Speed:
                    delta = _currentEntityState.Speed;
                    _currentEntityState.Speed =
                        _statMutator.Mutate(_currentEntityState, stat, _currentEntityState.Speed, speedMin, speedMax, speedMutationDelta);
                    delta = _currentEntityState.Speed - delta;
                    break;
                case Stat.JumpHeight:
                    delta = _currentEntityState.JumpHeight;
                    _currentEntityState.JumpHeight =
                        _statMutator.Mutate(_currentEntityState, stat, _currentEntityState.JumpHeight, jumpHeightMin, jumpHeightMax, jumpHeightMutationDelta);
                    delta = _currentEntityState.JumpHeight - delta;
                    break;
                case Stat.Gravity:
                    delta = _currentEntityState.Gravity;
                    _currentEntityState.Gravity =
                        _statMutator.Mutate(_currentEntityState, stat, _currentEntityState.Gravity, gravityMin, gravityMax, gravityMutationDelta);
                    delta = _currentEntityState.Gravity - delta;
                    break;
                case Stat.ProjectileSpeed:
                    delta = projectileState.Speed;
                    projectileState.Speed =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Speed, projectileSpeedMin, projectileSpeedMax, projectileSpeedMutationDelta);
                    delta = projectileState.Speed - delta;
                    break;
                case Stat.ProjectileDamage:
                    delta = projectileState.Damage;
                    projectileState.Damage =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Damage, projectileDamageMin, projectileDamageMax, projectileDamageMutationDelta);
                    delta = projectileState.Damage - delta;
                    break;
                case Stat.ProjectileRange:
                    delta = projectileState.Range;
                    projectileState.Range =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Range, projectileRangeMin, projectileRangeMax, projectileRangeMutationDelta);
                    delta = projectileState.Range - delta;
                    break;
                case Stat.ProjectileFireRate:
                    delta = projectileState.FireRate;
                    projectileState.FireRate =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.FireRate, projectileFireRateMin, projectileFireRateMax, projectileFireRateMutationDelta);
                    delta = projectileState.FireRate - delta;
                    break;
                case Stat.ProjectileSpread:
                    delta = projectileState.Spread;
                    projectileState.Spread =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Spread, projectileSpreadMin, projectileSpreadMax, projectileSpreadMutationDelta);
                    delta = projectileState.Spread - delta;
                    break;
                case Stat.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Encountered unknown {typeof(Stat)}: {stat.ToString()}");
            }

            _currentEntityState.ProjectileState = projectileState;

            _nextMutationMoment = Time.time + 1f / mutationRate;

            OnStatUpdated?.Invoke(stat, delta);
        }
    }
}