using System.Collections;
using UnityEngine;

namespace Util
{
    public class MusicManager : MonoBehaviour
    {

        private AudioSource _source;
        void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);

            _source = GetComponent<AudioSource>();
        }

        public void StopSounds()
        {
            _source.Stop();
        }

        public void PopInSound(AudioClipEx clip)
        {
            if (clip.clip == _source.clip) return;

            _source.Stop();
            _source.SetClipDetails(clip);
            _source.Play();
        }

        public void FadeInSound(AudioClipEx clip, float duration)
        {
            if (clip.clip == _source.clip) return;

            StartCoroutine(CoFadeInSound(clip, duration));
        }

        private IEnumerator CoFadeInSound(AudioClipEx clip, float duration)
        {
            _source.SetClipDetails(clip);
            _source.volume = 0f;
            _source.Play();

            var start = Time.time;
            var end = start + duration;
            while (Time.time < end)
            {
                _source.volume = Tween.Lerp(0, clip.volume, (Time.time - start) / duration);
                yield return null;
            }

            _source.volume = clip.volume;
        }

        public void FadeOutSound(float duration)
        {
            StartCoroutine(CoFadeOutSound(duration));
        }

        private IEnumerator CoFadeOutSound(float duration)
        {
            float startVolume = _source.volume;

            var start = Time.time;
            var end = start + duration;
            while (Time.time < end)
            {
                _source.volume = Tween.Lerp(startVolume, 0f, (Time.time - start) / duration);
                yield return null;
            }

            _source.volume = 0f;
            _source.Stop();
        }

        public void FadeInOut(AudioClipEx clip, float durationIn, float durationOut)
        {
            if (clip.clip == _source.clip) return;
            StartCoroutine(CoFadeInOut(clip, durationIn, durationOut));
        }

        private IEnumerator CoFadeInOut(AudioClipEx clip, float durationIn, float durationOut)
        {
            yield return StartCoroutine(CoFadeOutSound(durationOut));
            yield return StartCoroutine(CoFadeInSound(clip, durationIn));
        }
    }
}