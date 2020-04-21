using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneManagerBehaviour : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ReloadSceneAfter(float sceneLoadDelay)
        {
            StartCoroutine(ReloadScene(sceneLoadDelay));
        }
        public void LoadNextSceneAfter(float sceneLoadDelay)
        {
            StartCoroutine(LoadNextScene(sceneLoadDelay));
        }


        private IEnumerator LoadNextScene(float sceneLoadDelay)
        {
            yield return new WaitForSeconds(sceneLoadDelay);
            SceneManagerEx.LoadNextScene();
        }
        private IEnumerator ReloadScene(float sceneLoadDelay)
        {
            yield return new WaitForSeconds(sceneLoadDelay);
            SceneManagerEx.ReloadScene();
        }
    }
}