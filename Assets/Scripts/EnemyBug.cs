using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBug : PT_MonoBehaviour //NOT Monobehaviour
{
	public float speed = 0.5f;
	public float health = 10;
	public float damageScale = 0.8f;
	public float damageScaleDuration = 0.25f;

	[Header("-----------")]

	private float damageScaleStartTime;
	private float _maxHealth;
	public Vector3 walkTarget;
	public bool walking;
	public Transform characterTrans;
	Rigidbody rb;

	//Stores damage for each element each frame
	public Dictionary<ElementType, float> damageDict;

	void Awake()
	{
		characterTrans = transform.Find("CharacterTrans");
		_maxHealth = health; //Used to put a top cap on healing
		ResetDamageDict();
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
		
	void Update()
	{
		WalkTo(Mage.S.pos);
	}

	void ResetDamageDict()
	{
		if (damageDict == null)
		{
			damageDict = new Dictionary<ElementType, float>();
		}
		damageDict.Clear();
		damageDict.Add(ElementType.earth, 0);
		damageDict.Add(ElementType.water, 0);
		damageDict.Add(ElementType.air, 0);
		damageDict.Add(ElementType.fire, 0);
		damageDict.Add(ElementType.aether, 0);
		damageDict.Add(ElementType.none, 0);
	}

/* ======================================================================
 * ======================================================================
 * ==========================WALKING CODE================================
 * ======================================================================
 * ======================================================================
 */

	//This is adapted directly from Mage

	//Walk to a s specific position. The position.z is always 0
	public void WalkTo(Vector3 xTarget)
	{
		walkTarget = xTarget; //Set the point to walk to
		walkTarget.z = 0; //Force z = 0
		walking = true; //Now the EnemyBug is walking
		Face(walkTarget); //Look in the direction of the walkTarget
	}

	//Face towards the point of interest
	public void Face(Vector3 poi)
	{
		Vector3 delta = poi - pos; //Find vector to the point of interest

		//Use Atan2 to get the rotation around Z that points the X-axis f 
		//EnemyBug:CharacterTrans towards poi

		float rZ = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);

		//Set the rotation of characterTrans (doesn't actually rotate Enemy)
		characterTrans.rotation = Quaternion.Euler(0, 0, rZ);
	}

	public void StopWalking() //Stops the EnemyBug from walking
	{
		walking = false;
		rb.velocity = Vector3.zero;
	}

	void FixedUpdate() //Happens every physics step (i.e., 50 times/second)
	{
		if (walking) //If EnemyBug is walking
		{
			if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime)
			{
				//If enemyBug is very close to walkTarget, just stop there
				pos = walkTarget;
				StopWalking();
			}
			else
			{
				//Otherwise, move towards walkTarget
				rb.velocity = (walkTarget - pos).normalized * speed;
			}
		}
		else
		{
			//If not walking, velocity should be zero
			rb.velocity = Vector3.zero;
		}
	}

	//Damage this instance. Bu default, the damage is instant, but it can also be treated as damage over time,
	//where the amt value would be the amount of damage done every second. Note: We can also use this code to heal the instance
	public void Damage(float amt, ElementType eT, bool damageOverTime = false)
	{
		//If it's DOT, then only damage the fractional amount for this frame
		if (damageOverTime)
		{
			amt *= Time.deltaTime;
		}

		//Treat different damage types differently (most are default)
		switch (eT)
		{
		case ElementType.fire:
			//Only the max damage from one fire source affects this instance
			damageDict[eT] = Mathf.Max(amt, damageDict[eT]);
			break;
		case ElementType.air:
			//air doesn't damage EnemyBugs, so do nothing
			break;
		default: 
			//By default, damage is added to the other damage by the same element
			damageDict[eT] += amt;
			break;
		}
	}

	//Once all the Updates() on all instances have been called, then LateUpdate() is called on all instances. LateUpdate() is still
	//called by Unity automatically every frame, though.
	void LateUpdate()
	{
		//Apply damage from the different element types

		//Iteration through a Dictionary uses a KeyValuePair
		//entry.Key is the ElementType, while entry.Value is the float
		float dmg = 0;

		foreach (KeyValuePair<ElementType, float> entry in damageDict)
		{
			dmg += entry.Value;
		}

		if (dmg > 0) //If this took damage...
		{
			//and if it is at full scale now (& not already damage scaling)...
			if (characterTrans.localScale == Vector3.one)
			{
				//Start the damage scale animation
				damageScaleStartTime = Time.time;
			}
		}

		//The damage scale animation
		float damU = (Time.time - damageScaleStartTime) / damageScaleDuration;
		damU = Mathf.Min(1, damU); //Limit the max localScale to 1
		float scl = (1 - damU) * damageScale + damU * 1;
		characterTrans.localScale = scl * Vector3.one;

		health -= dmg;
		health = Mathf.Min(_maxHealth, health); //Limit health if healing

		ResetDamageDict(); //Prepare for next frame

		if (health <= 0)
		{
			Die();
		}
	}

	//TODO: Different death animations, drop loot for player, etc.
	public void Die()
	{
		Destroy(gameObject);
	}
}
