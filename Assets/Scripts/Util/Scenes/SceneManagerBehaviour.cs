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
        private bool _reloadOngoing = false;

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

            StartCoroutine(SetActiveScene(_currentScene));
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
            // break out early if already loading
            if (_reloadOngoing) yield break;

            _reloadOngoing = true;
            yield return new WaitForSeconds(sceneLoadDelay);

            AsyncOperation unload = SceneManager.UnloadSceneAsync(_currentScene);

            while (!unload.isDone)
            {
                yield return 0;
            }


            _currentScene = newScene;

            SceneManager.LoadScene(_currentScene, LoadSceneMode.Additive);
            yield return StartCoroutine(SetActiveScene(_currentScene));

            _reloadOngoing = false;
        }

        private IEnumerator SetActiveScene(int idx)
        {
            yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentScene));

        }
    }
}