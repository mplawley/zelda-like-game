using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Mage : PT_MonoBehaviour //NOT MonoBehaviour
{
	public static Mage S;

	void Awake()
	{
		S = this; //Mage Singleton
	}
}
