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

    public bool playerCubeOnRight = false;

    private List<GameObject> playerDisplayers = new List<GameObject>();

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
        for (int i = 0; i < SceneManagerComponent.Instance.playerColors.Count && i < PlayerView.NbPlayer; i++)
        {
            GameObject o = Instantiate(SceneManagerComponent.Instance.playerPositionDisplayer.gameObject, transform);
            o.GetComponent<Image>().color = SceneManagerComponent.Instance.playerColors[i];
            RectTransform r = o.transform as RectTransform;
            Rect rect = r.rect;
            if (playerCubeOnRight)
            {
                r.anchorMax = new Vector2(1, 1 - 0.125f * i);
                r.anchorMin = new Vector2(1, 1 - 0.125f * i);
            }
            else
            {
                r.anchorMax = new Vector2(0, 1 - 0.125f * i);
                r.anchorMin = new Vector2(0, 1 - 0.125f * i);
            }
            r.sizeDelta = new Vector2(32, 32);
            r.anchoredPosition3D = new Vector3(0, 0, 0);

            Player p = PlayerView.GetPlayer(i);
            listeners.Add((p.Position,
                (sender) =>
                {
                    o.SetActive(p.Position.Value == position);
                }
            ));
        }

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
