using System;
using System.Data;
using System.Linq;
using Game.Common.Entities;
using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Areas
{
    public class OrderStatHandler : MonoBehaviour, IStatProvider, IStatMutator
    {
        private static readonly Stat[] AllStats = Enum.GetValues(typeof(Stat)).Cast<Stat>().ToArray();

        public Stat GetStat (EntityState entityState)
        {
            EntityStats originalStats = entityState.OriginalStats;
            ProjectileState originalProjectileState = originalStats.ProjectileState;

            foreach (Stat stat in AllStats) {
                switch (stat) {
                    case Stat.Speed:
                        if (!Mathf.Approximately(originalStats.Speed, entityState.Speed))
                            return Stat.Speed;
                        break;
                    case Stat.JumpHeight:
                        if (!Mathf.Approximately(originalStats.JumpHeight, entityState.JumpHeight))
                            return Stat.JumpHeight;
                        break;
                    case Stat.Gravity:
                        if (!Mathf.Approximately(originalStats.Gravity, entityState.Gravity))
                            return Stat.Gravity;
                        break;
                    case Stat.ProjectileSpeed:
                        if (!Mathf.Approximately(originalProjectileState.Speed, entityState.ProjectileState.Speed))
                            return Stat.ProjectileSpeed;
                        break;
                    case Stat.ProjectileDamage:
                        if (!Mathf.Approximately(originalProjectileState.Damage, entityState.ProjectileState.Damage))
                            return Stat.ProjectileDamage;
                        break;
                    case Stat.ProjectileRange:
                        if (!Mathf.Approximately(originalProjectileState.Range, entityState.ProjectileState.Range))
                            return Stat.ProjectileRange;
                        break;
                    case Stat.ProjectileFireRate:
                        if (!Mathf.Approximately(originalProjectileState.FireRate, entityState.ProjectileState.FireRate))
                            return Stat.ProjectileFireRate;
                        break;
                    case Stat.ProjectileSpread:
                        if (!Mathf.Approximately(originalProjectileState.Spread, entityState.ProjectileState.Spread))
                            return Stat.ProjectileSpread;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Encountered unknown {typeof(Stat)}: {stat.ToString()}");
                }
            }

            throw new DataException($"There's no stats defined for {typeof(Stat)}");
        }

        public float Mutate (EntityState entityState, Stat stat, float value, float min, float max, float rate)
        {
            EntityStats originalStats = entityState.OriginalStats;
            ProjectileState originalProjectileState = originalStats.ProjectileState;

            switch (stat) {
                case Stat.Speed:
                    return Mathf.MoveTowards(value, originalStats.Speed, rate);
                case Stat.JumpHeight:
                    return Mathf.MoveTowards(value, originalStats.JumpHeight, rate);
                case Stat.Gravity:
                    return Mathf.MoveTowards(value, originalStats.Gravity, rate);
                case Stat.ProjectileSpeed:
                    return Mathf.MoveTowards(value, originalProjectileState.Speed, rate);
                case Stat.ProjectileDamage:
                    return Mathf.MoveTowards(value, originalProjectileState.Damage, rate);
                case Stat.ProjectileRange:
                    return Mathf.MoveTowards(value, originalProjectileState.Range, rate);
                case Stat.ProjectileFireRate:
                    return Mathf.MoveTowards(value, originalProjectileState.FireRate, rate);
                case Stat.ProjectileSpread:
                    return Mathf.MoveTowards(value, originalProjectileState.Spread, rate);
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Encountered unknown {typeof(Stat)}: {stat.ToString()}");
            }
        }
    }
}