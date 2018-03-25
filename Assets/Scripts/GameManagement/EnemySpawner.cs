using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RWGameManager
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemy;
        private GameObject[] enemyscurr = new GameObject[5];
        private Transform loc;
        public int enemysAlive = 0;
        //public GameObject EnemySpawns;
        public Transform[] spawnlocs;
        public int enemiesToSpawn = 2;

        // Use this for initialization
        void Start()
        {
             GameObject[] sps = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
                int counter = 0;
                foreach (GameObject sp in sps)
                {
                    if (counter != sps.Length)
                    {
                        spawnlocs[counter] = sp.transform;
                        counter++;
                    }
                }

            int ensleft = enemiesToSpawn;
            while(ensleft > 0)
            {
                Trigger_spawn();
                ensleft--;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (enemysAlive == 0)
            {
                enemyscurr = new GameObject[5];
                GetComponent<GameManager>().AddRoundWon();
                int ensleft = enemiesToSpawn;
                while (ensleft > 0)
                {
                    Trigger_spawn();
                    ensleft--;
                }
            }
        }

        public void enemyKilled()
        {

        }

        public void Trigger_spawn()
        {
            int num = Random.Range(0, spawnlocs.Length - 1);
            loc = spawnlocs[num];
            GameObject current = Instantiate(enemy, loc, true);
            enemyscurr[enemysAlive] = current;
            print(current.name);
            current.transform.position = loc.transform.position;
            enemysAlive++;
        }

    }
}
