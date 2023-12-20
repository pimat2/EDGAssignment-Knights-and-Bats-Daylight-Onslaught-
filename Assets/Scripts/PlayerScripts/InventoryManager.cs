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
		public GameObject initialWeapon;
		public WeaponScriptableObject weaponData;
	}
	[System.Serializable]
	public class PassiveItemUpgrade
	{
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
	
	public void LevelUpWeapon(int slotIndex)
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

			if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
			{
				GameManager.instance.EndLevelUp();
			}
		}
	}
	
	public void LevelUpPassiveItem(int slotIndex)
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

			if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
			{	
				GameManager.instance.EndLevelUp();
			}
		}
	}
	void ApplyUpgradeOptions()
	{
		foreach (var upgradeOption in upgradeOptions)
		{
			int upgradeType = Random.Range(1, 3);//Choose between weapon and passive item
			if(upgradeType == 1)
			{
				WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];

				if(chosenWeaponUpgrade != null)
				{
					bool newWeapon = false;
					for (int i = 0; i < weaponSlots.Count; i++)
					{
						if(weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
						{
							newWeapon = false;
							if(!newWeapon)
							{
								upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i)); //Apply button funcitonality
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
				PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[Random.Range(0, passiveItemUpgradeOptions.Count)];

				if(chosenPassiveItemUpgrade != null)
				{
					bool newPassiveItem = false;
					for (int i = 0; i < passiveItemSlots.Count; i++)
					{
						if(passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.PassiveItemData)
						{
							newPassiveItem = false;
							if(!newPassiveItem)
							{
								upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i)); //Apply button functionality
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
		}
	}
	public void RemoveAndApplyUpgrades()
	{
		RemoveUpgradeOptions();
		ApplyUpgradeOptions();
	}
}