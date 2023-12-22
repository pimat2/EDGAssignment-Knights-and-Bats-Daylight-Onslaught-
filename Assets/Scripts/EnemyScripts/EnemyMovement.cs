using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
	EnemyStats enemy;
	Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    // Start is called before the first frame update
    void Start()
	{
		enemy = GetComponent<EnemyStats>();
	    player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Process knockback if knockbackduration is greater than 0
        if(knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            //else move normally
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        }
	    
    }
    public void Knockback(Vector2 velocity, float duration)
    {
        //Ignore knockback if duration is greater than zero
        if(knockbackDuration > 0) return;
        //Begins knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
