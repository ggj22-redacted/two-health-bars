using UnityEngine;

namespace Game.Common.Player
{
    public class PlayerHeadControl : MonoBehaviour
    {
        [SerializeField]
        private Transform head;

        [SerializeField]
        private Transform headPivot;

        [SerializeField]
        private float headSpeed = 1f;

        [SerializeField]
        private Camera mainCamera;

        private Vector3 target;

        private void Update ()
        {
            Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(cameraCenter, mainCamera.transform.forward, out RaycastHit hit, 100f)) {
                target = hit.point;
            } else {
                target = cameraCenter + mainCamera.transform.forward * 100f;
            }

            headPivot.rotation = Quaternion.LookRotation(target - headPivot.position);
        }
    }
}