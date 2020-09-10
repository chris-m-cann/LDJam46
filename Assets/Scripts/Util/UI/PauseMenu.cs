using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util.UI
{
    public class PauseMenu : MonoBehaviour
    {


        // dirty rotten static replace with scriptable object events in future projects
        public static bool IS_GAME_PAUSED = false;

        [SerializeField] private UnityEvent onPause;
        [SerializeField] private UnityEvent onResume;

        private float _timeScaleBefore = 1f;

        private void Awake()
        {
            IS_GAME_PAUSED = false;
        }

        private void OnDestroy()
        {
            // just in case we kill the scene when we are paused
            Time.timeScale = 1f;
        }

        public void PauseResume()
        {
            if (IS_GAME_PAUSED)
            {
                IS_GAME_PAUSED = false;
                Time.timeScale = _timeScaleBefore;
                onResume.Invoke();
            }
            else
            {
                IS_GAME_PAUSED = true;
                _timeScaleBefore = Time.timeScale;
                Time.timeScale = 0f;
                onPause.Invoke();
            }
        }


    }
}
