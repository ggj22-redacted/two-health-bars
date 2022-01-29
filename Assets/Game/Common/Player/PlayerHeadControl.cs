using UnityEngine;
using Zenject;

namespace Game.Common.Player
{
    public class PlayerHeadControl : MonoBehaviour
    {
        [SerializeField]
        private Transform headPivot;

        [SerializeField, Min(0)]
        private float minTargetDistance;

        [SerializeField, Min(0)]
        private float maxTargetDistance;

        [Inject]
        private Camera mainCamera;

        private Vector3 _target;

        private void Update ()
        {
            Vector3 origin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))
                             + mainCamera.transform.forward * minTargetDistance;
            if (Physics.Raycast(origin, mainCamera.transform.forward, out RaycastHit hit, maxTargetDistance)) {
                _target = hit.point;
            } else {
                _target = origin + mainCamera.transform.forward * maxTargetDistance;
            }

            headPivot.rotation = Quaternion.LookRotation(_target - headPivot.position);
        }
    }
}