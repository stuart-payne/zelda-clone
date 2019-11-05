using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProjectile : MonoBehaviour {

	public GameObject player;
	public int damage;
	public float timeout, speed;
	bool canDamageBoss;
	// Use this for initialization
	void OnEnable(){
		StartCoroutine("DestroyOnTimeout");
		StartCoroutine("BossDamageAfterTime", 2.0f);
		canDamageBoss = false;
	}
	
	// Update is called once per frame
	void Update () {
		var direction = player.transform.position - transform.position;
		direction.Normalize();
		transform.position += direction * speed * Time.deltaTime;
	}

	IEnumerator BossDamageAfterTime(float t) {
		yield return new WaitForSeconds(t);
		canDamageBoss = true;
	}

	IEnumerator DestroyOnTimeout(){
		yield return new WaitForSeconds(timeout);
		gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			player = col.gameObject;
			var dam = player.GetComponent<Damageable>();
			dam.OnDamageTaken(damage);
			gameObject.SetActive(false);
		} else if (col.gameObject.tag == "boss" && canDamageBoss) {
			var dam = col.gameObject.GetComponent<Damageable>();
			dam.OnDamageTaken(damage);
			gameObject.SetActive(false);
		} else if (col.gameObject.tag != "interactCollider" && col.gameObject.tag != "boss"){
			gameObject.SetActive(false);
		}
	}
}
