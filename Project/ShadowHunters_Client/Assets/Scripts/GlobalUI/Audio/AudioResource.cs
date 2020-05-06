using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "new_icon", menuName = "GameResource/Audio")]
public class AudioResource : ScriptableObject
{
    public string label;
    public AudioClip clip;
}
