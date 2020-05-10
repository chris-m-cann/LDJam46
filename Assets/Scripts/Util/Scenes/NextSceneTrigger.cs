using System;
using UnityEngine;

namespace Util
{
    public class NextSceneTrigger : MonoBehaviour
    {
        private SceneManagerBehaviour scenes;


        private void Awake()
        {
            scenes = FindObjectOfType<SceneManagerBehaviour>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) scenes.LoadNextSceneAfter(0f);
        }
    }
}