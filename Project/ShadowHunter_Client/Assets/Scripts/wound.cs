using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class wound : MonoBehaviour {
  
  public Sprite wound_0;
  public Sprite wound_1, wound_2, wound_3, wound_4, wound_5;
  public Sprite wound_6, wound_7, wound_8, wound_9, wound_10;
  public Sprite wound_11, wound_12, wound_13, wound_14, wound_15;

  void Start () {
    
  }
  
  public void Update () {
    int PlayerWound = 0;
    //PlayerWound = Player.wound;
    PlayerWound = Random.Range(1, 16);
    switch(PlayerWound)
    {
      case 1:
        this.GetComponent<Image>().sprite = wound_1;
        break;
      case 2:
        this.GetComponent<Image>().sprite = wound_2;
        break;
      case 3:
        this.GetComponent<Image>().sprite = wound_3;
        break;
      case 4:
        this.GetComponent<Image>().sprite = wound_4;
        break;
      case 5:
        this.GetComponent<Image>().sprite = wound_5;
        break;
      case 6:
        this.GetComponent<Image>().sprite = wound_6;
        break;
      case 7:
        this.GetComponent<Image>().sprite = wound_7;
        break;
      case 8:
        this.GetComponent<Image>().sprite = wound_8;
        break;
      case 9:
        this.GetComponent<Image>().sprite = wound_9;
        break;
      case 10:
        this.GetComponent<Image>().sprite = wound_10;
        break;
      case 11:
        this.GetComponent<Image>().sprite = wound_11;
        break;
      case 12:
        this.GetComponent<Image>().sprite = wound_12;
        break;
      case 13:
        this.GetComponent<Image>().sprite = wound_13;
        break;
      case 14:
        this.GetComponent<Image>().sprite = wound_14;
        break;
      case 15:
        this.GetComponent<Image>().sprite = wound_15;
        break;
      default:
      	this.GetComponent<Image>().sprite = wound_0;
        break;
    }
  }
}