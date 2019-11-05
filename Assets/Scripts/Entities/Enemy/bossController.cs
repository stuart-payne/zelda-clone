using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class bossController : MonoBehaviour {

	Sprite[] bossSprites;
	public int spriteMax, spritesPerDirection;
	public string spriteFileName, path;
	private SpriteRenderer rend;
	public int castStage = 0;
	public int  healthPoints, maxHealthPoints;

	public Transform playerTransform;
	public Color followProjColor;

	public float floatStrength, floatSpeed;

	public UnityEvent SpriteChanged;
	public Collider2D bossCol;

	public GameObject attachedOrb;

	public List<Sprite> castingOrbSprites;
	public bossAoe aoe;
	public int aoeDamage;

	private float originalY;
	public Slider healthBar;

	Vector3 up = new Vector3(0, 1, 0);
	Vector3 right = new Vector3(1, 0, 0);

	bool active = false;
	public bool Active {
		get {
			return active;
		}
		set {
			if(value != active) {
				active = value;
				OnActiveChanged();
			}
		}
	}
	//using the DoorFacing enum so dont have to rewrite the same thing
	private DoorFacing directionFacing;

	//using properties and an event so that sprite can be updated when values change rather than using Update(), saves a lot of calls
	public DoorFacing DirectionFacing{
		get{
			return directionFacing;
		}
		set{
			if(value != directionFacing)
				directionFacing = value;
				OnSpriteChanged();
		}
	}

	public int CastStage{
		get{
			return castStage;
		}
		set{
			if(value != castStage)
				castStage = value;
				OnSpriteChanged();
		}
	}

	void OnSpriteChanged(){
		if(SpriteChanged != null)
			SpriteChanged.Invoke();
	}
	
	// Use this for initialization
	void Start () {
		//populate sprite dictionary from resource folder
		rend = GetComponent<SpriteRenderer>();
		bossSprites = Resources.LoadAll<Sprite>(path + spriteFileName);
		SpriteChanged.AddListener(SelectSprite);
		originalY = transform.position.y;
		aoe.gameObject.SetActive(false);
		healthPoints = maxHealthPoints;
		healthBar.maxValue = maxHealthPoints;
		healthBar.value = healthPoints;
	}
	
	// Update is called once per frame
	void Update () {
		//find the angle between player and boss
		Vector3 targetDir = playerTransform.position - transform.position;
		float angleUp = Vector3.Angle(targetDir, up);
		float angleRight = Vector3.Angle(targetDir, right);
		UpdateDirection(angleUp, angleRight);
		healthBar.value = healthPoints;
		// create floating

		transform.position = new Vector3(transform.position.x, originalY + (Mathf.Sin(Time.time * floatSpeed) * floatStrength), transform.position.z);
	}

	void SelectSprite(){
		int index = castStage;
		/* use the facing enum to a select correct sprite. There are 6 sprites for each direction
		 so you add 6 for each direction. Order is down, up, right, left.*/
		switch(directionFacing){
			case DoorFacing.down:
				rend.sprite = bossSprites[index];
				break;
			case DoorFacing.up:
				rend.sprite = bossSprites[index + spritesPerDirection];
				break;
			case DoorFacing.right:
				rend.sprite = bossSprites[index + (spritesPerDirection * 2)];
				break;
			case DoorFacing.left:
				rend.sprite = bossSprites[index + (spritesPerDirection * 3)];
				break;
		}
	}

	void UpdateDirection(float upAngle, float rightAngle){
		if(upAngle >= 315f || upAngle <= 45f)
			DirectionFacing = DoorFacing.up;
		else if(upAngle > 45f && upAngle < 135f && rightAngle < 90f)
			DirectionFacing = DoorFacing.right;
		else if(upAngle >= 135f && upAngle < 225f)
			DirectionFacing = DoorFacing.down;
		else
			DirectionFacing = DoorFacing.left;
	}

	public void UpdateHealth(int dmg) {
		healthPoints -= dmg;
		if(healthPoints == 0) {
			Death();
		}
	}
 
	void Death() {
		//possible death animation
		gameObject.SetActive(false);
		healthBar.transform.parent.gameObject.SetActive(false);
	}

	void OnActiveChanged(){
		StartCoroutine("CastingSequence");
		healthBar.transform.parent.gameObject.SetActive(true);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Player") {
			Active = true;
		}
	}

	IEnumerator CastingSequence(){
		while(true){
			StartCoroutine("CastSingleOrb");
			yield return new WaitForSeconds(3f);
			StartCoroutine("CastFollowOrb");
			yield return new WaitForSeconds(3f);
			// StartCoroutine("CastBossAoe");
			// yield return new WaitForSeconds(3f);
		}
	}

	IEnumerator CastSingleOrb(){
		var orbRend = attachedOrb.GetComponent<SpriteRenderer>();
		orbRend.color = Color.white;
		while(castStage != 5){
			orbRend.sprite = castingOrbSprites[castStage];
			castStage += 1;
			attachedOrb.SetActive(true);
			yield return new WaitForSeconds(0.25f);
		}
		castStage = 0;
		GameObject orb = ObjectPooler.Instance.GetPooledObject("StraightOrb");
		attachedOrb.SetActive(false);
		Projectile proj = orb.GetComponent<Projectile>();
		Vector3 targetDir = playerTransform.position - attachedOrb.transform.position;
		targetDir.Normalize();
		proj.direction = targetDir;
		proj.speed = 1;
		proj.damage = 1;
		orb.SetActive(true);
		orb.transform.position = attachedOrb.transform.position;
	}

	IEnumerator CastFollowOrb(){
		var orbRend = attachedOrb.GetComponent<SpriteRenderer>();
		orbRend.color = followProjColor;
		while(castStage != 5){
				orbRend.sprite = castingOrbSprites[castStage];
				castStage += 1;
				attachedOrb.SetActive(true);
				yield return new WaitForSeconds(0.25f);
		}
		castStage = 0;
		GameObject orb = ObjectPooler.Instance.GetPooledObject("FollowOrb");
		attachedOrb.SetActive(false);
		FollowProjectile proj = orb.GetComponent<FollowProjectile>();
		proj.player = playerTransform.gameObject;
		proj.damage = 1;
		proj.timeout = 3f;
		proj.speed = 0.5f;
		orb.SetActive(true);
		orb.transform.position = attachedOrb.transform.position;
	}

	IEnumerator CastBossAoe(){
		aoe.GetComponent<SpriteRenderer>().color = Color.white;
		aoe.gameObject.SetActive(true);
		while(castStage < 6){
			aoe.ChangeSprite(castStage);
			castStage += 1;
			if(castStage == 5){
				aoe.GetComponent<SpriteRenderer>().color = Color.red;
			}
			yield return new WaitForSeconds(0.25f);
		}
		castStage = 0;
		aoe.DealDamage(aoeDamage);
		aoe.playerInAoe = false;
		aoe.gameObject.SetActive(false);
	}
}
