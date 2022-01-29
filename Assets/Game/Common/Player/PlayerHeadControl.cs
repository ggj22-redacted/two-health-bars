using UnityEngine;

namespace Game.Common.Player
{
    public class PlayerHeadControl : MonoBehaviour
    {
        [SerializeField]
        private Transform headPivot;

        [SerializeField]
        private Camera mainCamera;

        private Vector3 _target;

        private void Update ()
        {
            Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(cameraCenter, mainCamera.transform.forward, out RaycastHit hit, 100f)) {
                _target = hit.point;
            } else {
                _target = cameraCenter + mainCamera.transform.forward * 100f;
            }

            headPivot.rotation = Quaternion.LookRotation(_target - headPivot.position);
        }
    }
}