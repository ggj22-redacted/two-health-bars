using System;
using UnityEngine;

namespace Game.Common.Projectiles
{
    [Serializable]
    public struct ProjectileState
    {
        public Material material;

        [SerializeField]
        private float damage;

        [SerializeField, Min(0)]
        private float fireRate;

        [SerializeField, Min(0)]
        private float speed;

        [SerializeField, Min(0)]
        private float size;

        [SerializeField, Min(0)]
        private int count;

        [SerializeField, Min(0)]
        private float range;

        [SerializeField, Min(0)]
        private float spread;

        [SerializeField, Min(0)]
        private float lifetime;

        [SerializeField]
        private AudioClip shootClip;

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

        public int Count
        {
            get => count;
            set => count = value;
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

        public AudioClip ShootClip
        {
            get => shootClip;
            set => shootClip = value;
        }
    }
}