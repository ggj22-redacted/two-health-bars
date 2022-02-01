using UnityEngine;

namespace Game.Common.Rendering
{
    public class RendererEmissionControl : MonoBehaviour
    {
        private static readonly int Emission = Shader.PropertyToID("_Emission");

        [SerializeField]
        private Renderer referenceRenderer;

        [SerializeField]
        private Color emissionColor;

        [SerializeField, Range(-10, 10)]
        private float intensity;

        private void OnValidate()
        {
            if (Application.isPlaying)
                SetEmission();
        }

        public void SetEmission(Color color, float intensity)
        {
            referenceRenderer.material.SetColor(Emission, color * Mathf.Pow(2f, intensity));
        }

        public void SetEmission (Color color) => SetEmission(color, intensity);

        public void SetEmission (float intensity) => SetEmission(emissionColor, intensity);

        public void SetEmission () => SetEmission(emissionColor, intensity);
    }
}