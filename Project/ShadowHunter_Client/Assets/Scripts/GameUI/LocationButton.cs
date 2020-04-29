using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Noyau.Manager.view;
using EventSystem;
using UnityEngine.UI;
using Assets.Noyau.Players.view;
using Scripts.event_in;

public class LocationButton : MonoBehaviour
{
    //private static Dictionary<Position, Sprite> positionRessourec = new Dictionary<Position, Sprite>();
    private static string locationPath = "Images/locations";


    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();

    public int position;
    public Position realPosition;
    public Image image;
    private Button button;

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

    // Use this for initialization
    void Start()
    {
        Init();
    }
    


    public void Init()
    {
        realPosition = GameManager.Board[position];
        image.sprite = Resources.Load<Sprite>(locationPath + "/" + (int)realPosition);
        button = gameObject.GetComponent<Button>();
        OnNotification available = (sender) =>
        {
            button.interactable = SceneManagerComponent.boardAvailibility[position].Value;
        };

        listeners.Add((SceneManagerComponent.boardAvailibility[position], available));
        AddListeners();
    }

    public void OnClick()
    {
        if (GameManager.PlayerTurn.Value == PlayerView.GetPlayer(SceneManagerComponent.LocalPlayerId.Value) && SceneManagerComponent.boardAvailibility[position].Value)
        {
            EventView.Manager.Emit(new MoveOn() { PlayerId = SceneManagerComponent.LocalPlayerId.Value, Location=position });
        }
    }
}
