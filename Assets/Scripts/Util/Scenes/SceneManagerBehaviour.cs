using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util.UI;
using Random = UnityEngine.Random;

namespace Util
{
    public class SceneManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private int initialSceneIndex = 2;
        [SerializeField] private bool loadAdditiveSceneOnAwake = true;
        [SerializeField] private UnityEvent onSceneUnloadBegun;
        [SerializeField] private UnityEvent onSceneUnload;
        [SerializeField] private UnityEvent onSceneLoaded;
        [SerializeField] private Image image;
        [SerializeField] private int transIdx = -1;
        [SerializeField] private SceneTransition[] transitions;

        private int _currentScene = 2;
        private bool _reloadOngoing = false;
        private SceneTransition _activeTransition;

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

            _activeTransition = ChoseTransition();

            image.enabled = true;
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
            if (newScene >= SceneManager.sceneCountInBuildSettings) newScene = _currentScene;

            var isReload = newScene == _currentScene;

            // break out early if already loading
            if (_reloadOngoing) yield break;

            _reloadOngoing = true;
            yield return new WaitForSeconds(sceneLoadDelay);


            onSceneUnloadBegun.Invoke();
            if (!isReload && transitions.Length != 0)
            {
                _activeTransition = ChoseTransition();

                image.enabled = true;
                yield return StartCoroutine(TransitionOut(_activeTransition));
            }

            AsyncOperation unload = SceneManager.UnloadSceneAsync(_currentScene);

            while (!unload.isDone)
            {
                yield return 0;
            }

            onSceneUnload.Invoke();
            
            _currentScene = newScene;

            // just in case the scene was reloaded during a reduced timescale
            Time.timeScale = 1f;

            SceneManager.LoadScene(_currentScene, LoadSceneMode.Additive);
            yield return StartCoroutine(SetActiveScene(_currentScene, isReload));

            _reloadOngoing = false;
        }

        private IEnumerator SetActiveScene(int idx, bool isReload = false)
        {
            yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentScene));

            if (!isReload && transitions.Length != 0)
            {
                yield return StartCoroutine(TransitionIn(_activeTransition));

                image.enabled = false;
            }

            onSceneLoaded.Invoke();
        }

        private SceneTransition ChoseTransition()
        {
            if (transIdx >= 0)
            {
                return transitions[transIdx];
            }
            return transitions[Random.Range(0, transitions.Length)];
        }


        private IEnumerator TransitionIn(SceneTransition transition)
        {
            var initial = -.1f - transition.smoothing;
            var final = 1.1f + transition.smoothing;
            yield return StartCoroutine(Transition(transition, initial, final));
        }

        private IEnumerator TransitionOut(SceneTransition transition)
        {
            var initial = 1.1f + transition.smoothing;
            var final = -.1f - transition.smoothing;
            yield return StartCoroutine(Transition(transition, initial, final));
        }
        private IEnumerator Transition(SceneTransition transition, float initial, float final)
        {
            Time.timeScale = 0.2f;

            var start = Time.unscaledTime;
            var end = start + transition.duration;

            image.material.SetTexture("_Texture", transition.texture);
            image.material.SetFloat("_Smoothing", transition.smoothing);
            image.material.SetFloat("_Rotation", transition.rotation);

            image.material.SetFloat("_Cutoff", initial);



            while (Time.unscaledTime < end)
            {
                var cutoff = Tween.Lerp(initial, final, (Time.unscaledTime - start) / transition.duration);

                image.material.SetFloat("_Cutoff", cutoff);
                yield return null;
            }


            image.material.SetFloat("_Cutoff", final);

            Time.timeScale = 1f;
        }
    }
}