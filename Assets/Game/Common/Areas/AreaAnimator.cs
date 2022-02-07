using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Common.Areas
{
    public class AreaAnimator : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve offsetSpeedX;
        
        [SerializeField]
        private AnimationCurve offsetSpeedY;

        [SerializeField, Range(0, 1)]
        private float speed = 1;

        [SerializeField]
        private Renderer _renderer;

        private Vector2 _offset = Vector2.zero;

        private float start;

        private void Start ()
        {
            start = Random.Range(0f, 1000f);
        }

        private void Update ()
        {
            float currentTime = Time.time;
            float offsetX = currentTime + offsetSpeedX.Evaluate(start + currentTime) * speed;
            float offsetY = currentTime + offsetSpeedY.Evaluate(start + currentTime) * speed;

            _renderer.material.SetTextureOffset("_MainTex",  new Vector2(offsetX, offsetY));
        }
    }
}