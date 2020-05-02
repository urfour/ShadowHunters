using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using Assets.Scripts.GameUI;
using Assets.Scripts.MainMenuUI.Accounts;
using Assets.Scripts.MainMenuUI.SearchGame;
using Assets.src.Kernel.event_in;
using EventSystem;
using Lang;
using Scripts.event_in;
using Scripts.event_out;
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
    public Button attackButton;
    public RectTransform turnIndicator;

    public Image characterIcon;
    public Text characterTotalHealth;

    public Button revealButton;
    public Button powerButton;
    public Text infoDisplayer;

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

    public void DisplayCard()
    {
        //playerIcon.gameObject.SetActive(true);
        SceneManagerComponent.Instance.cardDisplayer.DisplayPlayer(this.player);
    }
    /*
    public void HideCard()
    {
        playerIcon.gameObject.SetActive(false);
    }
    */

    public void Attack()
    {
        if (GameManager.LocalPlayer.Value.getTargetablePlayers().Contains(player))
        {
            EventView.Manager.Emit(new AttackPlayerEvent() { PlayerId=GameManager.LocalPlayer.Value.Id, PlayerAttackedId=player.Id });
        }
    }

    public void Power()
    {
        if (GameManager.LocalPlayer.Value == player && player.CanUsePower.Value)
        {
            //player.Character.power.power(player);
            EventView.Manager.Emit(new PowerUseEvent(player.Id));
        }
    }

    public void Init(int playerId)
    {
        //playerPseudo.text = GRoom.Instance.JoinedRoom.Players.Value[playerId];
        PlayerId = playerId;
        player = PlayerView.GetPlayer(playerId);
        playerPseudo.text = player.Name;
        isLocalPlayer = player.Name == GAccount.Instance.LoggedAccount.Login;
        revealButton.gameObject.SetActive(isLocalPlayer);
        attackButton.gameObject.SetActive(!isLocalPlayer);

        if (isLocalPlayer)
        {
            OnNotification revealNotification = (sender) =>
            {
                revealButton.gameObject.SetActive(!player.Revealed.Value);
            };
            listeners.Add((player.Revealed, revealNotification));
        }

        switch (player.Character.team)
        {
            case CharacterTeam.Hunter:
                characterIcon.color = SceneManagerComponent.Instance.HunterColor;
                break;
            case CharacterTeam.Shadow:
                characterIcon.color = SceneManagerComponent.Instance.ShadowColor;
                break;
            case CharacterTeam.Neutral:
                characterIcon.color = SceneManagerComponent.Instance.NeutralColor;
                break;
            default:
                Logger.Warning("Invalid character type : " + player.Character.team);
                break;
        }


        OnNotification characterName = (sender) =>
        {
            if (player.Revealed.Value || isLocalPlayer)
            {
                playerCharacterName.text = Language.Translate(player.Character.characterName);
                playerIcon.sprite = ResourceLoader.CharacterSprites[player.Character.characterName];
                characterIcon.gameObject.SetActive(true);
            }
            else
            {
                playerCharacterName.text = Language.Translate("character.name.unknown");
                playerIcon.sprite = ResourceLoader.CharacterSprites["character.name.unknown"];
                characterIcon.gameObject.SetActive(false);
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
            if (GameManager.Board.ContainsKey(player.Position.Value))
            {
                position.text = Language.Translate("board.position." + GameManager.Board[player.Position.Value].ToString().ToLower());
            }
            else
            {
                position.text = Language.Translate("board.position." + Position.None.ToString().ToLower());
            }
        };
        listeners.Add((player.Position, positionNotification));

        OnNotification turnIndicatorN = (sender) =>
        {
                turnIndicator.gameObject.SetActive(GameManager.PlayerTurn.Value == player);
        };
        listeners.Add((GameManager.PlayerTurn, turnIndicatorN));


        if (!isLocalPlayer)
        {
            OnNotification attackAvailable = (sender) =>
            {
                attackButton.interactable = GameManager.PlayerTurn.Value == GameManager.LocalPlayer.Value 
                                            && GameManager.AttackAvailable.Value
                                            && GameManager.LocalPlayer.Value.getTargetablePlayers().Contains(player);
            };

            listeners.Add((player.Dead, attackAvailable));
            listeners.Add((player.Position, attackAvailable));
            listeners.Add((GameManager.LocalPlayer.Value.Dead, attackAvailable));
            listeners.Add((GameManager.LocalPlayer.Value.Position, attackAvailable));
            listeners.Add((GameManager.LocalPlayer.Value.HasRevolver, attackAvailable));
            listeners.Add((GameManager.PlayerTurn, attackAvailable));
            listeners.Add((GameManager.AttackAvailable, attackAvailable));
        }


        OnNotification powerAvailable = (sender) =>
        {
            powerButton.gameObject.SetActive(GameManager.LocalPlayer.Value == player && player.CanUsePower.Value);
        };

        listeners.Add((player.CanUsePower, powerAvailable));


        OnNotification notifIsDead = (sender) =>
        {
            if (player.Dead.Value)
            {
                infoDisplayer.text = Language.Translate("character.info.dead");
            }
        };

        listeners.Add((player.Dead, notifIsDead));
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
