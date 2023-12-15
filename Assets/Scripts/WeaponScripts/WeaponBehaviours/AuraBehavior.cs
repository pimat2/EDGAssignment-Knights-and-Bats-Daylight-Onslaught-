using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBehavior : MeleeWeaponBehavior
{
	List<GameObject> markedEnemies;
    // Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		markedEnemies = new List<GameObject>();
	}
	
	// Sent when another object enters a trigger collider attached to this object (2D physics only).
	protected override void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
		{
			EnemyStats enemy = col.GetComponent<EnemyStats>();
			enemy.TakeDamage(GetCurrentDamage());
			
			markedEnemies.Add(col.gameObject);//adds damaged enemy in list that prevents multiple triggers of the collider
		}
		else if(col.CompareTag("Prop"))
		{
			if(col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
			{
				breakable.TakeDamage(GetCurrentDamage());
				markedEnemies.Add(col.gameObject);
			}
		}
	}
}
