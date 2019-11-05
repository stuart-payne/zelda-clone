using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class basicEnemyController : MonoBehaviour {

	bool waitingForInput, aware;
	public Animator animController;
	float dirX, dirY;

	Vector3 chaseDirection;
	float chaseMagnitude;
	public float maxAggroDistance;

	Transform target;
	string[] layersForRaycast = { "Wall Layer"};
	string[] doorLayer = {"Door"};

	public UnityEvent OnDeath;

	public float speed;
	public int damage;
	// Use this for initialization
	void Start () {
		waitingForInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(aware){
			chaseDirection = target.position - transform.position;
			chaseMagnitude = chaseDirection.magnitude;
			chaseDirection.Normalize();
			transform.position += chaseDirection * speed * Time.deltaTime;
			animController.SetFloat("directionX", chaseDirection.x);
			animController.SetFloat("directionY", chaseDirection.y);
			animController.SetBool("isWalking", true);

			if(chaseMagnitude > maxAggroDistance)
				aware = false;
		}
		else{
		// start coroutine of either walk or idle when there is no coroutine active
			if(waitingForInput){
				int rand = Random.Range(0, 2);
				if(rand == 1)
					StartCoroutine("Idle");
				else
					StartCoroutine("Walking");
			}

			// move the enemy if it is in a walking animation
			bool isWalking = animController.GetBool("isWalking");
			if(isWalking){
				Vector3 dir = new Vector3(dirX, dirY, 0);
				
				// shoot raycast to check for walls and reverse direction if there is a wall hit. Stops enemy from running into wall
				RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.2f, LayerMask.GetMask(layersForRaycast));
				if(hit.collider != null){
					dir *= -1;
					dirX *= -1;
					dirY *= -1;
					animController.SetFloat("directionX", dirX);
					animController.SetFloat("directionY", dirY);
					Debug.Log("fired raycast and hit");
				}
				transform.Translate(dir * speed * Time.deltaTime);
			}
		}
	}

	IEnumerator Idle(){
		Debug.Log("Idle Coroutine fired");
		waitingForInput = false;
		yield return new WaitForSeconds(2f);
		waitingForInput = true;
	}

	IEnumerator Walking(){
		Debug.Log("Walking Coroutine fired");
		waitingForInput = false;
		
		int xDirection = 0;
		int yDirection = Random.Range(0, 2);

		// determine if negative or positive;
		if(yDirection == 1 && Random.Range(0, 2) == 1)
			yDirection *= -1;
		
		if(yDirection == 0){
			xDirection = 1;
			if(Random.Range(0, 2) == 1)
				xDirection *= -1;
		}

		//set variables on animator
		animController.SetFloat("directionX", xDirection);
		animController.SetFloat("directionY", yDirection);
		dirX = xDirection;
		dirY = yDirection;
		animController.SetBool("isWalking", true);

		//leave animator in state for 1 second
		yield return new WaitForSeconds(1f);

		//stop movement of enemy and reset stae back to waiting for input
		animController.SetBool("isWalking", false);
		waitingForInput = true;

	}

	public void TakeDamage(int i){
		//DEATH ANIMATION NEEDED
		Death();
		gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			target = col.transform;
			aware = true;
			//Debug.Log("I AM AWARE");
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		Debug.Log("ENEMY COLLISION FIRED");
		if(col.gameObject.tag == "Player"){
			Debug.Log("COLLISION WITH PLAYER");
			Damageable dam = col.collider.GetComponent<Damageable>();
			dam.OnDamageTaken(damage);
		}
	}

	public void Death(){
		if(OnDeath != null)
			OnDeath.Invoke();
	}
}
