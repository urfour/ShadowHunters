using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class dice_4_animation : MonoBehaviour {

	public Sprite dice_4_1, dice_4_2, dice_4_3, dice_4_4;
	void Start () {
	}
      

	public void ChangeDiceFace () {
    int dice = UnityEngine.Random.Range(1, 5); // creates a number between 1 and 4
    switch (dice)
      {
          case 1:
              this.GetComponent<Image>().sprite = dice_4_1;
              break;
          case 2:
			        this.GetComponent<Image>().sprite = dice_4_2;
              break;
          case 3:
			        this.GetComponent<Image>().sprite = dice_4_3;
              break;
          case 4:
			        this.GetComponent<Image>().sprite = dice_4_4;
              break;
          default:
              break;
      }	
    }
}
