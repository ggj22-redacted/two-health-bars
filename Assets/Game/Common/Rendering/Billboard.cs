using UnityEngine;

namespace Game.Common.Rendering
{
    [RequireComponent(typeof(Renderer))]
    public class Billboard : MonoBehaviour
    {
        public new Camera camera;

        private void LateUpdate()
        {
            Camera c = camera ? camera : Camera.main;
            if (!c)
                return;

            Transform cameraTransform = c.transform;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraUp = cameraTransform.up;

            transform.LookAt(transform.position - cameraForward, cameraUp);
        }
    }
}