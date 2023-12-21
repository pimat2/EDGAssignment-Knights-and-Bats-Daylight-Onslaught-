using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
	CharacterScriptableObject characterData;
	
	//CurrentStats
	
	float currentHealth;
	
	float currentRecovery;
	
	float currentMoveSpeed;
	
	float currentMight;
	
	float currentProjectileSpeed;
	
	float currentMagnet;

	#region CurrentStatsProperties
	float CurrentHealth
	{
		get { return currentHealth; }
		set
		{
			//Checks if the value of current health has been changed
			if(currentHealth != value)
			{
				currentHealth = value;
				if(GameManager.instance != null)
				{
					GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
				}
				//Updates the real time value of the stat
			}
		}
	}
	public float CurrentRecovery
	{
		get { return currentRecovery; }
		set
		{
			//Checks if the value of current recovery has been changed
			if(currentRecovery != value)
			{
				currentRecovery = value;
				if(GameManager.instance != null)
				{
					GameManager.instance.currentRecoveryDisplay.text = "Recovery " + currentRecovery;
				}
				//Updates the real time value of the stat
			}
		}
	}
	public float CurrentMoveSpeed
	{
		get { return currentMoveSpeed; }
		set
		{
			//Checks if the value of current Move Speed has been changed
			if(currentMoveSpeed != value)
			{
				currentMoveSpeed = value;
				if(GameManager.instance != null)
				{
					GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
				}
				//Updates the real time value of the stat
			}
		}
	}
	public float CurrentMight
	{
		get { return currentMight; }
		set
		{
			//Checks if the value of current Might has been changed
			if(currentMight != value)
			{
				currentMight = value;
				if(GameManager.instance != null)
				{
					GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
				}
				//Updates the real time value of the stat
			}
		}
	}
	public float CurrentProjectileSpeed
	{
		get { return currentProjectileSpeed; }
		set
		{
			//Checks if the value of current Projectile Speed has been changed
			if(currentProjectileSpeed != value)
			{
				currentProjectileSpeed = value;
				if(GameManager.instance != null)
				{
					GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
				}
				//Updates the real time value of the stat
			}
		}
	}
	public float CurrentMagnet
	{
		get { return currentMagnet; }
		set
		{
			//Checks if the value of current Magnet has been changed
			if(currentMagnet != value)
			{
				currentMagnet = value;
				if(GameManager.instance != null)
				{
					GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
				}
				//Updates the real time value of the stat
			}
		}
	}
	#endregion
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

	[Header("UI")]
	public Image healthBar;
	public Image expBar;
	public TextMeshProUGUI experienceLevelText;
	
	
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		characterData = CharacterSelector.GetData();
		CharacterSelector.instance.DestroySingleton();
		
		inventory = GetComponent<InventoryManager>();
		
		
		//Asigning Variables for different stats of the player
		CurrentHealth = characterData.MaxHealth;
		CurrentRecovery = characterData.Recovery;
		CurrentMoveSpeed = characterData.MoveSpeed;
		CurrentMight = characterData.Might;
		CurrentProjectileSpeed = characterData.ProjectileSpeed; 
		CurrentMagnet = characterData.Magnet;
		
		//Spawning the starting weapon of the character based on what is assigned in the scriptableobject	
		SpawnWeapon(characterData.StartingWeapon);
		
		//For testing purposes
		//SpawnWeapon(weaponTest);
		SpawnPassiveItem(passiveItemTest1);
		//SpawnPassiveItem(passiveItemTest2);
	}
	
	void Start()
	{
		//Initialize the experience cap as the first experience cap increase
		experienceCap = levelRanges[0].experienceCapIncrease;	
		//Sets the current stats display
		GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
		GameManager.instance.currentRecoveryDisplay.text = "Recovery " + currentRecovery;
		GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
		GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
		GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
		GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

		GameManager.instance.AssignChosenCharacterUI(characterData);

		UpdateHealthBar();
		UpdateExpBar();
		UpdateLevelText();
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
		experience = experience + amount;
		UpdateExpBar();
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

			UpdateLevelText();

			GameManager.instance.StartLevelUp();

		}
	}
	
	public void TakeDamage(float dmg)
	{
		if(isInvincible == false)
		{
			CurrentHealth -= dmg;
			
			invincibilityTimer = invincibilityDuration;
			isInvincible = true;
		
			if(CurrentHealth <= 0)
			{
				Kill();
			}
			UpdateHealthBar();
		}
		
	}
	public void Kill()
	{
		Debug.Log("PLAYER IS DEAD");
		if(!GameManager.instance.isGameOver)
		{
			GameManager.instance.AssignLevelReached(level);
			GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemsUISlots);
			GameManager.instance.GameOver();
		}
	}
	
	public void RestoreHealth(float amount)
	{
		if(CurrentHealth < characterData.MaxHealth)
		{
			CurrentHealth += amount;
			if(CurrentHealth > characterData.MaxHealth)
			{
				CurrentHealth = characterData.MaxHealth;
			}
		}
		
	}
	
	void Recover()
	{
		if(CurrentHealth < characterData.MaxHealth)
		{
			CurrentHealth += CurrentRecovery * Time.deltaTime;
			if(CurrentHealth > characterData.MaxHealth)
			{
				CurrentHealth = characterData.MaxHealth;
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

	void UpdateHealthBar()
	{
		healthBar.fillAmount = currentHealth / characterData.MaxHealth;
	}
	void UpdateExpBar()
	{
		//Update exp bar fill amount
		expBar.fillAmount = (float)experience / (float)experienceCap;
	}
	void UpdateLevelText()
	{
		//Update level text
		experienceLevelText.text = "LV" + level.ToString();
	}
}
