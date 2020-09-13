using System;
using UnityEngine;

namespace Util
{
    public class ChangeMusicOnStart : MonoBehaviour
    {
        [SerializeField] private AudioClipEx track;
        [SerializeField] private bool fadeIn;
        [SerializeField] private bool fadeOut;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeOutTime;


        private MusicManager _musicManager;
        private void Awake()
        {
            _musicManager = FindObjectOfType<MusicManager>();
        }

        private void Start()
        {
            if (track.clip != null)
            {
                ReplaceTrack();
            }
            else
            {
                StopTrack();
            }
        }

        private void ReplaceTrack()
        {
            if (fadeIn && fadeOut)
            {
                _musicManager.FadeInOut(track, fadeInTime, fadeOutTime);
            } else if (fadeIn)
            {
                _musicManager.FadeInSound(track, fadeInTime);
            } else if (fadeOut)
            {
                _musicManager.FadeInOut(track, 0f, fadeInTime);
            }
            else
            {
                _musicManager.PopInSound(track);
            }
        }

        private void StopTrack()
        {
            if (fadeOut)
            {
                _musicManager.FadeOutSound(fadeOutTime);
            }
            else
            {
                _musicManager.StopSounds();
            }
        }
    }
}