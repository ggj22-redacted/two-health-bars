using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Common.Entities;
using Zenject;


namespace Game.Common.UI
{
    public class HudWarningBehaviour : MonoBehaviour
    {
        [Inject]
        private EntityRespawner _entityRespawner;

        private Animator thisAnim;
        private float time = 0;
        private int counter = 0;
        private CanvasGroup thisGroup;

        [SerializeField]
        private TMP_Text counterText;
        public float delayWave;
        private bool waveActive;

        void Awake()
        {
            thisAnim = GetComponent<Animator>();
            thisGroup = GetComponent<CanvasGroup>();
            thisGroup.alpha = 0;
            waveActive = false;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        void WaveStart(EntityRespawner _entityRespawner)
        {
            RestartCounter();
        }

        private void OnEnable()
        {
            _entityRespawner.OnRespawn += WaveStart;
        }

        private void OnDisable()
        {
            _entityRespawner.OnRespawn -= WaveStart;
        }

        void UpdateCounterText(float updatedCounter)
        {
            thisAnim.SetTrigger("Countdown");
            counterText.text = updatedCounter.ToString();
        }

        void RestartCounter()
        {
            thisGroup.alpha = 1;
            counter = Mathf.RoundToInt(delayWave);
            time = delayWave;
            thisAnim.SetTrigger("Reset");
            waveActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (waveActive)
            {
                if (counter > 0)
                    time = time - Time.deltaTime;
                if (counter > Mathf.RoundToInt(time))
                {
                    counter = Mathf.RoundToInt(time);
                    if (counter == 0)
                        thisAnim.SetTrigger("Go");
                    UpdateCounterText(counter);
                }
            }
        }
    }
}