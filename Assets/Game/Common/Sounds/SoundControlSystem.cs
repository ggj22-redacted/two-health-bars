using UnityEngine;

public class SoundControlSystem : MonoBehaviour
{
    [System.Serializable]
    public struct SoundEffect
    {
        public AudioClip clip;

        public float volume;

        public float pitch;
    }

    [SerializeField]
    public SoundEffect[] soundClips;

    public AudioSource playSound1;

    public AudioSource playSound2;

    private AudioClip oldestClip;

    private void SetAudioSettings (AudioClip clip, AudioSource source)
    {
        for (int x = 0; x < soundClips.Length; x++) {
            if (soundClips[x].clip != clip)
                continue;

            source.clip = soundClips[x].clip;
            source.volume = soundClips[x].volume;
            source.pitch = soundClips[x].pitch;
        }
    }

    private int CheckAnyAudioSourcePlaying ()
    {
        int numberPlaying = 0;

        if (playSound1.isPlaying) {
            numberPlaying++;
        }

        if (playSound2.isPlaying) {
            numberPlaying++;
        }

        return numberPlaying;
    }

    private AudioSource FetchAudioSource (bool playingValue)
    {
        if (playSound1.isPlaying == playingValue) {
            return playSound1;
        }

        if (playSound2.isPlaying == playingValue) {
            return playSound2;
        }

        return null;
    }

    private bool CheckClip (AudioClip clip)
    {
        if (playSound1.clip == clip && playSound1.isPlaying) {
            return true;
        }

        if (playSound2.clip == clip && playSound2.isPlaying) {
            return true;
        }

        return false;
    }

    private void SoundPlay (AudioClip clip)
    {
        switch (CheckAnyAudioSourcePlaying()) {
            case 0:
                SetAudioSettings(clip, FetchAudioSource(false));
                FetchAudioSource(false).Play();
                break;
            case 1:
                if (!CheckClip(clip)) {
                    SetAudioSettings(clip, FetchAudioSource(false));
                    FetchAudioSource(false).Play();
                }
                break;
            case 2:
                if (!CheckClip(clip)) {
                    if (playSound1.clip == oldestClip) {
                        playSound1.Stop();
                        oldestClip = playSound2.clip;
                    }

                    if (playSound2.clip == oldestClip) {
                        playSound2.Stop();
                        oldestClip = playSound1.clip;
                    }

                    AudioSource source = FetchAudioSource(false);
                    SetAudioSettings(clip, source);
                    FetchAudioSource(false).Play();
                }
                break;
        }
    }

    private void Update ()
    {
        if (CheckAnyAudioSourcePlaying() == 1) {
            oldestClip = FetchAudioSource(true).clip;
        }
    }
}