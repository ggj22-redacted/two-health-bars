using System.Collections;
using UnityEngine;

namespace Game.Common.Enemies
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");

        [Header("Model")]
        [SerializeField]
        private GameObject model;
        
        [SerializeField, Range(0, 3)]
        private float modelDeactivationDelay;

        [Header("Effect")]
        [SerializeField]
        private ParticleSystem deathEffect;

        [SerializeField, Range(0, 3)]
        private float effectPlayDelay;

        [Header("")]
        [SerializeField]
        private Animator animator;

        private Coroutine _handler;

        public void Execute ()
        {
            _handler ??= StartCoroutine(HandleDeath());
        }

        private IEnumerator HandleDeath ()
        {
            float startMoment = Time.time;
            bool effectPlayed = false;

            animator.SetBool(Death, true);

            while (!model.activeSelf || !effectPlayed) {
                if (model.activeSelf && Time.time - startMoment >= modelDeactivationDelay)
                    model.SetActive(false);

                if (!effectPlayed && Time.time - startMoment >= effectPlayDelay) {
                    deathEffect.Play();
                    effectPlayed = true;
                }

                yield return null;
            }
        }
    }
}