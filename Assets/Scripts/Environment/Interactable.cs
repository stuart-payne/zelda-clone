using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InteractEvent : UnityEvent<GameObject>
{}

public class Interactable : MonoBehaviour {

	public InteractEvent interactEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Lift()
	{
		
	}

	public void PrintText()
	{

	}
}
