using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using EventSystem;

public class WaitHandler : MonoBehaviour
{
    static WaitHandler instance;
    public static WaitHandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                instance = go.AddComponent<WaitHandler>();
            }
            return instance;
        }
    }
    public void BotChoice(float time, PlayerEvent e)
    {
        StartCoroutine(SimulateBotChoice(time, e));
    }

    IEnumerator SimulateBotChoice(float time, PlayerEvent e)
    {
        Debug.Log("Simulation d'attente du bot");
        yield return new WaitForSeconds(time);
        EventView.Manager.Emit(e);
        Debug.Log("Fin de l'attente");
    }

}
