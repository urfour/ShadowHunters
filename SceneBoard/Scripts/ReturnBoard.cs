using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Scipt permettant de charger une scène dans Unity
public class ReturnBoard : MonoBehaviour
{
    public void board(string Board)
    {
        SceneManager.LoadScene("Board");
    }
}
