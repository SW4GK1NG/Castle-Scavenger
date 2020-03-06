using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] Sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in Sounds) {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
            s.Source.loop = s.loop;
        }
    }

    public void Play (string name) {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        s.Source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, sounds => sounds.Name == name);
        if (s == null)
            return;
        s.Source.Stop();
    }

    public void Pitch(string name, float pitch)
    {
        Sound s = Array.Find(Sounds, sounds => sounds.Name == name);
        if (s == null)
            return;
        s.Source.pitch = pitch;
    }
}
