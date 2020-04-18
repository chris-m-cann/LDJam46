using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatBall
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % (SceneManager.sceneCount);
            Debug.Log($"Loading Scene {nextScene}");
            SceneManager.LoadScene(nextScene);
        }
    }
}