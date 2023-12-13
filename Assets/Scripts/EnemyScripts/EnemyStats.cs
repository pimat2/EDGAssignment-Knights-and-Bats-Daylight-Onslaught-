using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	
	//CurrentStats
	[HideInInspector]
	public float currentMoveSpeed;
	[HideInInspector]
	public float currentHealth;
	[HideInInspector]
	public float currentDamage;
	
	public float despawnDistance = 20f;
	Transform player;
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentMoveSpeed = enemyData.MoveSpeed;
		currentHealth = enemyData.MaxHealth;
		currentDamage = enemyData.Damage;
	}
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	void Start()
	{
		player = FindObjectOfType<PlayerStats>().transform;
		
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	void Update()
	{
		if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
		{
			ReturnEnemy();
		}
	}
	
	public void TakeDamage(float dmg)
	{
		currentHealth -= dmg;
		
		if(currentHealth <= 0)
		{
			Kill();
		}
	}
	public void Kill()
	{
		Destroy(gameObject);
	}
	
	// OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
	private void  OnCollisionStay2D(Collision2D col)
	{
		
		//references the player stats script when collision happens and calls the function of TakeDamage there
		if(col.gameObject.CompareTag("Player"))
		{
			PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
			player.TakeDamage(currentDamage);
		}
	}
	
	// This function is called when the MonoBehaviour will be destroyed.
	private void OnDestroy()
	{
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		enemySpawner.OnEnemyKilled();
	}
	
	void ReturnEnemy()
	{
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		transform.position = player.position + enemySpawner.relativeSpawnPoints[Random.Range(0,enemySpawner.relativeSpawnPoints.Count)].position;
	}
}
