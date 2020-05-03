using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EventSystem;
using System.Collections.Generic;
using Lang;

public class PlayerEndGameBar : MonoBehaviour
{
    public Text PlayerName;
    public Text PlayerRoll;
    public Text WinOrNot;
    public Button DisplayCard;

    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();


    public void Display(Player p)
    {
        PlayerName.text = p.Name;
        PlayerRoll.text = Language.Translate("character.team." + p.Character.team.ToString().ToLower());

        OnNotification winornot = (sender) =>
        {
            if (p.HasWon.Value)
            {
                WinOrNot.text = Language.Translate("game.end.win");
            }
            else
            {
                WinOrNot.text = Language.Translate("game.end.lose");
            }
        };
        listeners.Add((p.HasWon, winornot));
        AddListeners();
    }

    public void AddListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            observed.AddListener(notification);
            notification(observed);
        }
    }

    public void RemoveListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            observed.RemoveListener(notification);
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
