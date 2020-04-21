using System;
using UnityEngine;
using Util;

namespace CatBall
{
    public class CatSounds : MonoBehaviour
    {
        [SerializeField] private LayerMask ground;
        [SerializeField] private AudioClip[] hitSounds;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (ground.Contains(other.gameObject.layer))
            {
                PlaySound(hitSounds);
            }
        }

        public void PlaySound(AudioClip[] clips)
        {
            if (clips.Length == 0) return;

            _source.Stop();

            var clip = SelectRandom(clips);
            _source.PlayOneShot(clip);
        }

        private AudioClip SelectRandom(AudioClip[] clips)
        {

            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
    }
}