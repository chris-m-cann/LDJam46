using System;
using CatBall;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CatBall
{
    public class PlatformerSoundController : MonoBehaviour
    {
        public AudioClip[] jump;
        public AudioClip[] land;
        public AudioClip footsteps;
        public AudioClip wallslide;
        public AudioClip[] kick;
        public AudioClip[] death;


        public float minSpeed = .5f;

        public AudioSource source;
        public PlatformerController controller;

        private Rigidbody2D _rigidbody;

        private bool _isPlayingRunning = false;
        private bool _isPlayingWallSliding = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            controller = GetComponent<PlatformerController>();
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Mathf.Abs(_rigidbody.velocity.x) > minSpeed &&
                controller.IsGrounded
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

            // // stopped on the ground
            // if (Mathf.Abs(_rigidbody.velocity.x) < minSpeed && controller.IsGrounded) StopSounds();
            // // in air
            // if (!controller.IsGrounded && !controller.IsOnAWall) StopSounds();

        }

        public void PlayFootsteps()
        {
            if (_isPlayingRunning) return;
            source.clip = footsteps;
            source.Play();
            _isPlayingRunning = true;
        }

        public void PlayWallSlide()
        {
            if (_isPlayingWallSliding) return;
            source.clip = wallslide;
            source.Play();
            _isPlayingWallSliding = true;
        }

        public void StopSounds()
        {
            source.Stop();
            _isPlayingRunning = false;
            _isPlayingWallSliding = false;
        }

        public void PlayJump() => PlaySound(jump);
        public void PlayLand() => PlaySound(land);
        public void PlayKick() => PlaySound(kick);
        public void PlayDeath() => PlaySound(death);

        public void PlaySound(AudioClip[] clips)
        {
            if (clips.Length == 0) return;

            source.Stop();

            var clip = SelectRandom(clips);
            source.PlayOneShot(clip);
        }

        private AudioClip SelectRandom(AudioClip[] clips)
        {

            return clips[Random.Range(0, clips.Length)];
        }
    }
}