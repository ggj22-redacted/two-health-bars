using System;
using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Areas
{
    public class Area : MonoBehaviour
    {
        public event Action OnPlayerEntered;

        public event Action OnPlayerLeft;

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

        private IStatProvider _statProvider;

        private IStatMutator _statMutator;

        private IStatUpdater _statUpdater;

        private float _nextMutationMoment;

        private float _nextUpdateMoment;

        public EntityState EntityState { get; set; }

        private void OnTriggerEnter (Collider other)
        {
            EntityState = other.GetComponent<EntityState>();

            _nextMutationMoment = Time.time + 1f / mutationRate;
            _nextUpdateMoment = Time.time + 1f / updateRate;

            if (EntityState)
                OnPlayerEntered?.Invoke();
        }

        private void OnTriggerExit (Collider other)
        {
            if (!EntityState)
                return;

            EntityState = null;
            OnPlayerLeft?.Invoke();
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
            if (!EntityState)
                return;

            if (Time.time >= _nextMutationMoment)
                HandleStatMutation(EntityState);

            if (Time.time >= _nextUpdateMoment)
                HandleStatUpdate(EntityState);
        }

        public void HandleStatMutation (EntityState entityState)
        {
            Stat stat = _statProvider.GetStat(entityState);

            float delta = 0f;
            ProjectileState projectileState = entityState.ProjectileState;
            switch (stat) {
                case Stat.Speed:
                    delta = entityState.Speed;
                    entityState.Speed =
                        _statMutator.Mutate(entityState, stat, entityState.Speed, speedMin, speedMax, speedMutationDelta);
                    delta = entityState.Speed - delta;
                    break;
                case Stat.JumpHeight:
                    delta = entityState.JumpHeight;
                    entityState.JumpHeight =
                        _statMutator.Mutate(entityState, stat, entityState.JumpHeight, jumpHeightMin, jumpHeightMax, jumpHeightMutationDelta);
                    delta = entityState.JumpHeight - delta;
                    break;
                case Stat.Gravity:
                    delta = entityState.Gravity;
                    entityState.Gravity =
                        _statMutator.Mutate(entityState, stat, entityState.Gravity, gravityMin, gravityMax, gravityMutationDelta);
                    delta = entityState.Gravity - delta;
                    break;
                case Stat.ProjectileSpeed:
                    delta = projectileState.Speed;
                    projectileState.Speed =
                        _statMutator.Mutate(entityState, stat, projectileState.Speed, projectileSpeedMin, projectileSpeedMax, projectileSpeedMutationDelta);
                    delta = projectileState.Speed - delta;
                    break;
                case Stat.ProjectileDamage:
                    delta = projectileState.Damage;
                    projectileState.Damage =
                        _statMutator.Mutate(entityState, stat, projectileState.Damage, projectileDamageMin, projectileDamageMax, projectileDamageMutationDelta);
                    delta = projectileState.Damage - delta;
                    break;
                case Stat.ProjectileRange:
                    delta = projectileState.Range;
                    projectileState.Range =
                        _statMutator.Mutate(entityState, stat, projectileState.Range, projectileRangeMin, projectileRangeMax, projectileRangeMutationDelta);
                    delta = projectileState.Range - delta;
                    break;
                case Stat.ProjectileFireRate:
                    delta = projectileState.FireRate;
                    projectileState.FireRate =
                        _statMutator.Mutate(entityState, stat, projectileState.FireRate, projectileFireRateMin, projectileFireRateMax, projectileFireRateMutationDelta);
                    delta = projectileState.FireRate - delta;
                    break;
                case Stat.ProjectileSpread:
                    delta = projectileState.Spread;
                    projectileState.Spread =
                        _statMutator.Mutate(entityState, stat, projectileState.Spread, projectileSpreadMin, projectileSpreadMax, projectileSpreadMutationDelta);
                    delta = projectileState.Spread - delta;
                    break;
                case Stat.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Encountered unknown {typeof(Stat)}: {stat.ToString()}");
            }

            entityState.ProjectileState = projectileState;

            _nextMutationMoment = Time.time + 1f / mutationRate;

            OnStatUpdated?.Invoke(stat, delta);
        }

        public void HandleStatUpdate (EntityState entityState)
        {
            _statUpdater.UpdateStats(entityState);
            _nextUpdateMoment = Time.time + 1f / updateRate;
        }
    }
}