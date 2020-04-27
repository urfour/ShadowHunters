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
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {

        for (int i = 0; i < NbPlayers; i++)
        {
            GameObject p = Instantiate(playerViewPrefab.gameObject, this.transform);
            PlayerViewComponent pv = p.GetComponent<PlayerViewComponent>();
            pv.PlayerId = i;
            pv.Init();
        }
    }
}
