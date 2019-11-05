using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryStateSprite : MonoBehaviour {

	// Use this for initialization
	public int maxHitPoints;
	private int currentHitPoints;
	public Sprite Open;
	public Sprite Closed;
	
	[HideInInspector]
	public SpriteRenderer rend;

	public delegate void StateChange(bool state);
	public event StateChange OnStateChange;

	private bool _isOpen;
	public bool isOpen { get { return _isOpen; }
	set{
		if(_isOpen != value)
		{
			_isOpen = value;
			StateChanged(value);	
		}
	}
	}
	
	public virtual void SetBool(bool state)
	{
		isOpen = state;
	}

	public bool GetBool()
	{
		return isOpen;
	}

	public void Switch()
	{
		if(isOpen)
			isOpen = false;
		else
			isOpen = true;
	}

	void StateChanged(bool state)
	{
		if(OnStateChange != null)
			OnStateChange(state);
	}

	public void UpdateSprite(bool state)
	{
		if(isOpen)
			rend.sprite = Open;
		else
			rend.sprite = Closed;
	}

	public void DebugEvent(bool state)
	{
		Debug.Log("Called: " + state.ToString());
	}

	public virtual void Start () {

		rend = gameObject.GetComponent<SpriteRenderer>();
		OnStateChange += UpdateSprite;
		OnStateChange += DebugEvent;
		isOpen = false;
		OnStateChange(isOpen);
		currentHitPoints = maxHitPoints;
	}

	// these methods are for adding to DamageTaken events on the Damageable components
	public void Break(int damage){
		Debug.Log("BREAK CALLED");
		if(currentHitPoints > 0)
		{
			currentHitPoints -= damage;
			if(currentHitPoints <= 0)
			{
				SetBool(true);
			}
		}
	}

	public void SwitchOnDamage(int damage)
	{
		Switch();
	}
}
