using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Zenject;
using System;

namespace Game.Common.UI
{
    public class UISystemEntity : MonoBehaviour
    {

        public event Action<UISystemEntity> OnStart;
        public event Action<UISystemEntity> OnMenu;
        public event Action<UISystemEntity> OffMenu;


        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void ActivateMenu()
        {
            OnMenu?.Invoke(this);
        }

        public void DeactivateMenu()
        {
            OffMenu?.Invoke(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time == 0)
            {
                OnStart?.Invoke(this);
            }
        }
    }
}