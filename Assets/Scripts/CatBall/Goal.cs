using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatBall
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private Door door;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Activating Door");
            door.Activate();
        }
    }
}