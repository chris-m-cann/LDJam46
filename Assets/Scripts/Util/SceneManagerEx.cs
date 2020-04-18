using UnityEngine.SceneManagement;

namespace Util
{
    public static class SceneManagerEx
    {
        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}