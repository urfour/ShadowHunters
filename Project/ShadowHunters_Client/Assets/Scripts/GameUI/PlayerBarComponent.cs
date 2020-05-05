using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarComponent : MonoBehaviour
{

    public PlayerViewComponent playerViewPrefab;

    public int NbPlayers = 4;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        NbPlayers = PlayerView.NbPlayer;
        for (int i = 0; i < PlayerView.NbPlayer; i++)
        {
            GameObject p = Instantiate(playerViewPrefab.gameObject, this.transform);
            PlayerViewComponent pv = p.GetComponent<PlayerViewComponent>();
            pv.PlayerId = i;
            pv.Init(i);
            pv.AddListeners();
        }

        float width = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            width += (transform.GetChild(i) as RectTransform).sizeDelta.x + 50;
        }
        (this.transform as RectTransform).sizeDelta = new Vector2(width, (this.transform as RectTransform).sizeDelta.y);
    }

    /*
    private void OnDestroy()
    {
        playerViewPrefab.GetComponent<PlayerViewComponent>().RemoveListeners();
    }
    */
}
