using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[System.Serializable]
	public class Wave
	{
		public string waveName;
		public List<EnemyGroup> enemyGroups;//A list of groups of enemies to spawn in this wave
		public int waveQuota; //Total number of enemies to spawn in wave
		public float spawnInterval; //Interval to spawn enemies
		public float spawnCount; //Number of enemies already spawned in wave
	}
	[System.Serializable]
	public class EnemyGroup
	{
		public string enemyName;
		public int enemyCount; //Number of enemies to spawn in wave
		public int spawnCount; //Number of enemies of this type already spawned in wave
		public GameObject enemyPrefab;
	}
	
	public List<Wave> waves; //A list of all the waves in the game
	public int currentWaveCount; //The index of the current wave(starts from 0)
	
	[Header("Spawner Attributes")]
	float spawnTimer;//timer for when to spawn the next enemy 
	Transform player;
	
    // Start is called before the first frame update
    void Start()
	{
		player = FindObjectOfType<PlayerStats>().transform;
		CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
	    spawnTimer += Time.deltaTime;
	    //Checks if its time to spawn the next enemy
	    if(spawnTimer >= waves[currentWaveCount].spawnInterval)
	    {
	    	spawnTimer = 0;
	    	SpawnEnemies();
	    }
    }
	void CalculateWaveQuota()
	{
		int currentWaveQuota = 0;
		foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
		{
			currentWaveQuota += enemyGroup.enemyCount;
		}
		
		waves[currentWaveCount].waveQuota = currentWaveQuota;
		Debug.Log(currentWaveQuota);
	}
	
	void SpawnEnemies()
	{
		//Check if the minimum number of enemies in the wave have been spawned
		if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
		{
			//Spawn each type of enemy until the quota is filled
			foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
			{
				//Check if the minimum number of enemies of this type have been spawned
				if(enemyGroup.spawnCount < enemyGroup.enemyCount)
				{
					Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
					Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
					
					enemyGroup.spawnCount++;
					waves[currentWaveCount].spawnCount++;
				}
			}
		}
	}
}
