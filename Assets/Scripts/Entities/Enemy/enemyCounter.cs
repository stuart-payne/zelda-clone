using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class enemyCounter : MonoBehaviour {

	// Use this for initialization
	basicEnemyController[] enemyChildren;
	
	[SerializeField]
	int enemyCount;

	public UnityEvent OnGoalReached;

	void Start () {
		enemyChildren = GetComponentsInChildren<basicEnemyController>();
		foreach(var enemy in enemyChildren){
			enemy.OnDeath.AddListener(DecrementCounter);
		}
		enemyCount = enemyChildren.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DecrementCounter(){
		enemyCount -= 1;
		if(enemyCount == 0 && OnGoalReached != null)
			OnGoalReached.Invoke();
	}
}
