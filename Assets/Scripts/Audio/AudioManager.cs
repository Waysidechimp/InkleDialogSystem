using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    public void InsertSoundEffect(AudioClip sfxClip)
    {
        sfx.clip = sfxClip;
    }

    public void PlaySoundEffect()
    {
        sfx.Play();
    }

    public void StopSoundEffect()
    {
        sfx.Stop();
    }

    public void SFXRandomizePitch()
    {
        sfx.pitch = Random.Range(0.5f, 1.5f);
    }

    public void SFXNormalizePitch()
    {
        sfx.pitch = 1;
    }
}
