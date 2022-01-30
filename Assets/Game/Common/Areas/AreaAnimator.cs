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
            float speedX = offsetSpeedX.Evaluate(start + Time.time) * speed;
            float speedY = offsetSpeedY.Evaluate(start + Time.time) * speed;

            _offset += new Vector2(speedX, speedY);

            _renderer.material.SetTextureOffset("_MainTex", _offset);
        }
    }
}