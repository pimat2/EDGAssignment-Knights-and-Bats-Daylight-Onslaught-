using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
	protected PlayerStats player;
	public PassiveItemScriptableObject passiveItemData;
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	void Start()
	{
		player = FindObjectOfType<PlayerStats>();	
		ApplyModifier();
	}
	protected virtual void ApplyModifier()
	{
			
	}
}
