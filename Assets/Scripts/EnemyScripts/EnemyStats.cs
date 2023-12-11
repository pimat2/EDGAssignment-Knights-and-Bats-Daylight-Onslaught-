using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	
	//CurrentStats
	float currentMoveSpeed;
	float currentHealth;
	float Damage;
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentMoveSpeed = enemyData.MoveSpeed;
		currentHealth = enemyData.MaxHealth;
		Damage = enemyData.Damage;
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
}
