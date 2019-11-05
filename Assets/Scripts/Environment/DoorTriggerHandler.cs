using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerHandler : MonoBehaviour {

	// Use this for initialization
	public BinaryStateSprite door;
	public BinaryStateSprite switchObject;
	
	void Start()
	{
		switchObject = GetComponent<BinaryStateSprite>();
	}

	private void OnTriggerStay2D(Collider2D col)
	{
		if(!col.isTrigger){
			switchObject.SetBool(true);
			if(door != null)
				door.SetBool(true);
		}
	}
/* 	private void OnTriggerEnter2D(Collider2D col)
	{
		door.Switch();
		switchObject.Switch();
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		door.Switch();
		switchObject.Switch();
	} */

	private void OnTriggerExit2D(Collider2D col)
	{
		switchObject.SetBool(false);
		if(door != null)
			door.SetBool(false);
	}
}
