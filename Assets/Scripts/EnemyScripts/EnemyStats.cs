using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	
	//CurrentStats
	float currentMoveSpeed;
	float currentHealth;
	float currentDamage;
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentMoveSpeed = enemyData.MoveSpeed;
		currentHealth = enemyData.MaxHealth;
		currentDamage = enemyData.Damage;
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
}
