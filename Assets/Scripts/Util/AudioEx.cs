using UnityEngine;

namespace Util
{
    public static class AudioEx
    {

        public static void SetClipDetails(this AudioSource source, AudioClipEx clipEx)
        {
            source.clip = clipEx.clip ?? source.clip;
            source.volume = clipEx.volume;
            source.outputAudioMixerGroup = clipEx.mixer ?? source.outputAudioMixerGroup;
        }
    }
}