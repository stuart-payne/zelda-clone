using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHitbox : MonoBehaviour {

	public AudioClip damageSound;
	public AudioClip missSound;
	public bool isTriggerActive;
	public playerController player;
	public float offset = -0.05f;
	public BoxCollider2D interactHitbox, playerHitbox;

	float playerHitboxX, playerHitboxY;

	// Use this for initialization
	void Start () {
		interactHitbox = GetComponent<BoxCollider2D>();
		playerHitbox = GetComponentInParent<BoxCollider2D>();
		player.OnDirectionChanged += ChangeHitboxLocation;
		playerHitboxX = playerHitbox.size.x;
		playerHitboxY = playerHitbox.size.y;
	}
	
	// Update is called once per frame
	public virtual void OnTriggerStay2D(Collider2D collider)
	{
		if(isTriggerActive)	{
			if(Input.GetKeyDown("f"))
			{
				Debug.Log("F fired");
				Interactable inter = collider.gameObject.GetComponent<Interactable>();
				if(inter != null)
					inter.interactEvent.Invoke(inter.gameObject);
			}

			if(Input.GetKeyDown("space")){
				if(!collider.isTrigger){
					Damageable damageable = collider.gameObject.GetComponent<Damageable>();
					SoundManager.instance.PlaySound(damageSound);
					if(damageable != null){
						damageable.OnDamageTaken(player.swordDamage);
						Debug.Log("SWORD EVENT SENT");
				}
				if(collider.gameObject.tag == "StraightOrb") {
					collider.gameObject.GetComponent<Projectile>().ReverseDirection();
				}
			}
			}

		}
	}

	public void ChangeHitboxLocation(float x, float y)
	{
		if(x != 0 && y == 0){
			interactHitbox.offset = new Vector2(playerHitboxX * x, 0f + offset); 
			interactHitbox.size = new Vector2(playerHitboxX, playerHitboxY);
		}
		else if(x == 0 && y != 0){
			interactHitbox.offset = new Vector2(0f, playerHitboxY * y + offset);
			interactHitbox.size = new Vector2(playerHitboxY, playerHitboxX);
		}
		Debug.Log("Changehitbox Success");
	}
}
