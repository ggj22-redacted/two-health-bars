using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Retro3D
{
    public class RenderTextureDescriptorProvider : MonoBehaviour, IRenderTextureDescriptorProvider
    {
        [SerializeField, Min(0)]
        private int width = 320;

        [SerializeField, Min(0)]
        private int height = 240;

        [SerializeField]
        private Format format;

        [SerializeField]
        private GraphicsFormat graphicsFormat = GraphicsFormat.RGBA_DXT5_UNorm;

        [SerializeField]
        private RenderTextureFormat renderTextureFormat = RenderTextureFormat.Default;

        [SerializeField]
        private int depthBufferBits = 24;

        public RenderTextureDescriptor RenderTextureDescriptor => format == Format.UseGraphicsFormat
            ? new RenderTextureDescriptor(width, height, graphicsFormat, depthBufferBits)
            : new RenderTextureDescriptor(width, height, renderTextureFormat, depthBufferBits);

        private enum Format
        {
            UseGraphicsFormat,
            UseRenderTextureFormat
        }
    }
}