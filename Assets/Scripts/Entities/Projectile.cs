using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile: MonoBehaviour {

	// Use this for initialization
	public GameObject player;
	public Vector3 direction;
	public float speed;
	public int damage;
	bool canDamageBoss;
	
	void OnEnable() {
		canDamageBoss = false;
		StartCoroutine("BossDamageAfterTime", 1.0f);
	}

	public void ReverseDirection() {
		direction *= -1;
		Debug.Log("REVERSE DIRECTION FIRED");
	}

	IEnumerator BossDamageAfterTime(float t) {
		yield return new WaitForSeconds(t);
		canDamageBoss = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			player = col.gameObject;
			var dam = player.GetComponent<Damageable>();
			dam.OnDamageTaken(damage);
			gameObject.SetActive(false);
		}else if(col.gameObject.tag == "boss" && canDamageBoss){
			var dam = col.gameObject.GetComponent<Damageable>();
			dam.OnDamageTaken(damage);
			gameObject.SetActive(false);
		}else if(col.gameObject.tag == "sword"){
			ReverseDirection();
		}
		else if(col.gameObject.tag != "interactCollider" && col.gameObject.tag != "boss") {
			gameObject.SetActive(false);
		}
	}
}
