using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Common.UI
{
    public class StageUpButtonBehaviour : MonoBehaviour
    {

        [SerializeField]
        private Image[] highlightImg;

        [SerializeField]
        private TextMeshProUGUI[] highlightText;

        [SerializeField]
        private Color32 highlightColor;

        [SerializeField]
        private Color32 normalColor;

        private Button thisButton;
        private bool activateLerp;
        private float speed;
        private float time;

        // Start is called before the first frame update
        void Start()
        {
            thisButton = GetComponent<Button>();
            activateLerp = false;
            time = 0;
            speed = 3f;
        }

        public void ColorImg(string colorTxt)
        {
            for (int x = 0; x < highlightImg.Length; x++)
            {
                switch (colorTxt)
                {
                    case "highlight":
                        activateLerp = true;
                        break;
                    default:
                    case "normal":
                        highlightImg[x].color = normalColor;
                        activateLerp = false;
                        time = 0;
                        break;
                }
            }
        }

        public void ColorTxt(string colorTxt)
        {
            for (int x = 0; x < highlightText.Length; x++)
            {
                switch (colorTxt)
                {
                    case "highlight":
                        activateLerp = true;
                        break;
                    default:
                    case "normal":
                        highlightText[x].color = normalColor;
                        activateLerp = false;
                        time = 0;
                        break;
                }            
            }
        }

        Color32 ColorLerp()
        {
            Color32 color;
            time = Mathf.PingPong(Time.unscaledTime * speed, 1);
            color = Color.Lerp(normalColor, highlightColor, time);
            return color;
        }

        // Update is called once per frame
        void Update()
        {
            if (activateLerp)
            {
                for (int x = 0; x < highlightText.Length; x++)
                {
                    highlightText[x].color = ColorLerp();
                }

                for (int x = 0; x < highlightImg.Length; x++)
                {
                    highlightImg[x].color = ColorLerp();
                }
            }
        }
    }

}