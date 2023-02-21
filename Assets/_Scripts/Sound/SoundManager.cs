using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private GameObject audioSourcePf;

    private List<GameObject> audioSources = new List<GameObject>();
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public int PlayDependantClipOnTransform(AudioClip clip, Transform t)
    {
        if (clip == null) return -1;
        GameObject audioSourceGo = Instantiate(audioSourcePf, t);
        AudioSource audioSource = audioSourceGo.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audioSourceGo, clip.length);
        audioSources.Add(audioSourceGo);
        return audioSources.Count-1;
    }

    public void StopDependantClip(int index)
    {
        if (index < 0) return;
        if (index > audioSources.Count - 1) return;

        try
        {
            audioSources[index].GetComponent<AudioSource>().Stop();
        }
        catch
        {
            Debug.Log("No clip found in index: " + index);
        }
    }

    public void PlayClipOnTransform(AudioClip clip, Transform t)
    {
        if (clip == null) return;
        GameObject audioSourceGo = Instantiate(audioSourcePf, t);
        AudioSource audioSource = audioSourceGo.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(audioSourceGo, clip.length);
        audioSources.Add(audioSourceGo);
    }
}
