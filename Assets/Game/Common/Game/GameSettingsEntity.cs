using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

namespace Game.Common.GameSettings
{

    public class GameSettingsEntity : MonoBehaviour
    {

        public bool fullscreen;
        public int width;
        public int height;
        public AudioMixer audioMixer;

        public float masterAudio;
        public float musicAudio;
        public float effectAudio;

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

        public void SettingsSave()
        {
            PlayerPrefs.Save();
        }

        void GetAudio()
        {
            masterAudio = PlayerPrefs.GetFloat("MasterVol", 1.0f);
            musicAudio = PlayerPrefs.GetFloat("MusicVol", 1.0f);
            effectAudio = PlayerPrefs.GetFloat("EffectVol", 1.0f);
        }

        public void SetAudio(float value, string mixer)
        {
            switch (mixer)
            {
                case "master":
                    audioMixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
                    PlayerPrefs.SetFloat("MasterVol", value);
                    GetAudio();
                    break;
                case "music":
                    audioMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
                    PlayerPrefs.SetFloat("MusicVol", value);
                    GetAudio();
                    break;
                case "effect":
                    audioMixer.SetFloat("EffectVol", Mathf.Log10(value) * 20);
                    PlayerPrefs.SetFloat("EffectVol", value);
                    GetAudio();
                    break;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GetAudio();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}