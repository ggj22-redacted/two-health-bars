using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.UI
{
    public class WindowBehaviour : MonoBehaviour
    {

        public bool firstWindow;
        public bool inputWindow;
        public Button backButton;
        public Button gearButton;
        public GameObject secondWindow;
        private GameObject createdWindow;


        private CanvasGroup thisCanvas;
        public CanvasGroup recoverCanvas;

        [SerializeField]
        private GameObject audioObject;

        private AudioSource[] audioObjectSources;
        private AudioSource moveSound;
        private AudioSource positiveSound;
        private AudioSource negativeSound;
        private CanvasGroup gearCanvas;
        private Button gearCanvasButton;

        private void Awake()
        {
            if (inputWindow)
            {
                gearCanvas = transform.parent.transform.Find("GearView").GetComponent<CanvasGroup>();
                gearCanvasButton = gearCanvas.gameObject.GetComponent<GearViewBehaviour>().selectButton;
            }
        }

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
                createdWindow = Instantiate(secondWindow, gameObject.transform.parent);
                createdWindow.GetComponent<WindowBehaviour>().recoverCanvas = recoverCanvas;
            }

            Destroy(gameObject);
        }

        void ShowGearView()
        {
            positiveSound.Play();
            GearView(true, 1);
        }

        void HideGearView()
        {
            GearView(false, 0);
        }

        void GearView(bool value, int alpha)
        {
            thisCanvas.interactable = !value;
            thisCanvas.blocksRaycasts = !value;
            gearCanvas.alpha = alpha;
            gearCanvas.interactable = value;
            gearCanvas.blocksRaycasts = value;
            gearCanvas.gameObject.transform.SetAsLastSibling();
        }

        public void PlayMoveSound()
        {
            moveSound.Play();
        }

        private void OnEnable()
        {
            backButton.onClick.AddListener(CallWindowDestruction);
            if (gearButton)
                gearButton.onClick.AddListener(ShowGearView);
            if (gearCanvasButton)               
                gearCanvasButton.onClick.AddListener(HideGearView);
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(CallWindowDestruction);

            if (gearButton)
                gearButton.onClick.RemoveListener(ShowGearView);
            if (gearCanvasButton)
                gearCanvasButton.onClick.RemoveListener(HideGearView);
        }

        private void OnDestroy()
        {
            if (!firstWindow)
            {
                recoverCanvas.alpha = 1;
                recoverCanvas.interactable = true;
                recoverCanvas.blocksRaycasts = true;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}