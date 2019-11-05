using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderOrder : MonoBehaviour {

	// Use this for initialization
	SpriteRenderer rend;

	void Start()
	{
		rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//render those with higher Y positions in front of those with lower to fake a 3d feeling
		rend.sortingOrder =  (Int32)(-transform.position.y * 100);
	}
}
