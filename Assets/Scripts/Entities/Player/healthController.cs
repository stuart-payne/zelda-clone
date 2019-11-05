using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthController : MonoBehaviour {

	public playerController player;

	public List<Image> hearts;

	public Sprite empty, half, full;
	
	// Use this for initialization
	public void UpdateHealth(int dmg){
		StartCoroutine("LateUpdateHealth");
	}

	void Start(){
		//ensure that UpdateHealth grabs the player.currentHealth after playerController has initialised
		StartCoroutine("LateStart");
	}

	IEnumerator LateStart(){
		yield return new WaitForEndOfFrame();
		UpdateHealth(0);
	}

	IEnumerator LateUpdateHealth(){
		yield return new WaitForEndOfFrame();
		//Debug.Log("UPDATE HEALTH FIRED");
		int currentHealth = player.currentHealth;
		//Debug.Log("Current Health is: " + currentHealth);
		foreach(var heart in hearts){
			if(currentHealth == 0)
				heart.sprite = empty;
			else if(currentHealth == 1){
				heart.sprite = half;
				currentHealth -= 1;
			}
			else{
				heart.sprite = full;
				currentHealth -= 2;
			}
		}
	}
}
