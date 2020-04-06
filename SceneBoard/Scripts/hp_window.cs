using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_windows : MonoBehaviour {

	public GameObject hp;
	private int count = 0;


	// Use this for initialization
	void Start () {
		hp.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void view ()
	{
		if (count%2==0){
			hp.SetActive(true);
			count++;
		}
		else {
			hp.SetActive(false);
			count++;
		} 

	}
}
