using Retro3D;
using UnityEngine;
using UnityEngine.Rendering;

namespace CRT
{
    [ExecuteInEditMode]
    public class CRTEffect : MonoBehaviour, IOnRenderImage
    {
        [SerializeField]
        private Shader shader;

        [SerializeField, Range(0, 1)]
        private float vignetSize = 0.2f;

        [SerializeField, Range(0, 10)]
        private float vignetDarkening = 1.3f;
        
        [SerializeField, Range(0, 1)]
        private float scanlineIntensity = 1;

        [SerializeField, Range(0, 1)]
        private float colorShift = 0.2f;

        [SerializeField, Range(0, 1)]
        private float noiseX;

        [SerializeField, Range(0, 1)]
        private float rgbNoise;

        [SerializeField] [Range(0, 1)]
        private float sinNoiseScale;

        [SerializeField, Range(0, 10)]
        private float sinNoiseWidth;
        
        [SerializeField]
        private float sinNoiseOffset;

        [SerializeField]
        private Vector2 offset;

        [SerializeField, Range(0, 2)]
        private float scanLineTail = 1.5f;

        [SerializeField, Range(-10, 10)]
        private float scanLineSpeed = 10;

        private Material _material;

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

            Material.SetFloat("_VignetSize", vignetSize);
            Material.SetFloat("_VignetDarkening", vignetDarkening);
            Material.SetFloat("_ScanlineIntensity", scanlineIntensity);
            Material.SetFloat("_ColorShift", colorShift);
            Material.SetFloat("_NoiseX", noiseX);
            Material.SetFloat("_RGBNoise", rgbNoise);
            Material.SetFloat("_SinNoiseScale", sinNoiseScale);
            Material.SetFloat("_SinNoiseWidth", sinNoiseWidth);
            Material.SetFloat("_SinNoiseOffset", sinNoiseOffset);
            Material.SetFloat("_ScanLineSpeed", scanLineSpeed);
            Material.SetFloat("_ScanLineTail", scanLineTail);
            Material.SetVector("_Offset", offset);
            Material.SetFloat("_CurrentTime", Time.unscaledTime);

            cb.Blit(source, destination, Material, 0);
        }
    }
}