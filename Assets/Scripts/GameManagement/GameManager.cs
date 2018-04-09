using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RWGameManager
{
    public class GameManager : MonoBehaviour {
        public int score = 0;
        public int round = 0;
        private EnemySpawner enSpawner;
        private Text roundtext;
        private Text gemtext;
        private Text scoreText;
        public GameObject pauseUI;
        private GameObject playerJoystickController;
        private Scene currentScene;
        private bool paused = false;
        public int _highScore;
        private GameObject playerUI;
        private float IntTimeScale;
        public GameObject[] playerPickups;
        public int PlayerGems;
        
        // Use this for initialization
        void Start () {
            currentScene = SceneManager.GetActiveScene();
            enSpawner = FindObjectOfType<EnemySpawner>();
            //roundtext = GameObject.FindGameObjectWithTag("RoundText").GetComponent<Text>();
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
            gemtext = GameObject.FindGameObjectWithTag("GemText").GetComponent<Text>();
            playerJoystickController = GameObject.FindGameObjectWithTag("JoystickCanvas");
            playerUI = GameObject.FindGameObjectWithTag("PlayerUI");

            if(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_highScore") != 0)
                _highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_highScore");
            print(PlayerPrefs.GetInt("gems"));
        }
	
	    // Update is called once per frame
	    void Update () {
		    if(enSpawner.enemysAlive > 0)
            {

            }
            
        }

        void FixedUpdate()
        {
            if(score > _highScore)
            {
                _highScore = score;
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_highScore",_highScore);
                PlayerPrefs.Save();
            }

            if(Input.GetKey(KeyCode.Escape))
            {
                PauseGame();
            }

            gemtext.text = PlayerGems.ToString();
            scoreText.text = score.ToString();
        }
        
        public void GemCollect()
        {
            PlayerGems++;
        }


        public void ToMenu()
        {
            int curgems = PlayerPrefs.GetInt("gems");
            PlayerPrefs.SetInt("gems", curgems + PlayerGems);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu");
        }

        public void PauseGame()
        {
            if (!paused)
            {
                paused = true;
                pauseUI.SetActive(true);
                playerJoystickController.SetActive(false);
                playerUI.SetActive(false);
                IntTimeScale = Time.timeScale;
                Time.timeScale = IntTimeScale / 25.0f;
            }
            else
            {
                paused = false;
                pauseUI.SetActive(false);
                playerJoystickController.SetActive(true);
                playerUI.SetActive(true);
                Time.timeScale = IntTimeScale;
            }
        }


        public void AddToScore(float value)
        {
            score = score + Mathf.RoundToInt(value);
        }

        public void reload_scene()
        {
            int curgems = PlayerPrefs.GetInt("gems");
            PlayerPrefs.SetInt("gems", curgems + PlayerGems);
            PlayerPrefs.Save();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        public void AddRoundWon()
        {
            score += 100;
            round++;
        }
    }
}