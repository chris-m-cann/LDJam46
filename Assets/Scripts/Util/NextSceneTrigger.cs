using System;
using UnityEngine;

namespace Util
{
    public class NextSceneTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            SceneManagerEx.LoadNextScene();
        }
    }
}