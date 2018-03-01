using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RWGameManager
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemy;
        private EnemyController enscript;
        private GameObject enemycurr;
        private Transform loc;

        public bool enemyAlive = true;
        public GameObject EnemySpawns;
        public Transform[] spawnlocs;

        // Use this for initialization
        void Start()
        {
            Trigger_spawn();
            enemycurr = GameObject.FindGameObjectWithTag("Enemy");
            enscript = enemycurr.GetComponentInChildren<EnemyController>();

            int counter = 0;
            foreach (GameObject sp in GameObject.FindGameObjectsWithTag("EnemySpawnPoint"))
            {
                if (counter != GameObject.FindGameObjectsWithTag("EnemySpawnPoint").Length)
                {
                    spawnlocs[counter] = sp.transform;
                    counter++;
                }
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (enemyAlive == false)
            {
                Trigger_spawn();
            }
        }

        public void Trigger_spawn()
        {
            loc = spawnlocs[Random.Range(0, spawnlocs.Length)];
            Instantiate(enemy, loc);
            enemycurr = GameObject.FindGameObjectWithTag("Enemy");
            enscript = enemycurr.GetComponentInChildren<EnemyController>();
            enemyAlive = true;
        }

    }
}
