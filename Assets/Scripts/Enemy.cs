using UnityEngine;
using System.Collections;

public interface Enemy
{
	//These are declarations of properties that will be implemented by all Classes that implement the Enemy interface
	Vector3 pos {get; set;} //The Enemy's transform.position
	float touchDamage {get; set;} //Damage done by touching the Enemy 

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
