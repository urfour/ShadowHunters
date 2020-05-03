using UnityEngine;
using System.Collections;
using EventSystem;
using Kernel.Settings;

public class AudioAsyncComponent : MonoBehaviour
{
    AudioSource source;

    OnNotification volumeListener;
    ListenableObject listened;

    public void Play(AudioClip clip, bool isEffect)
    {
        source = gameObject.GetComponent<AudioSource>();
        if (isEffect)
        {
            volumeListener = (sender) =>
            {
                source.volume = (float)SettingManager.Settings.UI_EffectVolume.Value;
            };
            listened = SettingManager.Settings.UI_EffectVolume;
        }
        else
        {
            volumeListener = (sender) =>
            {
                source.volume = (float)SettingManager.Settings.UI_MusicVolume.Value;
            };
            listened = SettingManager.Settings.UI_MusicVolume;
        }

        listened.AddListener(volumeListener);
        volumeListener(listened);

        source.clip = clip;
        source.Play();
    }

    private void OnDestroy()
    {
        listened.RemoveListener(volumeListener);
    }
}
