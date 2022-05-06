using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace JwDeveloper.Utility
{
    public class SimpleButtonFunction : MonoBehaviour
    {
        public bool toggle = false;
        public UnityEvent toggleEventOn;
        public UnityEvent toggleEventOff;

        public void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        [SerializeField]
        public void LoadScene(LoadSceneObject utilityGO)
        {
            //SceneManager.LoadScene(sceneName.ToString());
            SceneManager.LoadScene((int)utilityGO.sceneName);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ToggleEvent(bool toggle)
        {
            if (toggle)
            {
                toggleEventOn?.Invoke();
            }
            else
            {
                toggleEventOff?.Invoke();
            }
        }

        public void ToggleEvent()
        {
            if (!this.toggle)
            {
                toggleEventOn?.Invoke();
            }
            else
            {
                toggleEventOff?.Invoke();
            }
            toggle = !toggle;
        }

    }
}