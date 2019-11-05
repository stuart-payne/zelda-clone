using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	
	public static ObjectPooler Instance;

	public List<GameObject> pooledObjects;
	public List<PoolObject> objectsToPool;
	// Use this for initialization
	void Start () {
		Instance = this;
		PopulatePooledObjects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ResetPooledObjects() {
		pooledObjects = new List<GameObject>();
		PopulatePooledObjects();
	}

	void PopulatePooledObjects () {
		foreach(var poolObj in objectsToPool){
			for(var i = 0; i < poolObj.amountToPool; i++){
			GameObject obj = (GameObject)Instantiate(poolObj.objectToPool);
			obj.SetActive(false);
			pooledObjects.Add(obj);
			}
		}
	}

	public GameObject GetPooledObject(string tag){
		for(var i = 0; i < pooledObjects.Count; i++){
			if(pooledObjects[i].activeInHierarchy == false && pooledObjects[i].tag == tag)
				return pooledObjects[i];
		}
		return null;
	}


}

[System.Serializable]
public class PoolObject{
	public GameObject objectToPool;
	public int amountToPool;
}
