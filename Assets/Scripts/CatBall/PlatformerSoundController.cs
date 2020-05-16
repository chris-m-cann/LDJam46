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

        private void Awake()
        {
            controller = GetComponent<PlatformerController>();
            source = GetComponent<AudioSource>();
        }

        // private void Update()
        // {
        //     if (source.isPlaying)
        //     {
        //         if (controller.IsGrounded && Math.Abs(rb.velocity.x) < float.Epsilon)
        //         {
        //             source.Pause();
        //             return;
        //         }
        //
        //         if (controller.IsOnAWall && )
        //     }
        //
        //
        //     // stop when we stop
        //     if (source.isPlaying && Math.Abs(rb.velocity.x) < float.Epsilon && !controller.IsOnAWall)
        //     {
        //         source.Pause();
        //         return;
        //     }
        //
        //     // is we arnt playing it and we are grounded and
        //     if (!source.isPlaying && controller.IsGrounded && Math.Abs(rb.velocity.x) > minSpeed)
        //     {
        //         source.clip = footsteps;
        //         source.Play();
        //     }
        //
        //     if (!source.isPlaying && controller.IsOnAWall && Math.Abs(rb.velocity.y) > minSpeed)
        //     {
        //         source.clip = wallslide;
        //         source.Play();
        //     }
        // }

        public void PlayFootsteps()
        {
            if (source.clip == footsteps && source.isPlaying) return;
            source.clip = footsteps;
            source.Play();
        }

        public void PlayWallSlide()
        {
            if (source.clip == wallslide && source.isPlaying) return;
            source.clip = wallslide;
            source.Play();
        }

        public void StopSounds() => source.Stop();
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

        // public void ChangeState(IPlayerSoundState state)
        // {
        //     throw new NotImplementedException();
        // }
    }
}

// interface IPlayerSoundState
// {
//     void Update(PlatformerSoundController controller);
// }
//
// class GroundedState : IPlayerSoundState
// {
//     public void Update(PlatformerSoundController controller)
//     {
//         var stopped = Math.Abs(controller.rb.velocity.x) < controller.minSpeed;
//
//         if (controller.source.isPlaying && stopped)
//         {
//             controller.source.Pause();
//         }
//         else if (!controller.source.isPlaying && !stopped)
//         {
//             controller.source.clip = controller.footsteps;
//             controller.source.Play();
//         }
//     }
// }
//
// class InAir : IPlayerSoundState
// {
//     public void Update(PlatformerSoundController controller)
//     {
//         if (controller.source.isPlaying) controller.source.Pause();
//
//         if (controller.controller.IsOnAWall) controller.ChangeState(new OnAWall());
//     }
// }
//
// internal class OnAWall : IPlayerSoundState
// {
//     public void Update(PlatformerSoundController controller)
//     {
//         if (!controller.source.isPlaying && Math.Abs(controller.rb.velocity.y) > controller.minSpeed)
//         {
//             controller.source.clip = controller.wallslide;
//             controller.source.Play();
//         }
//     }
// }