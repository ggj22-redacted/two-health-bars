using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.UI
{
    public class GearViewBehaviour : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer bodyMesh;
        [SerializeField]
        private MeshRenderer headMesh;

        [SerializeField]
        private Material[] bodyMaterials;
        [SerializeField]
        private Material[] headMaterials;

        public Button selectButton;
        [SerializeField]
        private Button leftButton;
        [SerializeField]
        private Button rightButton;

        private int materialPos;

        // Start is called before the first frame update
        void Start()
        {
            materialPos = 0;
            bodyMesh.material = bodyMaterials[materialPos];
            headMesh.material = headMaterials[materialPos];
        }

        void CycleLeftMaterials()
        {
            DoCycle(-1);
        }

        void CycleRightMaterials()
        {
            DoCycle(+1);
        }

        void DoCycle(int direction)
        {
            materialPos += direction;
            if (materialPos < 0)
                materialPos = bodyMaterials.Length - 1;

            if (materialPos == bodyMaterials.Length)
                materialPos = 0;

            bodyMesh.material = bodyMaterials[materialPos];
            headMesh.material = headMaterials[materialPos];

        }

        private void OnEnable()
        {
            leftButton.onClick.AddListener(CycleLeftMaterials);
            rightButton.onClick.AddListener(CycleRightMaterials);
        }

        private void OnDisable()
        {
            leftButton.onClick.RemoveListener(CycleLeftMaterials);
            rightButton.onClick.RemoveListener(CycleRightMaterials);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}