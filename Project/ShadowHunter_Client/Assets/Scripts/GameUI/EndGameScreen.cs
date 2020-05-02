using UnityEngine;
using System.Collections;
using Assets.Noyau.Players.view;
using UnityEngine.UI;
using EventSystem;
using System.Collections.Generic;
using Assets.Noyau.Manager.view;
using Lang;
using Assets.Scripts.GameUI;

public class EndGameScreen : MonoBehaviour
{
    public RectTransform playersContent;
    public PlayerEndGameBar PlayerEndGameBarPrefab;
    public Text localPlayerWonText;
    public Image characterCardDisplayer;

    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();

    public void Init()
    {
        foreach (Player p in PlayerView.GetPlayers())
        {
            GameObject o = Instantiate(PlayerEndGameBarPrefab.gameObject, playersContent);
            PlayerEndGameBar peg = o.GetComponent<PlayerEndGameBar>();
            peg.Display(p);
            peg.DisplayCard.onClick.AddListener(delegate () { this.DisplayPlayer(p); });
        }

        OnNotification local = (sender) =>
        {
            if (GameManager.LocalPlayer.Value.HasWon.Value)
            {
                localPlayerWonText.text = Language.Translate("game.end.win");
            }
            else
            {
                localPlayerWonText.text = Language.Translate("game.end.lose");
            }
        };

        DisplayPlayer(GameManager.LocalPlayer.Value);

        OnNotification activation = (sender) =>
        {
            if (GameManager.GameEnded.Value)
            {
                gameObject.SetActive(true);
            }
        };

        listeners.Add((GameManager.GameEnded, activation));
        listeners.Add((GameManager.LocalPlayer, local));
        AddListeners();
    }

    public void DisplayPlayer(Player p)
    {
        if (ResourceLoader.CardSprites.ContainsKey(p.Character.characterName))
        {
            characterCardDisplayer.sprite = ResourceLoader.CardSprites[p.Character.characterName];
        }
        else
        {
            characterCardDisplayer.sprite = null;
            Debug.LogWarning("Unknown card label : " + p.Character.characterName);
        }
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
