using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public CharacterScriptableObject characterData;
	// Start is called before the first frame update
    
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
	
	public List<LevelRange> levelRanges;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	void Start()
	{
		//Initialize the experience cap as the first experience cap increase
		experienceCap = levelRanges[0].experienceCapIncrease;	
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
	
}
