using UnityEngine;
using System.Collections;
using EventSystem;
using System.Collections.Generic;
using Assets.Noyau.Players.view;
using Assets.Noyau.event_in;
using Assets.Noyau.Manager.view;
using Scripts.event_in;

public class EndTurnButton : MonoBehaviour
{

    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();

    // Use this for initialization
    void Start()
    {
        Init();
        AddListeners();
    }


    public void OnClick()
    {
        if (GameManager.PlayerTurn.Value == PlayerView.GetPlayer(SceneManagerComponent.LocalPlayerId.Value) && GameManager.TurnEndable.Value)
        {
            EventView.Manager.Emit(new EndTurnEvent() { PlayerId = SceneManagerComponent.LocalPlayerId.Value });
        }
    }


    public void Init()
    {
        Player player = PlayerView.GetPlayer(SceneManagerComponent.LocalPlayerId.Value);

        OnNotification canEnd = (sender) =>
        {
            gameObject.SetActive(GameManager.PlayerTurn.Value == player && GameManager.TurnEndable.Value);
        };

        listeners.Add((GameManager.PlayerTurn, canEnd));
        listeners.Add((GameManager.TurnEndable, canEnd));
    }

    public void AddListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            //Language.AddListener(notification);
            observed.AddListener(notification);
            notification(observed);
        }
    }

    public void RemoveListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            //Language.RemoveListener(notification);
            observed.RemoveListener(notification);
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
