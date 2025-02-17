using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;
    
    [System.Serializable]
    public class AudioClipData
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        [Range(0f, 1f)]
        public float volume = 1f;
        public bool loop = true;  // Default true for music
        [HideInInspector]
        public AudioSource source;
    }

    public AudioMixer mainMixer;
    public List<AudioClipData> musicTracks = new List<AudioClipData>();
    private Dictionary<string, AudioSource> musicDictionary = new Dictionary<string, AudioSource>();
    private AudioSource currentMusic;

    [Header("Sound Effects")]
    public List<AudioClipData> soundEffects = new List<AudioClipData>();
    private Dictionary<string, AudioSource> sfxDictionary = new Dictionary<string, AudioSource>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeMusic();
            InitializeSoundEffects();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSoundEffects()
    {
        foreach (var sfx in soundEffects)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sfx.clip;
            source.outputAudioMixerGroup = sfx.mixerGroup;
            source.volume = sfx.volume;
            source.loop = sfx.loop;
            source.spatialBlend = 1f;  // 3D sound for effects
            source.playOnAwake = false;
            sfx.source = source;
            
            sfxDictionary[sfx.name] = source;
        }
    }

    void InitializeMusic()
    {
        foreach (var music in musicTracks)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = music.clip;
            source.outputAudioMixerGroup = music.mixerGroup;
            source.volume = music.volume;
            source.loop = music.loop;
            source.playOnAwake = false;
            music.source = source;
            
            musicDictionary[music.name] = source;
        }
    }

    public void PlayMusic(string name)
    {
        // Stop current music if any is playing
        if (currentMusic != null && currentMusic.isPlaying)
        {
            currentMusic.Stop();
        }

        // Play new music
        if (musicDictionary.TryGetValue(name, out AudioSource newMusic))
        {
            newMusic.Play();
            currentMusic = newMusic;
        }
    }

    public void StopMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
        }
    }

    public void PauseMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (currentMusic != null && !currentMusic.isPlaying)
        {
            currentMusic.UnPause();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (currentMusic != null)
        {
            currentMusic.volume = Mathf.Clamp01(volume);
        }
    }

     // Basic sound effect play
    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out AudioSource source))
        {
            source.Play();
        }
    }

    // Play at specific position in 3D space
    public void PlaySFXAtPosition(string name, Vector3 position)
    {
        if (sfxDictionary.TryGetValue(name, out AudioSource source))
        {
            AudioSource.PlayClipAtPoint(source.clip, position, source.volume);
        }
    }

     // Play with random pitch variation for more natural sounds
    public void PlaySFXWithRandomPitch(string name, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (sfxDictionary.TryGetValue(name, out AudioSource source))
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.Play();
        }
    }
}