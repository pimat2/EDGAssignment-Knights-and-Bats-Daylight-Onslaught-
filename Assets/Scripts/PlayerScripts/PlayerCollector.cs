using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
	PlayerStats player;
	CircleCollider2D playerCollector;
	public float pullSpeed;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	void Start()
	{
		player = FindObjectOfType<PlayerStats>();
		playerCollector = GetComponent<CircleCollider2D>();
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	void Update()
	{
		playerCollector.radius = player.CurrentMagnet;	
	}
	// Sent when another object enters a trigger collider attached to this object (2D physics only).
	void OnTriggerEnter2D(Collider2D col)
	{
		//check if the other gameobject that the player has collided with has the iCollecible interface, if so calls the Collect() function on collided object
		if(col.TryGetComponent(out iCollectible collectible))
		{
			//
			Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
			//Vecto2 to point from the item to the playe
			Vector2 forceDirection = (transform.position - col.transform.position).normalized;
			//Applies force to the item in the forceDirection with the pullSpeed
			rb.AddForce(forceDirection * pullSpeed * Time.deltaTime * 100);
			
			collectible.Collect();
		}
	}
}
