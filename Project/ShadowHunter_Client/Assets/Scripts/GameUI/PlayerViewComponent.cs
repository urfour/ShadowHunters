using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using Assets.Scripts.MainMenuUI.Accounts;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using Lang;
using Scripts.event_in;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerViewComponent : MonoBehaviour
{
    public int PlayerId;
    public bool isLocalPlayer = false;

    public Text playerPseudo;
    public Text playerCharacterName;
    public Image playerIcon;
    public Text playerWound;
    public Text position;

    public Button revealButton;

    private Player player;

    private List<(ListenableObject observed, OnNotification notification)> listeners = new List<(ListenableObject observed, OnNotification notification)>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reveal()
    {
        if (!player.Revealed.Value)
        {
            EventView.Manager.Emit(new RevealCardEvent() { PlayerId = player.Id });
            revealButton.interactable = false;
        }
    }

    public void Init(int playerId)
    {
        //playerPseudo.text = GRoom.Instance.JoinedRoom.Players.Value[playerId];
        PlayerId = playerId;
        player = PlayerView.GetPlayer(playerId);
        playerPseudo.text = player.Name;
        isLocalPlayer = player.Name == GAccount.Instance.LoggedAccount.Login;
        revealButton.interactable = isLocalPlayer;

        OnNotification characterName = (sender) =>
        {
            if (player.Revealed.Value || isLocalPlayer)
            {
                playerCharacterName.text = Language.Translate(player.Character.characterName);
            }
            else
            {
                playerCharacterName.text = Language.Translate("character.name.unknown");
            }
        };
        listeners.Add((player.Revealed, characterName));

        OnNotification playerWounds = (sender) =>
        {
            playerWound.text = player.Wound.Value.ToString();
        };
        listeners.Add((player.Wound, playerWounds));

        OnNotification positionNotification = (sender) =>
        {
            position.text = Language.Translate("board.position." + GameManager.Board[player.Position.Value].ToString().ToLower());
        };
        listeners.Add((player.Position, positionNotification));
    }

    public void AddListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            Language.AddListener(notification);
            observed.AddListener(notification);
            notification(observed);
        }
    }

    public void RemoveListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            Language.RemoveListener(notification);
            observed.RemoveListener(notification);
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
