using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisionCard", menuName = "Card/VisionCard", order = 3)]
[System.Serializable]
public class VisionCard : Card
{
    public VisionEffect visionEffect;
}
