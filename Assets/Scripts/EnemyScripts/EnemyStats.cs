using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	
	//CurrentStats
	[HideInInspector]
	public float currentMoveSpeed;
	[HideInInspector]
	public float currentHealth;
	[HideInInspector]
	public float currentDamage;
	
	public float despawnDistance = 20f;
	Transform player;

	[Header("Damage Feedback")]
	public Color damageColor = new Color(1, 0, 0, 1);//Color of the damage flash
	public float damageFlashDuration = 0.2f;//How long the flash should last
	public float deathFadeTime = 0.6f;//Time for enemy to fade away
	Color originalColor;
	SpriteRenderer sr;
	EnemyMovement movement;
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		currentMoveSpeed = enemyData.MoveSpeed;
		currentHealth = enemyData.MaxHealth;
		currentDamage = enemyData.Damage;
	}
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	void Start()
	{
		player = FindObjectOfType<PlayerStats>().transform;
		sr = GetComponent<SpriteRenderer>();
		originalColor = sr.color;
		movement = GetComponent<EnemyMovement>();
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	void Update()
	{
		if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
		{
			ReturnEnemy();
		}
	}
	
	public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
	{
		currentHealth -= dmg;
		StartCoroutine(DamageFlash());
		//Apply knockback if its not zero
		if(knockbackForce > 0)
		{
			//Gets the direction of knockback
			Vector2 dir = (Vector2)transform.position - sourcePosition;
			movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
		}
		//Kills enemy if health is less than zero
		if(currentHealth <= 0)
		{
			Kill();
		}
	}
	IEnumerator DamageFlash()
	{
		sr.color = damageColor; 
		yield return new WaitForSeconds(damageFlashDuration);
		sr.color = originalColor;
	}
	public void Kill()
	{
		StartCoroutine(KillFade());
	}
	IEnumerator KillFade()
	{
		WaitForEndOfFrame w = new WaitForEndOfFrame();
		float t = 0, 
		origAlpha = sr.color.a;
		//While loop that fires every frame reducing the opacity of the sprite
		while(t < deathFadeTime)
		{
			yield return w;
			t += Time.deltaTime;

			//set the color for this frame
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1-t / deathFadeTime) * origAlpha);
		}

		Destroy(gameObject);
	}
	
	// OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
	private void  OnCollisionStay2D(Collision2D col)
	{
		
		//references the player stats script when collision happens and calls the function of TakeDamage there
		if(col.gameObject.CompareTag("Player"))
		{
			PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
			player.TakeDamage(currentDamage);
		}
	}
	
	// This function is called when the MonoBehaviour will be destroyed.
	private void OnDestroy()
	{
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		enemySpawner.OnEnemyKilled();
	}
	
	void ReturnEnemy()
	{
		EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
		transform.position = player.position + enemySpawner.relativeSpawnPoints[Random.Range(0,enemySpawner.relativeSpawnPoints.Count)].position;
	}
}
