using UnityEngine;
using UnityEngine.Rendering;

namespace Retro3D
{
    public interface IOnRenderImage
    {
        void RenderImage (CommandBuffer cb, RenderTargetIdentifier source, RenderTargetIdentifier destination);
    }
}