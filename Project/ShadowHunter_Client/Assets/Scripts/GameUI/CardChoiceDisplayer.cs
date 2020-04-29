using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Noyau.Cards.model;
using Lang;

public class CardChoiceDisplayer : MonoBehaviour
{
    public Text target;
    public Text description;
    public Button button;
    
    public void Display(Player p, CardEffect cardEffect)
    {
        target.text = p.Name;
        description.text = Language.Translate(cardEffect.description);
    }
}
