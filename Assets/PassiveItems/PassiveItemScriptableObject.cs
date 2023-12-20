using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObject/PassiveItem")]
public class PassiveItemScriptableObject : ScriptableObject
{
	[SerializeField]
	float multiplier;
	public float Multiplier {get => multiplier; private set => multiplier = value; }
	[SerializeField]
	int level;
	public int Level { get => level; private set => level = value; }
	[SerializeField]
	GameObject nextLevelPrefab;
	public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }
	[SerializeField]
	new string name;
	public string Name { get => name; private set => name = value; }
	[SerializeField]
	string description; //What is the description of this passive item?(if passive item is an upgrade, place the description of the upgrade)
	public string Description { get => description; private set => description = value; }
	[SerializeField]
	Sprite icon;
	public Sprite Icon { get => icon; private set => icon = value; }
}
