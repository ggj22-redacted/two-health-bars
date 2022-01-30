using UnityEngine;

namespace Game.Common.Entities
{
    public class EntityKiller : MonoBehaviour
    {
        private void OnTriggerEnter (Collider other)
        {
            EntityState entityState = other.GetComponent<EntityState>();

            if (entityState)
                entityState.Health -= float.MaxValue;
        }
    }
}