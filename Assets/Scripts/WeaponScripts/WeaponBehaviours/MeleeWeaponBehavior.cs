using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for all melee weapon behaviors
/// </summary>
public class MeleeWeaponBehavior : MonoBehaviour
{
	public float destroyAfterSeconds;
	
    // Start is called before the first frame update
	protected virtual void Start()
    {
	    Destroy(gameObject, destroyAfterSeconds); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
