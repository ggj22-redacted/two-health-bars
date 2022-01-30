using UnityEngine;

public class WorldToScreenFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        if(target) {
            var wantedPos = Camera.main.WorldToScreenPoint(target.position);
            transform.position = wantedPos;
        }
    }
}
