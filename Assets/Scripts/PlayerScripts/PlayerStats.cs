using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	CharacterScriptableObject characterData;
	
	//CurrentStats
	[HideInInspector]
	public float currentHealth;
	[HideInInspector]
	public float currentRecovery;
	[HideInInspector]
	public float currentMoveSpeed;
	[HideInInspector]
	public float currentMight;
	[HideInInspector]
	public float currentProjectileSpeed;
	[HideInInspector]
	public float currentMagnet;
	
	//Experience and level of the player
	[Header("Experience/Level")]
	public int experience = 0;
	public int level = 1;
	public int experienceCap;
	
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
	
	InventoryManager inventory;
	public int weaponIndex;
	public int passiveItemIndex;
	
	//For Testing Purposes
	public GameObject weaponTest;
	public GameObject passiveItemTest1;
	public GameObject passiveItemTest2;
	
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		characterData = CharacterSelector.GetData();
		CharacterSelector.instance.DestroySingleton();
		
		inventory = GetComponent<InventoryManager>();
		
		
		//Asigning Variables for different stats of the player
		currentHealth = characterData.MaxHealth;
		currentRecovery = characterData.Recovery;
		currentMoveSpeed = characterData.MoveSpeed;
		currentMight = characterData.Might;
		currentProjectileSpeed = characterData.ProjectileSpeed; 
		currentMagnet = characterData.Magnet;
		
		//Spawning the starting weapon of the character based on what is assigned in the scriptableobject	
		SpawnWeapon(characterData.StartingWeapon);
		//For testing purposes
		SpawnWeapon(weaponTest);
		SpawnPassiveItem(passiveItemTest1);
		SpawnPassiveItem(passiveItemTest2);
	}
	
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
		Recover();
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
	
	public void RestoreHealth(float amount)
	{
		if(currentHealth < characterData.MaxHealth)
		{
			currentHealth += amount;
			if(currentHealth > characterData.MaxHealth)
			{
				currentHealth = characterData.MaxHealth;
			}
		}
		
	}
	
	void Recover()
	{
		if(currentHealth < characterData.MaxHealth)
		{
			currentHealth += currentRecovery * Time.deltaTime;
			if(currentHealth > characterData.MaxHealth)
			{
				currentHealth = characterData.MaxHealth;
			}
		}
		
	}
	public void SpawnWeapon(GameObject weapon)
	{
		if(weaponIndex >= inventory.weaponSlots.Count - 1)
		{
			return;
		}
		//Spawn the correct starting weapon of player character
		GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
		spawnedWeapon.transform.SetParent(transform); //Sets the weapon to be a child of the player character
		inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());
		weaponIndex++;
	}
	public void SpawnPassiveItem(GameObject passiveItem)
	{
		if(passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
		{
			return;
		}
		//Spawn the correct starting weapon of player character
		GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
		spawnedPassiveItem.transform.SetParent(transform); //Sets the weapon to be a child of the player character
		inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());
		passiveItemIndex++;
	}
}
