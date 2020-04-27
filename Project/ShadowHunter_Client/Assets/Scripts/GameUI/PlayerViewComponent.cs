using Assets.Noyau.Players.view;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerViewComponent : MonoBehaviour
{
    public int PlayerId;

    public Text playerPseudo;
    public Text playerCharacterName;
    public Image playerIcon;
    public Text playerWound;

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

    public void Init(int playerId)
    {
        //playerPseudo.text = GRoom.Instance.JoinedRoom.Players.Value[playerId];
        PlayerId = playerId;
        player = PlayerView.GetPlayer(playerId);
        OnNotification characterName = (sender) =>
        {
            if (player.Revealed.Value)
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
    }

    public void AddListeners()
    {
        foreach (var (observed, notification) in listeners)
        {
            observed.AddListener(notification);
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
