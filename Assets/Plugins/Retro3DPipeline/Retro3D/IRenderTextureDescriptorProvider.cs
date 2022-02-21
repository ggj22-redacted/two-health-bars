using UnityEngine;

namespace Retro3D
{
    public interface IRenderTextureDescriptorProvider
    {
        RenderTextureDescriptor RenderTextureDescriptor { get; }
    }
}