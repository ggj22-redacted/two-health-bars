using System;
using UnityEngine;

namespace Game.Common.Projectiles
{
    [Serializable]
    public struct ProjectileState
    {
        [SerializeField]
        private float damage;

        [SerializeField]
        private float fireRate;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float size;

        [SerializeField]
        private float range;

        [SerializeField]
        private float spread;

        [SerializeField]
        private float lifetime;

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public float Size
        {
            get => size;
            set => size = value;
        }

        public float Range
        {
            get => range;
            set => range = value;
        }

        public float Spread
        {
            get => spread;
            set => spread = value;
        }

        public float Lifetime
        {
            get => lifetime;
            set => lifetime = value;
        }
    }
}