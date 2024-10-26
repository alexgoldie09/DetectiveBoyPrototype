using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Actions : Actions
{
    [SerializeField] private AudioClip[] audioClips; // Reference to the audio clips to be played
    [SerializeField] private float minPitch = 1f, maxPitch = 1f; // Reference to if you want pitch changed
    [SerializeField] private bool isMusic; // Reference to whether the audio clip is music or not

    private AudioManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = AudioManager.instance;
    }

    public override void Act()
    {
        if(!isMusic)
        {
            manager.PlaySfx(audioClips[Random.Range(0, audioClips.Length)], transform, minPitch, maxPitch);
        }
        else
        {
            manager.PlayMusic(audioClips[0]);
        }
    }
}
