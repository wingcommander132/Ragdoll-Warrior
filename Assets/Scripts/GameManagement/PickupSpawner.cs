using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {
    public GameObject[] spawns;
    public GameObject[] pickups;
	// Use this for initialization
	void Start () {
        spawns = new GameObject[GameObject.FindGameObjectsWithTag("EnemySpawnPoint").Length];
        spawns = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        StartCoroutine(PickupSpawn());        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PickupSpawn()
    {
        GameObject spawn = spawns[Random.Range(0,spawns.Length-1)];
        GameObject pickup = pickups[Random.Range(0, pickups.Length - 1)];
        Instantiate(pickup, spawn.transform);
        yield return new WaitForSecondsRealtime(Random.Range(4.0f,20.0f));
        yield return StartCoroutine(PickupSpawn());
    }
}
