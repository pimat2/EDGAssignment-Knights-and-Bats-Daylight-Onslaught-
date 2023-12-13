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
	public int enemiesAlive;
	public int maxEnemiesAllowed; //Maximum number of enemies allowed on the map at once
	public bool maxEnemiesReached = false;
	public float waveInterval; //Time interval between each wave
	[Header("Spawn Positions")]
	public List<Transform> relativeSpawnPoints;//A list to store all the relative spawn points of enemies
	
	
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
		//Check if current wave has ended and next wave should start
		if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
		{
			StartCoroutine(BeginNextWave());
		}
	    spawnTimer += Time.deltaTime;
	    //Checks if its time to spawn the next enemy
	    if(spawnTimer >= waves[currentWaveCount].spawnInterval)
	    {
	    	spawnTimer = 0;
	    	SpawnEnemies();
	    }
    }
    
    
	IEnumerator BeginNextWave()
	{
		//Wait for 'waveinterva' seconds before checking if there are more waves to be spawned, if so increase currentWaveCount and CalculateWaveQuota
		yield return new WaitForSeconds(waveInterval);
		if(currentWaveCount < waves.Count -1)
		{
			currentWaveCount ++;
			CalculateWaveQuota();
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
	
	
	/// <summary>
	/// This method will stop spawning enemies if the amount of enemies on the map is more than the maximum allowed.
	/// The method will only spawn enemies in a particular wave until it is time for the next wave's enemies to be spawned.
	/// </summary>
	void SpawnEnemies()
	{
		//Check if the minimum number of enemies in the wave have been spawned
		if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
		{
			//Spawn each type of enemy until the quota is filled
			foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
			{
				//Check if the minimum number of enemies of this type have been spawned
				if(enemyGroup.spawnCount < enemyGroup.enemyCount)
				{
					//Stops the spawning of enemies if enemiesAlive is more than the max number allowed
					if(enemiesAlive >= maxEnemiesAllowed)
					{
						maxEnemiesReached = true;
						return;
					}
					
					//Spawn the enemy at a random position close to the player
					Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0,relativeSpawnPoints.Count)].position, Quaternion.identity);
					
					enemyGroup.spawnCount++;
					waves[currentWaveCount].spawnCount++;
					enemiesAlive++;
				}
			}
		}
		//Resets the bool if enemiesAlive falls belowe the max number of enemies allowed
		if(enemiesAlive < maxEnemiesAllowed)
		{
			maxEnemiesReached = false;
		}
	}
	
	public void OnEnemyKilled()
	{
		enemiesAlive--;
	}
}
