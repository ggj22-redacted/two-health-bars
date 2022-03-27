using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.UI
{
    public class WindowBehaviour : MonoBehaviour
    {

        public bool firstWindow;
        public Button backButton;
        public GameObject secndWindow;
        private GameObject createdWindow;

        private CanvasGroup thisCanvas;
        public CanvasGroup recoverCanvas;

        [SerializeField]
        private GameObject audioObject;

        private AudioSource[] audioObjectSources;
        private AudioSource moveSound;
        private AudioSource positiveSound;
        private AudioSource negativeSound;

        // Start is called before the first frame update
        void Start()
        {
            thisCanvas = GetComponent<CanvasGroup>();
            audioObject = transform.parent.transform.Find("UiSoundsFX").gameObject;
            audioObjectSources = audioObject.GetComponentsInChildren<AudioSource>();
            moveSound = audioObjectSources[0];
            positiveSound = audioObjectSources[1];
            negativeSound = audioObjectSources[2];
        }

        void CallWindowDestruction()
        {
            negativeSound.Play();

            if (firstWindow)
            {
                createdWindow = Instantiate(secndWindow, gameObject.transform.parent);
                createdWindow.GetComponent<WindowBehaviour>().recoverCanvas = recoverCanvas;
            }

            Destroy(gameObject);
        }

        public void PlayMoveSound()
        {
            moveSound.Play();
        }

        private void OnEnable()
        {
            backButton.onClick.AddListener(CallWindowDestruction);
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(CallWindowDestruction);
        }

        private void OnDestroy()
        {
            if (!firstWindow)
            {
                recoverCanvas.alpha = 1;
                recoverCanvas.interactable = true;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}