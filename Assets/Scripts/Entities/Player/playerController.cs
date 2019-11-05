using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	public InteractHitbox interactHitbox;
	public Animator animController;
	Transform playerPosition;
	GameObject heldObject;
	BoxCollider2D col;
	Damageable damageable;

	bool inputEnabled, hurt;
	public int swordDamage;
	public float dirX, dirY;
	public float speed, dropOffsetY, hurtTimer, hurtSpeed, hurtInvulnerableDuration;
	private float hurtCounter;
	public Vector2 liftOffset;

	public int maxHealth; 
	public int currentHealth; 
	private GameObject collidedEnemy;

	public delegate void directionChange(float x, float y);

	//event we use to change direction of the interaction hitbox
	public event directionChange OnDirectionChanged;

	// Use this for initialization
	void Start () {
		interactHitbox = gameObject.GetComponentInChildren<InteractHitbox>();
		playerPosition = gameObject.transform;
		animController = gameObject.GetComponent<Animator>();
		col = gameObject.GetComponent<BoxCollider2D>();
		damageable = GetComponent<Damageable>();
		OnDirectionChanged += DebugDir;
		currentHealth = maxHealth;
		inputEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(inputEnabled){
			dirX = Input.GetAxis("Horizontal");
			dirY = Input.GetAxis("Vertical");
			if(dirX != 0f || dirY != 0f)
			{
				animController.SetBool("isWalking", true);
				animController.SetFloat("dirX", dirX);
				animController.SetFloat("dirY", dirY);
				Vector3 movement = new Vector3(dirX, dirY, 0); 
				playerPosition.Translate(movement * speed * Time.deltaTime);
				DirectionChanged(dirX, dirY);
			}
			else
				animController.SetBool("isWalking", false);
		}
		else if(hurt){
			if(hurtCounter > hurtTimer){
				hurtCounter = 0;
				hurt = false;
				inputEnabled = true;
			}
			else{
				hurtCounter += Time.deltaTime;
				Vector3 hurtDirection = collidedEnemy.transform.position - transform.position;
				transform.position += -hurtDirection * hurtSpeed * Time.deltaTime;
			}
		}

		if(currentHealth <= 0){
			GameManager.instance.GameOver();
			gameObject.SetActive(false);
		}

		// if(heldObject == null)
		// 	animController.SetTrigger("dropTrigger");
	}

	void DirectionChanged(float x, float y)
	{
		if(OnDirectionChanged != null)
		{
			OnDirectionChanged(x, y);
		}
	}

	void DebugDir(float x, float y)
	{
		Debug.Log("Float X = " + x.ToString() + " Float Y = " + y.ToString());
	}

	public void LiftObject(GameObject liftable)
	{
		animController.SetTrigger("liftTrigger");
		liftable.GetComponent<SpriteRenderer>().sortingLayerName = "LiftedObject";
		liftable.GetComponent<BoxCollider2D>().enabled = false;
		heldObject = liftable;
		liftable.transform.parent = gameObject.transform;
		liftable.transform.position = gameObject.transform.position + new Vector3(liftOffset.x, liftOffset.y, 0f);
		// play lift animation
	}

	public void DropObject()
	{
		heldObject.transform.parent = null;
		// needs a small offset if the plaeyr is dropping the pot downwards, otherwise drops at feet and pushes player up
		if(animController.GetFloat("dirY") > -0.9f){
			heldObject.transform.position = gameObject.transform.position + new Vector3(interactHitbox.interactHitbox.offset.x, interactHitbox.interactHitbox.offset.y, 0);
		}
		else{
			heldObject.transform.position = gameObject.transform.position + new Vector3(interactHitbox.interactHitbox.offset.x, interactHitbox.interactHitbox.offset.y + dropOffsetY, 0);
			//Debug.Log("WEEEEEEE");
		}
		heldObject.GetComponent<SpriteRenderer>().sortingLayerName = "Interactables";
		heldObject.GetComponent<BoxCollider2D>().enabled = true;
		heldObject = null;
		animController.SetTrigger("dropTrigger");
	}

	public void UpdateCurrentHealth(int damage){
		currentHealth -= damage;
		Debug.Log("Damage is: " + damage + ", currentHealth is: " + currentHealth);
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Enemy"){
			collidedEnemy = collision.gameObject;
			inputEnabled = false;
			hurt = true;
			StartCoroutine("Invulnerable", hurtInvulnerableDuration);
			//coroutine goes here
		}
	}

	IEnumerator Invulnerable(float duration){
		damageable.canTakeDamage = false;
		yield return new WaitForSeconds(duration);
		damageable.canTakeDamage = true;
	}

	
	}
