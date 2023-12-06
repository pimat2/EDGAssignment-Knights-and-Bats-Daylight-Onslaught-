using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
	MapController mc;
	
	public GameObject targetMap;
    // Start is called before the first frame update
    void Start()
    {
	    mc = FindObjectOfType<MapController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	// Sent each frame where another object is within a trigger collider attached to this object (2D physics only).
	private void OnTriggerStay2D(Collider2D col)
	{
		if(col.CompareTag("Player"))
		{
			mc.currentChunk = targetMap;
		}
	}
	// Sent when another object leaves a trigger collider attached to this object (2D physics only).
	private void OnTriggerExit2D(Collider2D col)
	{
		if(col.CompareTag("Player"))
		{
			if(mc.currentChunk == targetMap)
			{
				mc.currentChunk = null;
			}
		}
	}
}
