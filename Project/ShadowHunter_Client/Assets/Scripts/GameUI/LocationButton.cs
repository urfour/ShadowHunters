using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Noyau.Manager.view;
using EventSystem;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour
{
    //private static Dictionary<Position, Sprite> positionRessourec = new Dictionary<Position, Sprite>();
    private static string locationPath = "Icons/locations";


    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();

    public int position;
    public Position realPosition;
    public Image image;

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
        realPosition = GameManager.Board[position];
        image.sprite = Resources.Load<Sprite>(locationPath + "/" + (int)realPosition);
    }
    
}
