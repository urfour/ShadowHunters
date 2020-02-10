using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour
{
    public GameObject playerCards;

    // Start is called before the first frame update
    void Start()
    {
        playerCards.transform.SetParent(transform);
        Debug.Log("A new player has connected.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
