using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Game.Common.Areas
{
    public class ChaosStatHandler : MonoBehaviour, IStatProvider, IStatMutator
    {
        private static readonly Stat[] AllStats = Enum.GetValues(typeof(Stat)).Cast<Stat>().ToArray();

        private readonly Random _random = new Random();

        public Stat GetStat (EntityState entityState)
        {
            int index = _random.Next(0, AllStats.Length);

            return AllStats[index];
        }

        public float Mutate (EntityState entityState, Stat stat, float value, float min, float max, float rate)
        {
            // generating delta and placing it in range [-1, 1]
            int delta = _random.Next(0, 2) * 2 - 1;
            value += delta * rate;
            value = Mathf.Clamp(value, min, max);

            return value;
        }
    }
}