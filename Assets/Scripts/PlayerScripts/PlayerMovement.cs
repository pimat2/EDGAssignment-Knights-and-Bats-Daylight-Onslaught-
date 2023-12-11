﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	
	[HideInInspector]
	public float lastHorizontalVector;
	[HideInInspector]
	public float lastVerticalVector;
	[HideInInspector]
	public Vector2 moveDir;
	[HideInInspector]
	public Vector2 lastMovedVector;
    // Start is called before the first frame update
    
	Rigidbody2D rb;
	public CharacterScriptableObject characterData;
	
	void Start()
    {
	    rb = GetComponent<Rigidbody2D>();
	    lastMovedVector = new Vector2(1, 0f);
    }

    // Update is called once per frame
    void Update()
    {
	    InputManagement();
    }
    
	// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	void FixedUpdate()
	{
		Move();
	}
    
	void InputManagement()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		
		moveDir = new Vector2(moveX, moveY).normalized;
		
		if(moveDir.x != 0)
		{
			lastHorizontalVector = moveDir.x;
			lastMovedVector = new Vector2(lastHorizontalVector, 0f); //last moved X
		}
		if(moveDir.y != 0)
		{
			lastVerticalVector = moveDir.y;
			lastMovedVector = new Vector2(lastVerticalVector, 0f);// last moved Y
		}
		if(moveDir.x != 0 && moveDir.y != 0)
		{
			lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
		}
	}
	
	void Move()
	{
		rb.velocity = new Vector2 (moveDir.x * characterData.MoveSpeed, moveDir.y * characterData.MoveSpeed);
	}
}
