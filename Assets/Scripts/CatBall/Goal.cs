using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util;

namespace CatBall
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private Door door;
        [SerializeField] private UnityEvent onGoal;

        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SceneManagerEx.LoadNextScene();
                return;
            }
            if (!other.gameObject.CompareTag("Ball")) return;
            onGoal.Invoke();
            door.Activate();
            other.gameObject.SetActive(false);
            _audio.Play();
        }
    }
}