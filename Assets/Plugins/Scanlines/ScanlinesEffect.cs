using System;
using Retro3D;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scanlines
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Camera/Scanlines Effect")]
    public class ScanlinesEffect : MonoBehaviour, IOnRenderImage
    {
        private static readonly int LineWidth = Shader.PropertyToID("_LineWidth");

        private static readonly int Hardness = Shader.PropertyToID("_Hardness");

        private static readonly int Speed = Shader.PropertyToID("_Speed");

        private static readonly int CurrentTime = Shader.PropertyToID("_CurrentTime");

        public Shader shader;

        private Material _material;

        [Range(0, 10)]
        public float lineWidth = 2f;

        [Range(0, 1)]
        public float hardness = 0.9f;

        [Range(0, 1)]
        public float displacementSpeed = 0.1f;

        private Material Material
        {
            get
            {
                if (_material)
                    return _material;

                _material = new Material(shader) {
                    hideFlags = HideFlags.HideAndDontSave
                };

                return _material;
            }
        }

        private void OnDisable ()
        {
            if (_material)
                DestroyImmediate(_material);
        }

        public void RenderImage (CommandBuffer cb, RenderTargetIdentifier source, RenderTargetIdentifier destination)
        {
            if (!shader || !enabled)
                return;

            Material.SetFloat(LineWidth, lineWidth);
            Material.SetFloat(Hardness, hardness);
            Material.SetFloat(Speed, displacementSpeed);
            Material.SetFloat(CurrentTime, Time.unscaledTime);

            cb.Blit(source, destination, Material, 0);
        }
    }
}