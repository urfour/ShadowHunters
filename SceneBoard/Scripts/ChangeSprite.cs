using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
 public class ChangeSprite : MonoBehaviour {
 
     public Sprite neutral, hunter, shadow;
     int guess;
 
     public void change_sprite ()
     {
         guess++;  
         if(guess%3==0)
            this.GetComponent<Image>().sprite = neutral;
         if(guess%3==1)
            this.GetComponent<Image>().sprite = hunter;
         if(guess%3==2)
            this.GetComponent<Image>().sprite = shadow;
    }
 }
