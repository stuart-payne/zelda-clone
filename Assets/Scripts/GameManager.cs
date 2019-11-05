using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	public Image screenFader;
	public Text gameOverText;
	public ObjectPooler objectPooler;
	public Button button;
	
	[Range(0, 1)]
	public float gameOverAlpha;
	public static GameManager instance = null;
	
	void Awake () {
		//singleton
		if(instance == null)
			instance = this;
		else if(instance !=  this){
			instance.screenFader = screenFader;
			instance.gameOverText = gameOverText;
			instance.button = button;
			instance.button.onClick.AddListener(instance.ResetScene);
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
		button.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GameOver(){
		StartCoroutine("FadeGameOverScreen");
	}

	IEnumerator FadeGameOverScreen(){
		Color newColor = screenFader.color;
		newColor.a = 0;
		
		while(screenFader.color.a <= gameOverAlpha){
			newColor.a += (gameOverAlpha / 5); 
			screenFader.color =  newColor;
			yield return new WaitForSeconds(0.05f);
		}

		Color textColor = gameOverText.color;
		textColor.a = 1f;
		gameOverText.color = textColor; 
		button.gameObject.SetActive(true);
	}

	public void ResetScene(){
		StartCoroutine("LoadAsyncScene");
	}

	IEnumerator LoadAsyncScene(){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("main");

		while(!asyncLoad.isDone){
			yield return null;
			objectPooler.ResetPooledObjects();
		}
	}
}
