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
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource audioLoop;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        EnsureAudioSources();
        audioLoop.loop = true;

        dictSound.Clear();
        foreach (Sound tmp in sounds)
        {
            if (tmp == null || string.IsNullOrEmpty(tmp.name) || tmp.audioClip == null)
                continue;

            if (!dictSound.ContainsKey(tmp.name))
                dictSound.Add(tmp.name, tmp.audioClip);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void EnsureAudioSources()
    {
        if (audioSource != null && audioLoop != null)
            return;

        AudioSource[] existingSources = GetComponents<AudioSource>();
        if (existingSources.Length >= 2)
        {
            if (audioSource == null)
                audioSource = existingSources[0];
            if (audioLoop == null)
                audioLoop = existingSources[1];
            return;
        }

        if (existingSources.Length == 1)
        {
            if (audioSource == null)
                audioSource = existingSources[0];
            if (audioLoop == null)
                audioLoop = gameObject.AddComponent<AudioSource>();
            return;
        }

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        if (audioLoop == null)
            audioLoop = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(String name)
    {
        if (audioSource == null)
            return;

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
        if (audioLoop == null)
            return;

        if (dictSound.ContainsKey(name))
        {
            AudioClip clip = dictSound[name];
            if (audioLoop.clip != clip)
                audioLoop.clip = clip;

            if (!audioLoop.isPlaying)
                audioLoop.Play();
        }
    }
    public void StopLoop()
    {
        if (audioLoop != null)
            audioLoop.Stop();
    }
}
