using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEvent : MonoBehaviour
{
    public GameObject discardPile;
    // Start is called before the first frame update
    public IEnumerator UsePower()
    {
        Debug.Log("Carte initialisée");
        yield return new WaitForSeconds(3f);
        transform.SetParent(discardPile.transform, false);
        Debug.Log("la carte elle est utilisée p'tit bg");
    }

}
