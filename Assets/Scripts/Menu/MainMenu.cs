using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void loadLevel()
	{
		SceneManager.LoadScene("LoadingScreen");
	}

	public void populateLevelData(GameObject level)
	{
		level.GetComponent<Text>().text = PlayerPrefs.GetInt(level.gameObject.name + "_highScore").ToString();
	}
}
