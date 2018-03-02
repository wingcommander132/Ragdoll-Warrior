using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RWGameManager
{
    public class GameManager : MonoBehaviour {
        public int score = 0;
        public int round = 1;
        public int highScore = 0;
        public int mostRounds = 0;
        private EnemySpawner enSpawner;
        private Text maxRounds;
        private Text highScoreText;
        private Text roundtext;
        private Text scoreText;
        // Use this for initialization
        void Start () {
            enSpawner = FindObjectOfType<EnemySpawner>();
            maxRounds = GameObject.FindGameObjectWithTag("MaxRoundsText").GetComponent<Text>();
            highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<Text>();
            roundtext = GameObject.FindGameObjectWithTag("RoundText").GetComponent<Text>();
            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        }
	
	    // Update is called once per frame
	    void Update () {
		    if(enSpawner.enemyAlive == true)
            {

            }
            maxRounds.text = mostRounds.ToString();
            highScoreText.text = highScore.ToString();
            roundtext.text = round.ToString();
            scoreText.text = score.ToString();
        }

        public void AddToScore(float value)
        {
            score = score + Mathf.RoundToInt(value);
            if (score > highScore)
                highScore = score;
            
        }

        public void AddRoundWon(int enemyLevel)
        {
            score += enemyLevel * 10;
            round++;
            
            if (round > mostRounds)
                mostRounds = round;
        }
    }
}