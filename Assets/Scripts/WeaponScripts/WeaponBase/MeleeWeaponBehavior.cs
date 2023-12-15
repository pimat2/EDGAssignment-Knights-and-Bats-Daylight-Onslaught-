using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for all melee weapon behaviors
/// </summary>
public class MeleeWeaponBehavior : MonoBehaviour
{
	public WeaponScriptableObject weaponData;
	public float destroyAfterSeconds;
	
	
	protected float currentDamage;
	protected float currentSpeed;
	protected float currentCooldownDuration;
	protected int currentPierce;
	
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentPierce = weaponData.Pierce;
		currentDamage = weaponData.Damage;
		currentSpeed = weaponData.Speed;
		currentCooldownDuration = weaponData.CooldownDuration;
	}
	
	public float GetCurrentDamage()
	{
		return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
	}
	
    // Start is called before the first frame update
	protected virtual void Start()
    {
	    Destroy(gameObject, destroyAfterSeconds); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	// Sent when another object enters a trigger collider attached to this object (2D physics only).
	protected virtual void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("Enemy"))
		{
			EnemyStats enemy = col.GetComponent<EnemyStats>();
			enemy.TakeDamage(GetCurrentDamage());
		}
		else if(col.CompareTag("Prop"))
		{
			if(col.gameObject.TryGetComponent(out BreakableProps breakable))
			{
				breakable.TakeDamage(GetCurrentDamage());
			}
		}
	}
}
