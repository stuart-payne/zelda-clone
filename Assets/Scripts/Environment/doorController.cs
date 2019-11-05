using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorFacing{
	up,
	down,
	right,
	left
}

public class doorController : BinaryStateSprite {

	// Use this for initialization
	GameObject player;
	public int doorID;
	public int connectedDoor;
	public DoorFacing doorDirection;
	public float playerTeleportOffset = 0.2f;
	public bool startingDoorOpen = false;
	BoxCollider2D col;
	Vector3 teleportOffset;

	public override void Start () {

		rend = gameObject.GetComponent<SpriteRenderer>();
		col = gameObject.GetComponent<BoxCollider2D>();
		OnStateChange += UpdateSprite;
		OnStateChange += DebugEvent;
		OnStateChange += SwitchCollider;
		isOpen = startingDoorOpen;
		UpdateSprite(isOpen);
		GetTeleportPositionOffset();
		Debug.Log("teleport offset is"  + teleportOffset.y);
		LevelManager.instance.doorPosition.Add(doorID, gameObject.transform.position + teleportOffset);
	}

	void SwitchCollider(bool state)
	{
		if(state)
			col.enabled = false;
		else
			col.enabled = true;
	}
	
	void DoorConnection(){
		player.transform.position = LevelManager.instance.doorPosition[connectedDoor];
		string room = LevelManager.instance.doorToRoom[connectedDoor];
		//Camera.current.transform.position = LevelManager.instance.roomCameraPosition[room];
	}

	void GetTeleportPositionOffset(){
		switch(doorDirection){
			case DoorFacing.up:
				teleportOffset = new Vector3(0, playerTeleportOffset, 0);
				break;
			case DoorFacing.down:
				teleportOffset = new Vector3(0, -playerTeleportOffset, 0);
				break;
			case DoorFacing.right:
				teleportOffset = new Vector3(playerTeleportOffset, 0, 0);
				break;
			case DoorFacing.left:
				teleportOffset = new Vector3(-playerTeleportOffset, 0, 0);
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			player = col.gameObject;
			DoorConnection();
		}
	}

	IEnumerator DebugConnection(){
		yield return new WaitForSeconds(2f);
		Debug.Log(connectedDoor);
	}
}
