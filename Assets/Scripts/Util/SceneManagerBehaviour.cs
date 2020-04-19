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
    }
}