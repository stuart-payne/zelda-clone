using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	public static LevelManager instance;
	
	public Dictionary<string, List<int>> roomToDoor = new Dictionary<string, List<int>>();
	public Dictionary<int, string> doorToRoom = new Dictionary<int, string>();
	public Dictionary<int, Vector3> doorPosition = new Dictionary<int, Vector3>();
	public Dictionary<string, Vector3> roomCameraPosition = new Dictionary<string, Vector3>();
	
	void Awake () {
		if(instance == null)
			instance = this;
		else
			Destroy(this.gameObject);

		//StartCoroutine("DebugDict");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator DebugDict(){
		yield return new WaitForSeconds(2f);
		foreach(var stuff in doorPosition){
			Debug.Log(stuff.Value + " " + stuff.Key);
		}
	}
}
