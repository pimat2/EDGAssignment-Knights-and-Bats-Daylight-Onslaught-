using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
	public List<WeaponController> weaponSlots = new List<WeaponController>(6);
	public int[] weaponLevels = new int[6];
	public List<Image> weaponUISlots = new List<Image>(6);
	public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
	public int[] passiveItemLevels = new int[6];
	public List<Image> passiveItemsUISlots = new List<Image>(6);

	[System.Serializable]
	public class WeaponUpgrade
	{
		public int weaponUpgradeIndex;
		public GameObject initialWeapon;
		public WeaponScriptableObject weaponData;
	}
	[System.Serializable]
	public class PassiveItemUpgrade
	{
		public int passiveItemUpgradeIndex;
		public GameObject initialPassiveItem;
		public PassiveItemScriptableObject PassiveItemData;
	}
	[System.Serializable]
	public class UpgradeUI
	{
		public TextMeshProUGUI upgradeNameDisplay;
		public TextMeshProUGUI upgradeDescriptionDispaly;
		public Image upgradeIcon;
		public Button upgradeButton;
	}
	
	public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>(); //List of upgrade options for weaopns
	public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();//List of upgrade options for passive items
	public List<UpgradeUI> upgradeOptions = new List<UpgradeUI>(); //List of ui for upgrade options present in the scene

	PlayerStats player;

	void Start() 
	{
		player = GetComponent<PlayerStats>();
	}

	public void AddWeapon(int slotIndex, WeaponController weapon)//Adds a weapon to a specific slot in the list weaponSlots
	{
		weaponSlots[slotIndex] = weapon;
		weaponLevels[slotIndex] = weapon.weaponData.Level;
		weaponUISlots[slotIndex].enabled = true;
		weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
		if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
		{
			GameManager.instance.EndLevelUp();
		}
	}
	
	public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)//Adds a passive item to a specific slot in the list passiveItemSlots
	{
		passiveItemSlots[slotIndex] = passiveItem;
		passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
		passiveItemsUISlots[slotIndex].enabled = true;
		passiveItemsUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
		if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
		{
			GameManager.instance.EndLevelUp();
		}
	}
	
	public void LevelUpWeapon(int slotIndex, int upgradeIndex)
	{
		if(weaponSlots.Count > slotIndex)
		{
			WeaponController weapon = weaponSlots[slotIndex];
			if(!weapon.weaponData.NextLevelPrefab)//Checks if there is a next level for the current weapon
			{
				Debug.LogError("NO NEXT LEVEL FOR" + weapon.name);
				return;
			}
			GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
			upgradedWeapon.transform.SetParent(transform);
			AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
			Destroy(weapon.gameObject);
			weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level; //Makes sure we have the correct weapon level

			weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

			if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
			{
				GameManager.instance.EndLevelUp();
			}
		}
	}
	
	public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
	{
		if(passiveItemSlots.Count > slotIndex)
		{
			PassiveItem passiveItem = passiveItemSlots[slotIndex];
			if(!passiveItem.passiveItemData.NextLevelPrefab)//Checks if there is a next level for the current passive item
			{
				Debug.LogError("NO NEXT LEVEL FOR" + passiveItem.name);
				return;
			}
			GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
			upgradedPassiveItem.transform.SetParent(transform);
			AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
			Destroy(passiveItem.gameObject);
			passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level; //Makes sure we have the correct passive item level

			passiveItemUpgradeOptions[upgradeIndex].PassiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData;

			if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
			{	
				GameManager.instance.EndLevelUp();
			}
		}
	}
	void ApplyUpgradeOptions()
	{
		List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
		List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

		foreach (var upgradeOption in upgradeOptions)
		{
			if(availablePassiveItemUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
			{
				return;
			}
			
			int upgradeType;

			if(availableWeaponUpgrades.Count == 0)
			{
				upgradeType = 2;
			}
			else if(availablePassiveItemUpgrades.Count == 0)
			{
				upgradeType = 1;
			}
			else
			{
				upgradeType = Random.Range(1,3); //Randomly choose between weapons and passive items
			}

			if(upgradeType == 1)
			{
				WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];

				availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

				if(chosenWeaponUpgrade != null)
				{
					EnableUpgradeUI(upgradeOption);
					bool newWeapon = false;
					for (int i = 0; i < weaponSlots.Count; i++)
					{
						if(weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
						{
							newWeapon = false;
							if(!newWeapon)
							{
								if(!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
								{
									DisableUpgradeUI(upgradeOption);
									break;
								}
								upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex)); //Apply button funcitonality
								//Set the description and the name to be that of the next level weapon's name and description
								upgradeOption.upgradeDescriptionDispaly.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
								upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
							}
							break;
						}
						else
						{
							newWeapon = true;
						}
					}
					if(newWeapon)//Spawn a new weapon
					{
						upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); //Apply button functionality
						//Apply initial description and name
						upgradeOption.upgradeDescriptionDispaly.text = chosenWeaponUpgrade.weaponData.Description;
						upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
					}
					upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
				}
			}
			else if(upgradeType == 2)
			{
				PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];

				availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

				if(chosenPassiveItemUpgrade != null)
				{
					EnableUpgradeUI(upgradeOption);
					bool newPassiveItem = false;
					for (int i = 0; i < passiveItemSlots.Count; i++)
					{
						if(passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.PassiveItemData)
						{
							newPassiveItem = false;
							if(!newPassiveItem)
							{
								if(!chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab)
								{
									DisableUpgradeUI(upgradeOption);
									break;
								}
								upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex)); //Apply button functionality
								//Set the description and the name to be that of the next level passive item's name and description
								upgradeOption.upgradeDescriptionDispaly.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
								upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
							}
							break;
						}
						else
						{
							newPassiveItem = true;
							
						}
					}
					if(newPassiveItem == true)
					{
						upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem)); //Apply button functionality
						//Apply initial desciption and name
						upgradeOption.upgradeDescriptionDispaly.text = chosenPassiveItemUpgrade.PassiveItemData.Description;
						upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.PassiveItemData.Name;
					}
					upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.PassiveItemData.Icon;
				}
			}
		}
	}
	void RemoveUpgradeOptions()
	{
		foreach (var upgradeOption in upgradeOptions)
		{
			upgradeOption.upgradeButton.onClick.RemoveAllListeners();
			DisableUpgradeUI(upgradeOption); //Call the DisableUpgradeUI method to disable all UI options before applying upgrades to them
		}
	}
	public void RemoveAndApplyUpgrades()
	{
		RemoveUpgradeOptions();
		ApplyUpgradeOptions();
	}

	void DisableUpgradeUI(UpgradeUI ui)
	{
		ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
	}
	void EnableUpgradeUI(UpgradeUI ui)
	{
		ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
	}
}