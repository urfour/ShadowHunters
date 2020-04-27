using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dice_6_animation : MonoBehaviour {

	public Sprite dice_6_1, dice_6_2, dice_6_3, dice_6_4, dice_6_5, dice_6_6;
	void Start () {
		
	}

	public void ChangeDiceFace () {
    int dice = Random.Range(1, 7); // creates a number between 1 and 6
	  switch (dice)
      {
          case 1:
              this.GetComponent<Image>().sprite = dice_6_1;
              break;
          case 2:
			        this.GetComponent<Image>().sprite = dice_6_2;
              break;
          case 3:
			        this.GetComponent<Image>().sprite = dice_6_3;
              break;
          case 4:
			        this.GetComponent<Image>().sprite = dice_6_4;
              break;
          case 5:
			        this.GetComponent<Image>().sprite = dice_6_5;
              break;
          case 6:
			        this.GetComponent<Image>().sprite = dice_6_6;
              break;    
          default:
              break;
      }		 
	}
}

