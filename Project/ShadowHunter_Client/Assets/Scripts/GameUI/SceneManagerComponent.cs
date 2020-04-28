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

public class SceneManagerComponent : MonoBehaviour, IListener<PlayerEvent>
{
    //public static SceneManagerComponant Instance { get; set; }

    public static Setting<int> LocalPlayerId = new Setting<int>(0);
    public static Setting<bool>[] boardAvailibility;

    public PlayerBarComponent PlayerBarComponent;

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
        EventView.Manager.AddListener(this, true);
        PlayerBarComponent.Init();
    }

    private void Update()
    {
        EventView.Manager.ExecMainThreaded();
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
    }

}
