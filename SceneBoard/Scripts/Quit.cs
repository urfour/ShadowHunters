using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame() //Permet de quitter le jeu
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
