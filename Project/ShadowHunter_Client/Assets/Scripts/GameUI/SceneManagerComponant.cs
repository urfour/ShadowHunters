using UnityEngine;
using System.Collections;
using Kernel.Settings;
using Assets.Scripts.MainMenuUI.SearchGame;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;

public class SceneManagerComponant : MonoBehaviour
{
    //public static SceneManagerComponant Instance { get; set; }

    public static Setting<int> LocalPlayerId = new Setting<int>(0);

    public PlayerBarComponent PlayerBarComponent;

    public static void InitBeforeScene(Room room)
    {
        GameManager.Init(room.MaxNbPlayer.Value);
        for (int i = 0; i < room.MaxNbPlayer.Value; i++)
        {
            PlayerView.GetPlayer(i).Name = room.Players.Value[i];
        }
        //PlayerBarComponent.Init();
    }

    private void Start()
    {
        PlayerBarComponent.Init();
    }
}
