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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
