/*
 * This is a debugger script 
 * 
 */

using UnityEngine;
using System.Collections;

public class GameStethoscope : MonoBehaviour
{
	public Vector3 mousePosition;
	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		mousePosition = Input.mousePosition;
	}
}
