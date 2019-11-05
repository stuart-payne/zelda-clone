using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomCameraPosition : MonoBehaviour {

	public Vector3 cameraPosition;
	string roomName;
	List<int> doorsInRoom = new List<int>();

	void Start(){
		roomName = gameObject.name;
		LevelManager.instance.roomCameraPosition.Add(roomName, cameraPosition);
		doorController[] doors = GetComponentsInChildren<doorController>();

		foreach(var door in doors){
			LevelManager.instance.doorToRoom.Add(door.doorID, roomName);
			doorsInRoom.Add(door.doorID);
		}

		LevelManager.instance.roomToDoor.Add(roomName, doorsInRoom);
	}
}
