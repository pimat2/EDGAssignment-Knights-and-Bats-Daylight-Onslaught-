using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for all weapons to inherit from
/// </summary>
public class WeaponController : MonoBehaviour
{
	[Header("Weapon Stats")]
	public GameObject weaponPrefab;
	public float damage;
	public float speed;
	public float cooldownDuration;
	float currentCooldown;
	public int pierce;
	
	protected PlayerMovement pm;
	
    // Start is called before the first frame update
	 protected virtual void Start()
	{
		pm = FindObjectOfType<PlayerMovement>();
	    currentCooldown = cooldownDuration; //Sets the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
	protected virtual void Update()
    {
	    currentCooldown -= Time.deltaTime;
	    if(currentCooldown <= 0f)
	    {
	    	Attack();
	    }
    }
	protected virtual void Attack()
	{
		currentCooldown = cooldownDuration;
	}
}
