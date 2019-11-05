using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAoe : MonoBehaviour {

	public List<Sprite> aoeSprites;

	public bool playerInAoe;
	SpriteRenderer rend;
	GameObject player;
	
	// Use this for initialization
	void Awake () {
		rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeSprite(int i){
		rend.sprite = aoeSprites[i];
	}

	public void DealDamage(int i){
		if(playerInAoe){
			var dam = player.GetComponent<Damageable>();
			dam.OnDamageTaken(i);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			playerInAoe = true;
			if(player == null)
				player = col.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.tag == "Player")
			playerInAoe = false;
	}
}
