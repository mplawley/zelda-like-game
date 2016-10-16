using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpiker : PT_MonoBehaviour //NOT Monobehaviour
{
	public float speed = 5f;
	public string roomXMLString = "{";

	[Header("-------------------")]

	public Vector3 moveDir;
	public Transform characterTrans;
	private Rigidbody rb;

	void Awake()
	{
		characterTrans = transform.Find("CharacterTrans");
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		//Set the move direction based on the character in Rooms.xml
		switch (roomXMLString)
		{
		case "^":
			moveDir = Vector3.up;
			break;
		case "v":
			moveDir = Vector3.down;
			break;
		case "{":
			moveDir = Vector3.left;
			break;
		case "}":
			moveDir = Vector3.right;
			break;
		}
	}

	void FixedUpdate() //Happens every physics steps, i.e., 50 times/second
	{
		rb.velocity = moveDir * speed;	
	}

	//This has the same structure as the Damage Method in EnemyBug
	public void Damage(float amt, ElementType eT, bool damageOverTime = false)
	{
		//Nothing damages the enemy spiker
	}

	void OnTriggerEnter(Collider other)
	{
		//Check to see if a wall was hit
		GameObject go = Utils.FindTaggedParent(other.gameObject);

		//In case nothing is tagged
		if (go == null)
		{
			return;
		}

		if (go.tag == "Ground")
		{
			//Make sure that the ground tile is in the direction we're moving.
			//A dot product will help us with this.
			float dot = Vector3.Dot(moveDir, go.transform.position - pos);
			if (dot > 0) //If the spiker is moving towards the block it hit
			{
				moveDir *= -1; //Reverse direction
			}
		}
	}
}
