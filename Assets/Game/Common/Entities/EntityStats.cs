using System;
using Game.Common.Projectiles;
using UnityEngine;

namespace Game.Common.Entities
{
    [Serializable]
    public class EntityStats
    {
        [SerializeField]
        private int maxHealth = 100;

        [SerializeField]
        private int maxShield = 100;

        [Header("Movement")]
        [SerializeField]
        private float speed = 1;

        [Header("Jumping")]
        [SerializeField]
        private float jumpHeight = 1;

        [SerializeField]
        private float gravity = -9.81f;

        [SerializeField]
        private ProjectileState projectileState;

        public int MaxHealth => maxHealth;

        public int MaxShield => maxShield;

        public float Speed => speed;

        public float JumpHeight => jumpHeight;

        public float Gravity => gravity;

        public ProjectileState ProjectileState => projectileState;

        public EntityStats (int maxHealth, int maxShield, float speed, float jumpHeight, float gravity, ProjectileState projectileState)
        {
            this.maxHealth = maxHealth;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
            this.gravity = gravity;
            this.projectileState = projectileState;
        }
    }
}