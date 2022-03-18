using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Common.GameSettings
{

    public class GameSettingsEntity : MonoBehaviour
    {

        public bool fullscreen;
        public int width;
        public int height;

        private void Awake()
        {

        }

        public void SetRes(bool value)
        {
            Screen.SetResolution(width, height, !value);
        }

        public void SetSize(float value)
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}