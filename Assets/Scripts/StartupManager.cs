using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartupManager : MonoBehaviour {

	public Button exit, start;
	// Use this for initialization
	void Start () {
		exit.onClick.AddListener(ExitGame);
		start.onClick.AddListener(StartGame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame(){
		StartCoroutine("LoadGame");
	}

	public void ExitGame(){
		Application.Quit();
	}

	IEnumerator LoadGame(){
		AsyncOperation loadAsync = SceneManager.LoadSceneAsync("main");

		while(!loadAsync.isDone)
			yield return null;
	}
}
