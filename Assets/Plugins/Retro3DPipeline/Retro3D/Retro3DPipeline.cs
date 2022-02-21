// Retro3DPipeline
// A minimal example of a custom render pipeline with the Retro3D shader.
// https://github.com/keijiro/Retro3DPipeline

using UnityEngine;
using UnityEngine.Rendering;

namespace Retro3D
{
    // Render pipeline runtime class
    public class Retro3DPipeline : RenderPipeline
    {
        private static readonly ShaderTagId PassNameDefault =
            new ShaderTagId("SRPDefaultUnlit"); //The shader pass tag for replacing shaders without pass

        // Temporary command buffer
        // Reused between frames to avoid GC allocation.
        // Rule: Clear commands right after calling ExecuteCommandBuffer.
        private CommandBuffer _cb;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_cb == null)
                return;

            _cb.Dispose();
            _cb = null;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            //base.Render(context, cameras);

            // Lazy initialization of the temporary command buffer.
            _cb ??= new CommandBuffer();

            // Constants used in the camera render loop.
            var defaultRTDesc = new RenderTextureDescriptor(298, 224, RenderTextureFormat.Default, 24);
            var rtID = Shader.PropertyToID("_LowResScreen");

            foreach (Camera camera in cameras)
            {
                // Set the camera up.
                context.SetupCameraProperties(camera);

                // Setup commands: Initialize the temporary render texture.
                _cb.name = "Setup";

                RenderTextureDescriptor rtDesc = defaultRTDesc;
#if UNITY_EDITOR
                if (camera.cameraType == CameraType.SceneView) {
                    rtDesc = new RenderTextureDescriptor(camera.scaledPixelWidth, camera.scaledPixelHeight, RenderTextureFormat.Default, 24);
                    _cb.GetTemporaryRT(rtID, rtDesc);
                    _cb.SetRenderTarget(rtID);
                } else {
#endif
                    if (camera.targetTexture) {
                        rtDesc = camera.targetTexture.descriptor;
                        _cb.SetRenderTarget(camera.targetTexture);
                    }
                    else {
                        IRenderTextureDescriptorProvider rtdProvider = camera.GetComponent<IRenderTextureDescriptorProvider>();
                        if (rtdProvider != null) {
                            rtDesc = rtdProvider.RenderTextureDescriptor;
                            //camera.rect = new Rect(0, 0, rtDesc.width / 320f, rtDesc.height / 240f);
                        }

                        _cb.GetTemporaryRT(rtID, rtDesc);
                        _cb.SetRenderTarget(rtID);
                    }
#if UNITY_EDITOR
                }
#endif
                // TODO: this is not a set of flags
                bool depthOnly = camera.clearFlags == CameraClearFlags.Depth;
                _cb.ClearRenderTarget(true, true, camera.backgroundColor);
                context.ExecuteCommandBuffer(_cb);
                _cb.Clear();

#if UNITY_EDITOR
                if (camera.cameraType == CameraType.SceneView)
                    ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
#endif

                // Do basic culling.
                bool cull = camera.TryGetCullingParameters(out ScriptableCullingParameters scp);
                if (!cull)
                    continue;

                var culled = context.Cull(ref scp);

                //// Render visible objects that has "Base" light mode tag.
                var sorting = new SortingSettings(camera);

                DrawingSettings defaultSettings = new DrawingSettings(PassNameDefault, sorting);
                defaultSettings.SetShaderPassName(1, PassNameDefault); 

                var settings = new DrawingSettings(new ShaderTagId("Base"), sorting);
                var filter = FilteringSettings.defaultValue;
                filter.renderQueueRange = RenderQueueRange.opaque;
                context.DrawRenderers(culled, ref settings, ref filter);

                context.DrawRenderers(culled, ref defaultSettings, ref filter);

                if (camera.clearFlags == CameraClearFlags.Skybox)
                    context.DrawSkybox(camera);

                settings = new DrawingSettings(new ShaderTagId("Transparent"), sorting);
                filter = FilteringSettings.defaultValue;
                //filter.renderQueueRange = RenderQueueRange.transparent;
                context.DrawRenderers(culled, ref settings, ref filter);

                context.DrawRenderers(culled, ref defaultSettings, ref filter);

                // Blit the render result to the camera destination.
                _cb.name = "Blit";
                _cb.Blit(rtID, BuiltinRenderTextureType.CameraTarget);
                context.ExecuteCommandBuffer(_cb);
                _cb.Clear();

                IOnRenderImage[] onRenderImage = camera.GetComponents<IOnRenderImage>();
                if (onRenderImage.Length > 0) {
                    for (int i = 0; i < onRenderImage.Length; i++) {
                        onRenderImage[i].RenderImage(_cb, rtID, BuiltinRenderTextureType.CameraTarget);
                    }
                    context.ExecuteCommandBuffer(_cb);
                    _cb.Clear();
                }

#if UNITY_EDITOR
                if (camera.cameraType == CameraType.SceneView)
                    context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
#endif

                context.Submit();
            }
        }
    }
}
