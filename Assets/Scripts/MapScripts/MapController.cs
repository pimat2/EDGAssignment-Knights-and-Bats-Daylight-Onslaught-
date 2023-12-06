using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
	public List<GameObject> terrainChunks;
	public GameObject player;
	public float checkerRadius;
	Vector3 noTerrainPosition;
	public LayerMask terraiMask;
	public GameObject currentChunk;
	PlayerMovement pm;
	
	[Header("Optimization")]
	public List<GameObject> spawnedChunks;
	public GameObject latestChunk;
	public float maxOpsDistance; //must be higher and wider than the size of the tilemap
	float opDistance;
	float optimizerCooldown;
	public float optimizerCooldownDuration;
    // Start is called before the first frame update
    void Start()
    {
	    pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
	    ChunkChecker();
	    ChunkOptimizer();
	    
    }
    
	void ChunkChecker()
	{
		if(!currentChunk)
		{
			return;
		}
		if(pm.moveDir.x > 0 && pm.moveDir.y == 0) //right
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(20,0,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x < 0 && pm.moveDir.y == 0) //left
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(-20,0,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x == 0 && pm.moveDir.y > 0) //up
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Top").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(0,20,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x == 0 && pm.moveDir.y < 0) //down
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Bottom").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(0,-20,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x > 0 && pm.moveDir.y > 0) //right up
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("TopRight").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(20,20,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x > 0 && pm.moveDir.y < 0) //rigt down
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("BottomRight").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(20,-20,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x < 0 && pm.moveDir.y > 0) //left up
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("TopLeft").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(-20,20,0);
				SpawnChunk();	
			}
		}
		else if(pm.moveDir.x < 0 && pm.moveDir.y < 0) //left down
		{
			if(!Physics2D.OverlapCircle(currentChunk.transform.Find("BottomLeft").position, checkerRadius, terraiMask))
			{
				noTerrainPosition = player.transform.position + new Vector3(-20,-20,0);
				SpawnChunk();	
			}
		}
		
	}
	
	void SpawnChunk()
	{
		int rand = Random.Range(0, terrainChunks.Count);
		latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
		spawnedChunks.Add(latestChunk);
	}
	
	void ChunkOptimizer()
	{
		optimizerCooldown -= Time.deltaTime;
		if(optimizerCooldown <= 0f)
		{
			optimizerCooldown = optimizerCooldownDuration;
		}
		else{
			return;
		}
		foreach (GameObject chunk in spawnedChunks)
		{
			opDistance = Vector3.Distance(player.transform.position, chunk.transform.position);
			if(opDistance > maxOpsDistance)
			{
				chunk.SetActive(false);
			}
			else
			{
				chunk.SetActive(true);	
			}
		}
	}
}
