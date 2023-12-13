using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
	protected void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
