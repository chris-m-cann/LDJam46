using System;
using System.Collections;
using CatBall;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace CatBall
{
    public class PlatformerSoundController : MonoBehaviour
    {
        public AudioClipEx[] jump;
        public AudioClipEx[] land;
        public AudioClipEx footsteps;
        public AudioClipEx wallslide;
        public AudioClipEx[] kick;
        public AudioClipEx[] death;


        public float minSpeed = .5f;

        public AudioSource source;
        public PlatformerController controller;

        private Rigidbody2D _rigidbody;

        private bool _isPlayingRunning = false;
        private bool _isPlayingWallSliding = false;
        private bool _playingOneshot;
        private bool _landedPlayed = true;

        private void OnValidate()
        {
            if (_isPlayingRunning)
            {
                source.SetClipDetails(footsteps);
            }

            if (_isPlayingWallSliding)
            {
                source.SetClipDetails(wallslide);
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            controller = GetComponent<PlatformerController>();
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Mathf.Abs(_rigidbody.velocity.x) > minSpeed &&
                controller.IsGrounded && _landedPlayed
                )
            {
                PlayFootsteps();
            }
            else if (_rigidbody.velocity.y < 0 &&
                !controller.IsGrounded &&
                controller.IsOnAWall)
            {
                PlayWallSlide();
            }
            else
            {
                StopSounds();
            }
        }

        public void PlayFootsteps()
        {
            if (_isPlayingRunning || _playingOneshot) return;
            source.SetClipDetails(footsteps);
            source.loop = true;
            source.Play();
            _isPlayingRunning = true;
        }

        public void PlayWallSlide()
        {
            if (_isPlayingWallSliding || _playingOneshot) return;
            source.SetClipDetails(wallslide);
            source.loop = true;
            source.Play();
            _isPlayingWallSliding = true;
        }

        public void StopSounds()
        {
            if (!_playingOneshot)
                source.Stop();
            _isPlayingRunning = false;
            _isPlayingWallSliding = false;
        }

        public void PlayJump()
        {
            _landedPlayed = false;
            Debug.Log("PlayingJump");
            PlaySound(jump);
        }

        public void PlayLand()
        {
            Debug.Log("PlayingLand");
            PlaySound(land);
            _landedPlayed = true;
        }

        public void PlayKick() => PlaySound(kick);
        public void PlayDeath() => PlaySound(death);

        public void PlaySound(AudioClipEx[] clips)
        {
            if (clips.Length == 0) return;

            _playingOneshot = true;

            var clip = SelectRandom(clips);
            StopAllCoroutines();
            StartCoroutine(CoPlayClip(clip));
        }

        private IEnumerator CoPlayClip(AudioClipEx clip)
        {

            source.loop = false;
            source.outputAudioMixerGroup = clip.mixer ?? source.outputAudioMixerGroup;

            // using play one shot so can play multiple at the same time. such as kick and land
            source.PlayOneShot(clip.clip, clip.volume);
            yield return new WaitForSeconds(clip.clip.length);
            _playingOneshot = false;
        }

        private AudioClipEx SelectRandom(AudioClipEx[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }
    }
}