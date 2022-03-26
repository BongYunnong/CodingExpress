using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public int bankSize;
    private List<AudioSource> soundClip;
    private void Start()
    {
        soundClip = new List<AudioSource>();
        for(int i = 0; i < bankSize; i++)
        {
            GameObject soundInstance = new GameObject("sound");
            soundInstance.AddComponent<AudioSource>();
            soundInstance.transform.parent = this.transform;
            soundClip.Add(soundInstance.GetComponent<AudioSource>());
        }
    }
    public void PlaySound(AudioClip clip, float volume)
    {
        for(int i = 0; i < soundClip.Count; i++)
        {
            if (!soundClip[i].isPlaying)
            {
                soundClip[i].clip = clip;
                soundClip[i].volume = volume;
                soundClip[i].Play();
                return;
            }
        }
        GameObject soundInstance = new GameObject("sound");
        soundInstance.AddComponent<AudioSource>();
        soundInstance.transform.parent = this.transform;
        soundInstance.GetComponent<AudioSource>().clip = clip;
        soundInstance.GetComponent<AudioSource>().volume = volume;
        soundInstance.GetComponent<AudioSource>().Play();
        soundClip.Add(soundInstance.GetComponent<AudioSource>());
    }

    [SerializeField] AudioClip tap, tick;
    [SerializeField] AudioClip[] strum;
    int randomStrum;

    private void Update()
    {
        if (BPeerM.beatFull)
        {
            PlaySound(tap, 1);
            if (BPeerM.beatCountFull % 2 == 0)
            {
                randomStrum = Random.Range(0, strum.Length);
            }
        }
        if(BPeerM.beatD8 && BPeerM.beatCountFullD8 % 2 == 0)
        {
            PlaySound(tick, 0.1f);
        }
        if(BPeerM.beatD8 && (BPeerM.beatCountFullD8%8==2 || BPeerM.beatCountFullD8 % 8 == 4))
        {
            PlaySound(strum[randomStrum], 1);
        }
    }
}
