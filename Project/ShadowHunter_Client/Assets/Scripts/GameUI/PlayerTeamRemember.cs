using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerTeamRemember : MonoBehaviour
{
    public int remember = (int)CharacterTeam.None;
    public Image displayer;

    private void Start()
    {
        Refresh();
    }

    public void Switch()
    {
        remember = (remember + 1) % 4;
        Refresh();
    }

    public void Refresh()
    {
        switch ((CharacterTeam)remember)
        {
            case CharacterTeam.None:
                displayer.color = Color.white;
                break;
            case CharacterTeam.Hunter:
                displayer.color = SceneManagerComponent.Instance.HunterColor;
                break;
            case CharacterTeam.Shadow:
                displayer.color = SceneManagerComponent.Instance.ShadowColor;
                break;
            case CharacterTeam.Neutral:
                displayer.color = SceneManagerComponent.Instance.NeutralColor;
                break;
        }
    }
}
