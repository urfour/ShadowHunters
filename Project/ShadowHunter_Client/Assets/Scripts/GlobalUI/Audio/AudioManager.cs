using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; } = null;

    private static Dictionary<string, AudioClip> sources = new Dictionary<string, AudioClip>();


    private Dictionary<string, AudioAsyncComponent> auxiliaires = new Dictionary<string, AudioAsyncComponent>();

    private static string source_path = "audio";

    private AudioSource AudioSource;

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
            AudioSource.clip = sources[soundLabel];
            AudioSource.Play();
        }
    }

    public void PlayAsync(string soundLabel, bool isEffect = true, bool stopable = false)
    {
        if (sources.ContainsKey(soundLabel))
        {
            GameObject aux = new GameObject();
            aux.AddComponent<AudioSource>();
            AudioAsyncComponent aac = aux.AddComponent<AudioAsyncComponent>();
            aac.Play(sources[soundLabel], isEffect);
            if (stopable && !auxiliaires.ContainsKey(soundLabel))
            {
                auxiliaires.Add(soundLabel, aac);
            }
        }
    }
}
