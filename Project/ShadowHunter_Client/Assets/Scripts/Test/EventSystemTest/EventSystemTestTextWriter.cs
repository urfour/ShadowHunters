using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class EventSystemTestTextWriter : MonoBehaviour
{

    public void SetText(string text, Color c)
    {
        gameObject.GetComponent<Text>().text = text;
        gameObject.GetComponent<Text>().color = c;
    }

    public void SetText(string text)
    {
        gameObject.GetComponent<Text>().text = text;
    }
}
