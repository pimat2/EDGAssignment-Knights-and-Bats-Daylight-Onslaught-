using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for all projectile based weapons
/// </summary>
public class ProjectileWeaponBehavior : MonoBehaviour
{
	public WeaponScriptableObject weaponData;
	
	protected Vector3 direction;
	public float destroyAfterSeconds;
	
	//CurrentStats
	protected float currentDamage;
	protected float currentSpeed;
	protected float currentCooldownDuration;
	protected int currentPierce;
	
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentDamage = weaponData.Damage;
		currentSpeed = weaponData.Speed;
		currentCooldownDuration = weaponData.CooldownDuration;
		currentPierce = weaponData.Pierce;
	}
	
	public float GetCurrentDamage()
	{
		return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
	}
	
	protected virtual void Start()
    {
	    Destroy(gameObject, destroyAfterSeconds); 
    }

	public void DirectionChecker(Vector3 dir)
	{
		direction = dir;
		
		float dirX  = direction.x;
		float dirY = direction.y;
		
		Vector3 scale = transform.localScale;
		Vector3 rotation = transform.rotation.eulerAngles;
		
		if(dirX < 0 && dirY == 0) //left
		{
			scale.x = scale.x * -1;
			scale.y = scale.y * -1;
		}
		else if(dirX == 0 && dirY < 0) //down
		{
			scale.y = scale.y * -1;
		}
		else if(dirX == 0 && dirY > 0) //up
		{
			scale.x = scale.x * -1;
		}
		else if(dir.x > 0 && dir.y > 0) //right up
		{
			rotation.z = 0f;
		}
		else if(dir.x > 0 && dir.y < 0) //right down
		{
			rotation.z = -90f;
		}
		else if(dir.x < 0 && dir.y > 0) //left up
		{
			scale.x = scale.x * -1;
			scale.y = scale.y * -1;
			rotation.z = -90f;
		}
		else if(dir.x < 0 && dir.y < 0) //left down
		{
			scale.x = scale.x * -1;
			scale.y = scale.y * -1;
			rotation.z = 0f;
		}
		transform.localScale = scale;
		transform.rotation = Quaternion.Euler(rotation);
	}
	
	// Sent when another object enters a trigger collider attached to this object (2D physics only).
	protected virtual void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("Enemy"))
		{
			EnemyStats enemy = col.GetComponent<EnemyStats>();
			enemy.TakeDamage(GetCurrentDamage(), transform.position);
			ReducePierce();
		}
		else if(col.CompareTag("Prop"))
		{
			if(col.gameObject.TryGetComponent(out BreakableProps breakable))
			{
				breakable.TakeDamage(GetCurrentDamage());
				ReducePierce();
			}
		}
	}
	
	void ReducePierce()
	{
		currentPierce--;
		if(currentPierce <= 0)
		{
			Destroy(gameObject);
		}
	}
}
