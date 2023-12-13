using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
	// Sent when another object enters a trigger collider attached to this object (2D physics only).
	void OnTriggerEnter2D(Collider2D col)
	{
		//check if the other gameobject that the player has collided with has the iCollecible interface, if so calls the Collect() function on collided object
		if(col.TryGetComponent(out iCollectible collectible))
		{
			collectible.Collect();
		}
	}
}
