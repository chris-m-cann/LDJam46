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
        private SceneManagerBehaviour _scenes;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
            _scenes = FindObjectOfType<SceneManagerBehaviour>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _scenes.LoadNextSceneAfter(0f);
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