using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour {
	private GameObject loadingImage;
	private Text tipText;
	private bool hasLoadingImage;
	public bool imageRotation;
	public bool imagePulse;
	public string[] loadingTips;
	public string levelToLoad;
	private AsyncOperation asyncLoad;
	// Use this for initialization
	void Start () {
		SceneManager.UnloadSceneAsync("MainMenu");

		if(GameObject.Find("LoadingIcon"))
		{
			loadingImage = GameObject.Find("LoadingIcon");
			hasLoadingImage = true;
		}
		tipText = GameObject.Find("LoadingTips").GetComponent<Text>();

		if(loadingTips.Length > 0)
		{
			StartCoroutine(TipsDisplay());
		}

		if(hasLoadingImage)
		{
			if(imagePulse)
			{
				StartCoroutine(LoadingPulser());
			}
		}

		asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
		//asyncLoad.allowSceneActivation = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(hasLoadingImage)
		{
			if(imageRotation)
			{
				loadingImage.transform.Rotate(new Vector3(0,0,2.0f));
			}
		}

		print(asyncLoad.progress);

		if(asyncLoad.isDone)
		{
			asyncLoad.allowSceneActivation = true;
		}
	}

	IEnumerator TipsDisplay()
	{
		string tip = loadingTips[Random.Range(0, loadingTips.Length -1)];
		tipText.text = tip;
		yield return new WaitForSecondsRealtime(5.0f);
		StartCoroutine(TipsDisplay());
	}

	IEnumerator LoadingPulser()
	{
		Image lodImg = loadingImage.GetComponent<Image>();
		lodImg.CrossFadeAlpha(0.0f,2.0f,false);
		yield return new WaitForSecondsRealtime(1.0f);
		lodImg.CrossFadeAlpha(255.0f,2.0f,false);
		yield return new WaitForSecondsRealtime(3.0f);
		StartCoroutine(LoadingPulser());
	}
}
