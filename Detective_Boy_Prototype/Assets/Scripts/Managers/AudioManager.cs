using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy the new one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Assign this as the instance
            instance = this;

            // Optionally, ensure this object persists across scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("SFX")]
    [SerializeField] private AudioMixerGroup sfxGroup; // Reference to the sfx group
    [SerializeField] private int audioSourceInstances = 5; // Reference to how many audio sources exist

    private Queue<AudioSource> sfxLib = new Queue<AudioSource>();

    [Header("Music")]
    [SerializeField] private AudioMixerGroup musicGroup; // Reference to the music group

    private AudioSource musicPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for(int i = 0; i < audioSourceInstances; i++)
        {
            sfxLib.Enqueue(AudioSourceInstantiate(sfxGroup, true, "SFXSource " + i.ToString("00")));
        }

        musicPlayer = AudioSourceInstantiate(musicGroup, false, "MusicSource");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private AudioSource AudioSourceInstantiate(AudioMixerGroup _group, bool _sfx, string name = "AudioSource")
    {
        AudioSource audio = new GameObject(name).AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = _group;
        audio.spatialBlend = _sfx ? 1f : 0f;

        audio.loop = !_sfx;

        audio.transform.SetParent(transform);

        return audio;
    }

    public void PlaySfx(AudioClip _clip, Transform _source = null, float _minPitch = 0.8f, float _maxPitch = 1.2f)
    {
        AudioSource audio;

        if(sfxLib.Count == 0)
        {
            audio = AudioSourceInstantiate(sfxGroup, true, "SFXSource " + audioSourceInstances++);
        }
        else
        {
            audio = sfxLib.Dequeue();
        }

        audio.transform.position = _source != null ? _source.position : Vector3.zero;

        audio.clip = _clip;

        // Set a random pitch within the specified range
        audio.pitch = Random.Range(_minPitch, _maxPitch);

        audio.Play();

        audio.transform.SetAsLastSibling(); // for illustrate the dequeue/enqueue process

        // Start coroutine to re-enqueue AudioSource after clip finishes playing
        StartCoroutine(ReEnqueueAfterPlay(audio));
    }

    private IEnumerator ReEnqueueAfterPlay(AudioSource audio)
    {
        // Wait until the audio clip has finished playing
        yield return new WaitForSeconds(audio.clip.length / audio.pitch);

        sfxLib.Enqueue(audio);
    }

    public void PlayMusic(AudioClip _music)
    {
        if(musicPlayer.clip == _music)
        {
            return;
        }

        musicPlayer.clip = _music;
        musicPlayer.Play();
    }

    #region Getters and Setters

    #endregion
}
