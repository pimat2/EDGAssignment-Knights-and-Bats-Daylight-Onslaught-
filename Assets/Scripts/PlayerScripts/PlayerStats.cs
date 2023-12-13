using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public CharacterScriptableObject characterData;
	
	//CurrentStats
	float currentHealth;
	float currentRecovery;
	float currentMoveSpeed;
	float currentMight;
	float currentProjectileSpeed;
	
	//Experience and level of the player
	[Header("Experience/Level")]
	public int experience = 0;
	public int level = 1;
	public int experienceCap;
	
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentHealth = characterData.MaxHealth;
		currentRecovery = characterData.Recovery;
		currentMoveSpeed = characterData.MoveSpeed;
		currentMight = characterData.Might;
		currentProjectileSpeed = characterData.ProjectileSpeed; 
	}
	
	//Class for defining a level range and the corresponding experience cap increase for that range
	[System.Serializable]
	public class LevelRange
	{
		public int startLevel;
		public int endLevel;
		public int experienceCapIncrease;
	}
	
	//Invincibility Frames
	[Header("Invincibility Frames")]
	public float invincibilityDuration;
	float invincibilityTimer;
	bool isInvincible;
	
	
	public List<LevelRange> levelRanges;
	
	
	void Start()
	{
		//Initialize the experience cap as the first experience cap increase
		experienceCap = levelRanges[0].experienceCapIncrease;	
	}
	
	
	void Update()
	{
		if(invincibilityTimer > 0)
		{
			invincibilityTimer -= Time.deltaTime;
		}
		else if(isInvincible)
		{
			isInvincible = false;
		}
	}
	
	public void IncreaseExperience(int amount)
	{
		experience =+ amount;
		LevelUpChecker();
	}
	
	void LevelUpChecker()
	{
		if(experience >= experienceCap)
		{
			level++;
			experience -= experienceCap;
			
			int experienceCapIncrease = 0;
			foreach(LevelRange range in levelRanges)
			{
				if(level >= range.startLevel && level <= range.endLevel)
				{
					experienceCapIncrease = range.experienceCapIncrease;
					break;
				}
			}
			experienceCap += experienceCapIncrease;
		}
	}
	
	public void TakeDamage(float dmg)
	{
		if(isInvincible == false)
		{
			currentHealth -= dmg;
			
			invincibilityTimer = invincibilityDuration;
			isInvincible = true;
		
			if(currentHealth <= 0)
			{
				Kill();
			}
		}
		
	}
	public void Kill()
	{
		Debug.Log("PLAYER IS DEAD");
	}
}
