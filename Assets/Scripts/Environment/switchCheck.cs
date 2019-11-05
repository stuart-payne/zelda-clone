using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCheck : MonoBehaviour{

	public BinaryStateSprite door;
	public BinaryStateSprite switch1;
	public BinaryStateSprite switch2;

	void Update(){
		if(switch1.GetBool() == true && switch2.GetBool() == true)
			door.SetBool(true);
		else
			door.SetBool(false);

	}
}

