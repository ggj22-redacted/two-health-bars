using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Common.Entities;
using Zenject;

namespace Game.Common.UI
{
    public class CrossAirBehaviour : MonoBehaviour
    {
        [Inject]
        private EntityRespawner _entityRespawner;
        private CanvasGroup thisGroup;
        private float delayCounter;
        private float counter;
        // Start is called before the first frame update


        private void Awake()
        {
            thisGroup = GetComponent<CanvasGroup>();
            thisGroup.alpha = 0;
            counter = 0;
        }

        void Start()
        {
            delayCounter = _entityRespawner.restartDelay;
        }

        void HideCanvas(EntityRespawner _entityRespawner)
        {
            thisGroup.alpha = 0;
            counter = delayCounter;
        }

        void ShowCanvas()
        {
            thisGroup.alpha = 1;
        }

        private void OnEnable()
        {
            _entityRespawner.OnRespawn += HideCanvas;
        }

        private void OnDisable()
        {
            _entityRespawner.OnRespawn -= HideCanvas;
        }

        // Update is called once per frame
        void Update()
        {
            if(counter > 0)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    ShowCanvas();
                }
            }
        }
    }
}