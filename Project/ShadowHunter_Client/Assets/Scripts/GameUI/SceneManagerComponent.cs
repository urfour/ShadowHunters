using UnityEngine;
using System.Collections;
using Kernel.Settings;
using Assets.Scripts.MainMenuUI.SearchGame;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using UnityEngine.SceneManagement;
using EventSystem;

public class SceneManagerComponent : MonoBehaviour
{
    //public static SceneManagerComponant Instance { get; set; }

    public static Setting<int> LocalPlayerId = new Setting<int>(0);

    public PlayerBarComponent PlayerBarComponent;

    public static void InitBeforeScene(Room room)
    {
        GameManager.Init(room.MaxNbPlayer.Value, room.RawData.Code);
        for (int i = 0; i < room.MaxNbPlayer.Value; i++)
        {
            PlayerView.GetPlayer(i).Name = room.Players.Value[i];
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
        PlayerBarComponent.Init();
    }

    private void Update()
    {
        EventView.Manager.ExecMainThreaded();
    }
}
