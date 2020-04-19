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
            var nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % (SceneManager.sceneCount);
            Debug.Log($"Loading Scene {nextScene}");
            SceneManager.LoadScene(nextScene);
        }
    }
}