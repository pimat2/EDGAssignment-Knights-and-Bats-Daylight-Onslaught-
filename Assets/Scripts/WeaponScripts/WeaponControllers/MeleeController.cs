using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : WeaponController
{
    // Start is called before the first frame update
	protected override void Start()
    {
	    base.Start();
    }

	protected override void Attack()
	{
		base.Attack();
		GameObject spawnedMelee = Instantiate(weaponPrefab);
		spawnedMelee.transform.position = transform.position;
		spawnedMelee.transform.parent = transform;
	}

}
