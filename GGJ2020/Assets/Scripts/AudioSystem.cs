using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AudioSystem : MonoBehaviour
{
    public List<AudioClip> clips;
    public List<AudioClip> SFXClips;
    private bool isPlayingOne = false;

    public AudioSource source1;
    public AudioSource source2;
    public AudioSource SFXSource;

    public static AudioSystem instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void PlayMusic(int clip)
    {
        if (isPlayingOne)
        {
            source1.clip = clips[clip];
            source1.Play();
            DOTween.To(() => source1.volume, x => source1.volume = x, 1, 1.0f);
            DOTween.To(() => source2.volume, x => source2.volume = x, 0, 1.0f);

        }
        else
        {
            source2.clip = clips[clip];
            source2.Play();
            DOTween.To(() => source2.volume, x => source2.volume = x, 1, 1.0f);
            DOTween.To(() => source1.volume, x => source1.volume = x, 0, 1.0f);
        }

        isPlayingOne = !isPlayingOne;

    }

    public void PlaySFX(int index)
    {
        SFXSource.PlayOneShot(SFXClips[index]);
    }
    public void PlaySFX(int index, float delay)
    {
        StartCoroutine(playeDelay(index, delay));
    }

    IEnumerator playeDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySFX(index);
    }

}
