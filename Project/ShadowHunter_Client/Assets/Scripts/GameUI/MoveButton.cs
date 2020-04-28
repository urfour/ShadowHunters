using UnityEngine;
using System.Collections;
using EventSystem;
using System.Collections.Generic;
using Lang;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using Assets.Noyau.event_in;

public class MoveButton : MonoBehaviour
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
        if (GameManager.PlayerTurn.Value == PlayerView.GetPlayer(SceneManagerComponent.LocalPlayerId.Value) && GameManager.MovementAvailable.Value)
        {
            EventView.Manager.Emit(new AskMovement() { PlayerId = SceneManagerComponent.LocalPlayerId.Value });
        }
    }
    

    public void Init()
    {
        Player player = PlayerView.GetPlayer(SceneManagerComponent.LocalPlayerId.Value);

        OnNotification canMove = (sender) =>
        {
            gameObject.SetActive(GameManager.PlayerTurn.Value == player && GameManager.MovementAvailable.Value);
        };

        listeners.Add((GameManager.PlayerTurn, canMove));
        listeners.Add((GameManager.MovementAvailable, canMove));
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
