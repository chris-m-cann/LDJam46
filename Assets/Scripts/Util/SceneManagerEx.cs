using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public static class SceneManagerEx
    {
        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LoadNextScene()
        {
            Debug.Log($"current scene idx = {SceneManager.GetActiveScene().buildIndex}, out of {SceneManager.sceneCountInBuildSettings}");
            var nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % (SceneManager.sceneCountInBuildSettings);
            Debug.Log($"Loading Scene {nextScene}");
            SceneManager.LoadScene(nextScene);
        }
    }
}