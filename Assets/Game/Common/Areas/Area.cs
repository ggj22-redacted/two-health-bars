using System;
using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Areas
{
    public class Area : MonoBehaviour
    {
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
            _statProvider = GetComponent<IStatProvider>();
            _statMutator = GetComponent<IStatMutator>();
            _statUpdater = GetComponent<IStatUpdater>();
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

            ProjectileState projectileState = _currentEntityState.ProjectileState;
            switch (stat) {
                case Stat.Speed:
                    _currentEntityState.Speed =
                        _statMutator.Mutate(_currentEntityState, stat, _currentEntityState.Speed, speedMin, speedMax, speedMutationDelta);
                    break;
                case Stat.JumpHeight:
                    _currentEntityState.JumpHeight =
                        _statMutator.Mutate(_currentEntityState, stat, _currentEntityState.JumpHeight, jumpHeightMin, jumpHeightMax, jumpHeightMutationDelta);
                    break;
                case Stat.Gravity:
                    _currentEntityState.Gravity =
                        _statMutator.Mutate(_currentEntityState, stat, _currentEntityState.Gravity, gravityMin, gravityMax, gravityMutationDelta);
                    break;
                case Stat.ProjectileSpeed:
                    projectileState.Speed =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Speed, projectileSpeedMin, projectileSpeedMax, projectileSpeedMutationDelta);
                    break;
                case Stat.ProjectileDamage:
                    projectileState.Damage =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Damage, projectileDamageMin, projectileDamageMax, projectileDamageMutationDelta);
                    break;
                case Stat.ProjectileRange:
                    projectileState.Range =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Range, projectileRangeMin, projectileRangeMax, projectileRangeMutationDelta);
                    break;
                case Stat.ProjectileFireRate:
                    projectileState.FireRate =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.FireRate, projectileFireRateMin, projectileFireRateMax, projectileFireRateMutationDelta);
                    break;
                case Stat.ProjectileSpread:
                    projectileState.Spread =
                        _statMutator.Mutate(_currentEntityState, stat, projectileState.Spread, projectileSpreadMin, projectileSpreadMax, projectileSpreadMutationDelta);
                    break;
                case Stat.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Encountered unknown {typeof(Stat)}: {stat.ToString()}");
            }

            _currentEntityState.ProjectileState = projectileState;

            _nextMutationMoment = Time.time + 1f / mutationRate;
        }
    }
}