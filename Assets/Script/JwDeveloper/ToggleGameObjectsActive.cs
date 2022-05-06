using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace JwDeveloper.Utility
{

    public class ToggleGameObjectsActive : MonoBehaviour
    {
        [Tooltip("Set true to set gameobjects active with default value on Awake()")]
        public bool setValueOnAwake = false;
        [Tooltip("if 'SetDefaultValue', set this to true to ToggleOn, while set it false to ToggleOff \nOnly affect on Awake()")]
        public bool valueOnAwake = true;
        public bool isToggleOn = false;
        [Space(10)]
        public GameObject[] activeObjects;
        public GameObject[] inactiveObjects;


        private void Awake()
        {
            if (setValueOnAwake)
            {
                ToggleGameObjects(valueOnAwake);
            }
        }

        public void SetGameObjectsActiveStatus(bool active)
        {
            foreach (var go in activeObjects)
            {
                if (go) go.SetActive(active);
            }

            foreach (var go in inactiveObjects)
            {
                if (go) go.SetActive(!active);
            }
        }

        public void ToggleGameObjects()
        {
            ToggleGameObjects(!isToggleOn);
        }

        public void ToggleGameObjects(bool active)
        {
            isToggleOn = active;
            SetGameObjectsActiveStatus(active);
        }

        public void ToggleGameObjects_On()
        {
            ToggleGameObjects(true);
        }

        public void ToggleGameObjects_Off()
        {
            ToggleGameObjects(false);
        }
    }
}