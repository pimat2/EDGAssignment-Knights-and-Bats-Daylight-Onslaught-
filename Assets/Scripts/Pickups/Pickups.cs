using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour, iCollectible
{
	protected bool hasBeenCollected;

    public virtual void Collect()
    {
		hasBeenCollected = true;
    }

    protected void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
