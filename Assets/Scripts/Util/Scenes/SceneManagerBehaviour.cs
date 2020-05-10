using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private int initialSceneIndex = 2;
        [SerializeField] private bool loadAdditiveSceneOnAwake = true;

        private int _currentScene = 2;

        private void Awake()
        {
            if (!loadAdditiveSceneOnAwake) return;
            
            if (SceneManager.sceneCount == 1)
            {
                _currentScene = initialSceneIndex;
                SceneManager.LoadScene(_currentScene, LoadSceneMode.Additive);
            }
            else
            {
                _currentScene = SceneManager.GetSceneAt(1).buildIndex;
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ReloadSceneAfter(float sceneLoadDelay)
        {
            StartCoroutine(ReplaceScene(_currentScene, sceneLoadDelay));
        }
        public void LoadNextSceneAfter(float sceneLoadDelay)
        {
            StartCoroutine(ReplaceScene(_currentScene + 1, sceneLoadDelay));
        }

        private IEnumerator ReplaceScene(int newScene, float sceneLoadDelay)
        {
            yield return new WaitForSeconds(sceneLoadDelay);

            Debug.Log($"unloading scene {_currentScene}");


            AsyncOperation unload = SceneManager.UnloadSceneAsync(_currentScene);

            while (!unload.isDone)
            {
                yield return 0;
            }


            Debug.Log($"unloaded scene {_currentScene}");

            _currentScene = newScene;

            Debug.Log($"calling load scene {_currentScene}");
            SceneManager.LoadScene(_currentScene, LoadSceneMode.Additive);
        }
    }
}