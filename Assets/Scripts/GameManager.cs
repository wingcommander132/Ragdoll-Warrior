using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RWGameManager
{
    public class GameManager : MonoBehaviour {
        public int score = 0;
        public int round = 1;
        public int highScore = 0;
        public int mostRounds = 0;
        private EnemySpawner enSpawner;
        // Use this for initialization
        void Start () {
            enSpawner = FindObjectOfType<EnemySpawner>();
	    }
	
	    // Update is called once per frame
	    void Update () {
		    if(enSpawner.enemyAlive == true)
            {

            }
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