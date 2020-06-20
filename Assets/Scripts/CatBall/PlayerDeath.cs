using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace CatBall
{
    public class PlayerDeath : MonoBehaviour, IHurtable
    {
        [SerializeField] private GameObject corpse;
        [SerializeField] private float sceneLoadDelay = 1f;
        [SerializeField] private UnityEvent onDeath;

        private SceneManagerBehaviour _sceneManager;

        private void Awake()
        {
            _sceneManager = FindObjectOfType<SceneManagerBehaviour>();
        }

        public void Hurt()
        {
            onDeath.Invoke();
            Instantiate(corpse, transform.position, Quaternion.identity);
            _sceneManager.ReloadSceneAfter(sceneLoadDelay);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }


    }
}