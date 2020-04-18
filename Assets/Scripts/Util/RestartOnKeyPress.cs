using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class RestartOnKeyPress : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetButtonDown("Restart"))
            {
                SceneManagerEx.ReloadScene();
            }
        }
    }
}