using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageTakenEvent : UnityEvent<int>{}
public class Damageable : MonoBehaviour {
	
	// this bool will allow to add invulnerablity if wanted
	public bool canTakeDamage;
	// this event will allow for the entity components to have different behaviours based on when they take damage rather than it only doing health damage
	public DamageTakenEvent DamageTaken;
	public void OnDamageTaken(int i)
	{
		//invoke dmgtaken event only if entity can be damaged and as long that the event has subscribers;
		if(canTakeDamage && DamageTaken != null)
			DamageTaken.Invoke(i);
	}

	void Start()
	{
		DamageTaken.AddListener(DebugEvent);
	}

	public void DebugEvent(int i){
		Debug.Log("DAMAGE TAKEN EVENT FIRED");
	}
}
