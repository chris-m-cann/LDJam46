using System;
using CatBall;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class RestartOnKeyPress : MonoBehaviour
    {
        [SerializeField] private ControlScheme controls;

        private SceneManagerBehaviour _scenes;

        private void Awake()
        {
            _scenes = GetComponent<SceneManagerBehaviour>();
        }

        private void Update()
        {
            if (controls.restart.WasPressed())
            {
                _scenes.ReloadSceneAfter(0f);
            }
        }
    }
}