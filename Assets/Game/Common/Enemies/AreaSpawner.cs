using Cysharp.Threading.Tasks;
using Game.Common.Areas;
using UnityEngine;
using Zenject;

namespace Game.Common.Enemies
{
    public class AreaSpawner : MonoBehaviour
    {
        [SerializeField]
        private Area area;

        [SerializeField]
        private float spawnDelay;

        [Inject]
        private EntityState _entityState;

        public void Spawn ()
        {
            Spawn(spawnDelay).Forget();
        }

        public async UniTaskVoid Spawn (float delay)
        {
            if (_entityState.Health > 0)
                return;

            Vector3 position = _entityState.transform.position;
            position.y = 0;

            float endMoment = Time.time + delay;
            await UniTask.WaitUntil(() => Time.time >= endMoment);

            Instantiate(area, position, Quaternion.identity);
        }
    }
}