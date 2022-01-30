using Game.Common.Areas;
using Game.Common.Enemies.Types;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Game.Common.Game
{
    public class GameStage : MonoBehaviour
    {
        [SerializeField]
        private float shieldDelta;

        [SerializeField]
        private float shieldUpdateRate;

        [SerializeField]
        private EnemyType enemyType;

        [SerializeField]
        private Transform spawnOrigin;

        [SerializeField]
        private Rect spawnArea;

        public EnemyType EnemyType => enemyType;

        private GameStateSystem _gameStateSystem;

        private Random _random;

        public Vector3 SpawnLocation
        {
            get
            {
                float x = spawnArea.width * (float)_random.NextDouble() - spawnArea.width / 2;
                float z = spawnArea.height * (float)_random.NextDouble() - spawnArea.height / 2;

                return spawnOrigin.position + new Vector3(x, 0, z);
            }
        }

        public float ShieldDelta => shieldDelta;

        public float ShieldUpdateRate => shieldUpdateRate;

        public Area Area { get; private set; }

        private void Awake ()
        {
            Area = GetComponent<Area>();
            Area.CanBeDestroyed = false;
        }

        private void OnDestroy ()
        {
            if (_gameStateSystem)
                _gameStateSystem.UnregisterStage(this);
        }

        [Inject]
        private void Initialize (GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;

            gameStateSystem.RegisterStage(this);

            _random = new Random();
        }
    }
}