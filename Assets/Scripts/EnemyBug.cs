using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBug : PT_MonoBehaviour //NOT Monobehaviour
{
	public float speed = 0.5f;
	public float health = 10;

	[Header("-----------")]

	private float _maxHealth;
	public Vector3 walkTarget;
	public bool walking;
	public Transform characterTrans;
	Rigidbody rb;

	void Awake()
	{
		characterTrans = transform.Find("CharacterTrans");
		_maxHealth = health; //Used to put a top cap on healing
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
		
	void Update()
	{
		WalkTo(Mage.S.pos);
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
	public void Damage(float amt, bool damageOverTime = false)
	{
		//If it's DOT, then only damage the fractional amount for this frame
		if (damageOverTime)
		{
			amt *= Time.deltaTime;
		}

		health -= amt;
		health = Mathf.Min(_maxHealth, health); //Limit health if healing

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
