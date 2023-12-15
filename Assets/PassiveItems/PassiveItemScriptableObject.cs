using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObject/PassiveItem")]
public class PassiveItemScriptableObject : ScriptableObject
{
	[SerializeField]
	float multiplier;
	public float Multiplier {get => multiplier; private set => multiplier = value; }
}
