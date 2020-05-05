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
        GameObject layout = new GameObject("poslayout", typeof(RectTransform));
        layout.transform.SetParent(transform);
        RectTransform lrect = layout.transform as RectTransform;


        if (playerCubeOnRight)
        {
            lrect.anchorMin = new Vector2(1, 0);
            lrect.anchorMax = new Vector2(1, 1);
        }
        else
        {
            lrect.anchorMin = new Vector2(0, 0);
            lrect.anchorMax = new Vector2(0, 1);
        }
        lrect.anchoredPosition3D = new Vector3(0, 0, 0);
        lrect.sizeDelta = new Vector2(32, 0);

        VerticalLayoutGroup vlg = layout.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 5;
        vlg.childForceExpandHeight = true;
        vlg.childControlHeight = false;
        vlg.childControlWidth = false;

        for (int i = 0; i < SceneManagerComponent.Instance.playerColors.Count && i < PlayerView.NbPlayer; i++)
        {
            GameObject o = Instantiate(SceneManagerComponent.Instance.playerPositionDisplayer.gameObject, layout.transform);
            o.GetComponent<Image>().color = SceneManagerComponent.Instance.playerColors[i];
            RectTransform r = o.transform as RectTransform;

            r.sizeDelta = new Vector2(32, 32);

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
