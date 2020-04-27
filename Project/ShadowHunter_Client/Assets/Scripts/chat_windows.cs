using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chat_windows : MonoBehaviour {

	public GameObject chat_box;
	private int count = 0;


	// Use this for initialization
	void Start () {
		chat_box.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void view ()
	{
		if (count%2==0){
			chat_box.SetActive(true);
			count++;
		}
		else {
			chat_box.SetActive(false);
			count++;
		} 

	}
}
