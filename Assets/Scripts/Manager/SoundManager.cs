using UnityEngine;
using System.Collections.Generic;
using System;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audioClip;
        [Range(0f, 1f)]
        public float volume = 1;
    }

    public List<Sound> sounds = new List<Sound>();
    private Dictionary<string, AudioClip> dictSound = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;
    private AudioSource audioLoop;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioLoop = GetComponent<AudioSource>();
        audioLoop.loop = true;
        
        dictSound = new Dictionary<string, AudioClip>();
        foreach (Sound tmp in sounds)
        {
            dictSound.Add(tmp.name, tmp.audioClip);
        }
    }
    public void PlaySound(String name)
    {
        if (dictSound.ContainsKey(name))
        {
            audioSource.PlayOneShot(dictSound[name]);

        }
        else
        {
            Debug.LogWarning("This sound " + name + "not found!");
        }
    }
    public void PlayLoop(String name)
    {
        if (dictSound.ContainsKey(name))
        {
            audioLoop.clip = dictSound[name];
            audioLoop.Play();
        }
    }
    public void StopLoop()
    {
        audioLoop.Stop();
    }
}
