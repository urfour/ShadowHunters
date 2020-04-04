using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Scipt permettant de charger une scène dans Unity
public class OpenSettings : MonoBehaviour
{
    public void settings(string Settings)
    {
        //Application.LoadLevel(scenename);
        SceneManager.LoadScene("Settings");
    }
}

