using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Noyau.Cards.model;
using Lang;

public class CharacterEquipmentDisplayer : MonoBehaviour
{
    public Text label;
    public Button button;
    public EquipmentCard ec;


    public void Display(EquipmentCard ec)
    {
        label.text = Language.Translate(ec.cardLabel);
        this.ec = ec;
    }
}
