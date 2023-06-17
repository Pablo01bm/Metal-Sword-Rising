using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        { 
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }

        Play("Backgroundmusic");
    }

    public void Play(string name)
    {
        if (name != "KatanaSlice")
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }
        else
        {
            // If the sound name is "KatanaSlice", create a new AudioSource for each sound instance
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound " + name + " not found!");
                return;
            }

            // Create a new AudioSource for this sound instance
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = s.clip;
            newSource.volume = s.volume;
            newSource.pitch = s.pitch;

            newSource.Play();

            // Clean up the AudioSource after the sound finishes playing
            StartCoroutine(DestroyAfterPlaying(newSource, s.clip.length*0.4f));
        }
    }

    public void Stop()
    {
        // Get all the AudioSource components attached to the GameObject
        AudioSource[] audioSources = GetComponents<AudioSource>();

        // Iterate over each AudioSource and stop the playback
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator DestroyAfterPlaying(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Stop and destroy the AudioSource
        source.Stop();
        Destroy(source);
    }
}
