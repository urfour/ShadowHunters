using UnityEngine;
using System.Collections;
using Kernel.Settings;
using Assets.Scripts.MainMenuUI.SearchGame;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using UnityEngine.SceneManagement;
using EventSystem;
using Scripts;
using Scripts.event_out;
using Assets.Scripts.MainMenuUI.Accounts;
using Scripts.event_in;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.view;
using Assets.Noyau.Cards.model;
using Assets.src.Kernel.event_in;
using ServerInterface.RoomEvents;

public class SceneManagerComponent : MonoBehaviour, IListener<PlayerEvent>
{
    //public static SceneManagerComponant Instance { get; set; }

    public static Setting<int> LocalPlayerId = new Setting<int>(0);
    public static Setting<bool>[] boardAvailibility;

    public static SceneManagerComponent Instance { get; set; }

    public PlayerBarComponent PlayerBarComponent;
    public EndGameScreen EndGameScreen;

    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();

    public Button visionPickButton;
    public Button LightnessPickButton;
    public Button DarknessPickButton;

    public Color ShadowColor;
    public Color HunterColor;
    public Color NeutralColor;

    public CardDisplayer cardDisplayer;

    public static void InitBeforeScene(Room room)
    {
        boardAvailibility = new Setting<bool>[6];
        for (int i = 0; i < boardAvailibility.Length; i++)
        {
            boardAvailibility[i] = new Setting<bool>(false);
        }

        for (int i = 0; i < room.MaxNbPlayer.Value; i++)
        {
            if (room.Players.Value[i] == GAccount.Instance.LoggedAccount.Login)
            {
                LocalPlayerId.Value = i;
                break;
            }
        }

        GameManager.Init(room.MaxNbPlayer.Value, room.RawData.Code, LocalPlayerId.Value);

        for (int i = 0; i < room.MaxNbPlayer.Value; i++)
        {
            PlayerView.GetPlayer(i).Name = room.Players.Value[i];
        }
        if (LocalPlayerId.Value == 0)
        {
            EventView.Manager.Emit(new EndTurnEvent());
        }
        //PlayerBarComponent.Init();
    }


    public static void LoadScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }

    private void Start()
    {
        Instance = this;
        EventView.Manager.AddListener(this, true);
        PlayerBarComponent.Init();

        OnNotification visionPick = (sender) =>
        {
            visionPickButton.gameObject.SetActive(GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value && GameManager.PickVisionDeck.Value);
        };

        listeners.Add((GameManager.PlayerTurn, visionPick));
        listeners.Add((GameManager.PickVisionDeck, visionPick));

        OnNotification lightnessPick = (sender) =>
        {
            LightnessPickButton.gameObject.SetActive(GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value && GameManager.PickLightnessDeck.Value);
        };

        listeners.Add((GameManager.PlayerTurn, lightnessPick));
        listeners.Add((GameManager.PickLightnessDeck, lightnessPick));

        OnNotification darknessPick = (sender) =>
        {
            DarknessPickButton.gameObject.SetActive(GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value && GameManager.PickDarknessDeck.Value);
        };

        listeners.Add((GameManager.PlayerTurn, darknessPick));
        listeners.Add((GameManager.PickDarknessDeck, darknessPick));

        AddListeners();

        EndGameScreen.Init();
    }


    public void VisionPick()
    {
        if (GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value && GameManager.PickVisionDeck.Value)
        {
            GameManager.PickDarknessDeck.Value = false;
            GameManager.PickLightnessDeck.Value = false;
            GameManager.PickVisionDeck.Value = false;
            EventView.Manager.Emit(new DrawCardEvent() { PlayerId = GameManager.LocalPlayer.Value.Id, SelectedCardType = CardType.Vision });
        }
    }
    public void LightnessPick()
    {
        if (GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value && GameManager.PickLightnessDeck.Value)
        {
            GameManager.PickDarknessDeck.Value = false;
            GameManager.PickLightnessDeck.Value = false;
            GameManager.PickVisionDeck.Value = false;
            EventView.Manager.Emit(new DrawCardEvent() { PlayerId = GameManager.LocalPlayer.Value.Id, SelectedCardType = CardType.Light });
        }
    }
    public void DarknessPick()
    {
        if (GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value && GameManager.PickDarknessDeck.Value)
        {
            GameManager.PickDarknessDeck.Value = false;
            GameManager.PickLightnessDeck.Value = false;
            GameManager.PickVisionDeck.Value = false;
            EventView.Manager.Emit(new DrawCardEvent() { PlayerId = GameManager.LocalPlayer.Value.Id, SelectedCardType = CardType.Darkness });
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    
    

    public void OnEvent(PlayerEvent e, string[] tags = null)
    {
        if (e is SelectMovement sm)
        {
            for (int i = 0; i < boardAvailibility.Length; i++)
            {
                boardAvailibility[i].Value = false;
            }
            foreach (int pos in sm.LocationAvailable)
            {
                boardAvailibility[pos].Value = true;
            }
        }
        else if (e is MoveOn mo)
        {
            for (int i = 0; i < boardAvailibility.Length; i++)
            {
                boardAvailibility[i].Value = false;
            }
        }
        else if (e is SelectUsableCardPickedEvent selectUsable)
        {
            cardDisplayer.DisplayUsableCard((UsableCard)CardView.GetCard(selectUsable.CardId), PlayerView.GetPlayer(selectUsable.PlayerId));
        }
        else if (e is DrawEquipmentCardEvent equipmentCardEvent)
        {
            cardDisplayer.Display(CardView.GetCard(equipmentCardEvent.CardId), PlayerView.GetPlayer(equipmentCardEvent.PlayerId));
        }
        else if (e is ShowCharacterCardEvent showCard)
        {
            cardDisplayer.Display(showCard.CardLabel, PlayerView.GetPlayer(showCard.PlayerId));
        }
    }

    private void Update()
    {
        EventView.Manager.ExecMainThreaded();
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


    public void ReturnToMenu()
    {
        //EventView.Manager.Emit(new PlayerLeaveEvent(GameManager.LocalPlayer.Value.Id));
        GameManager.Clean();
        SceneManager.LoadScene(0);
    }
}
