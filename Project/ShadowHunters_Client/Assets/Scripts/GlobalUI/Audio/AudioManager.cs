﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using Kernel.Settings;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; } = null;

    private static Dictionary<string, AudioClip> sources = new Dictionary<string, AudioClip>();


    private Dictionary<string, AudioAsyncComponent> auxiliaires = new Dictionary<string, AudioAsyncComponent>();

    private static string source_path = "audio";

    private AudioSource AudioSource;
    private OnNotification volumeListener;
    private ListenableObject listened;

    public ListenableObject OnPlayedClipEnd = new ListenableObject();

    public AudioResource MainMenuMusique;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource = gameObject.GetComponent<AudioSource>();
            AudioResource[] cards = Resources.LoadAll<AudioResource>(source_path);
            foreach (AudioResource a in cards)
            {
                if (!sources.ContainsKey(a.label))
                {
                    sources.Add(a.label, a.clip);
                }
                else
                {
                    Debug.LogWarning("audio label already exists : " + a.label);
                }
            }
            Play(MainMenuMusique.clip);
            DontDestroyOnLoad(gameObject);

            volumeListener = (sender) =>
            {
                AudioSource.volume = (float)SettingManager.Settings.UI_MusicVolume.Value;
            };
            listened = SettingManager.Settings.UI_MusicVolume;

            listened.AddListener(volumeListener);
            volumeListener(listened);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
        }
    }

    public void Play(string soundLabel)
    {
        if (sources.ContainsKey(soundLabel))
        {
            AudioSource.Stop();
            AudioSource.clip = sources[soundLabel];
            AudioSource.Play();
        }
    }

    public void PlayAsync(string soundLabel, bool isEffect = true, bool stopable = false)
    {
        if (sources.ContainsKey(soundLabel))
        {
            GameObject aux = new GameObject();
            aux.transform.SetParent(transform);
            aux.AddComponent<AudioSource>();
            AudioAsyncComponent aac = aux.AddComponent<AudioAsyncComponent>();
            aac.Play(sources[soundLabel], isEffect);
            if (stopable && !auxiliaires.ContainsKey(soundLabel))
            {
                auxiliaires.Add(soundLabel, aac);
            }
        }
    }

    private void OnDestroy()
    {
        if (listened != null)
            listened.RemoveListener(volumeListener);
    }
}
