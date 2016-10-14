using UnityEngine;
using System.Collections;

public class ElementInventoryButton : MonoBehaviour
{
	public ElementType type;

	void Awake()
	{
		//This could easily be done in the Inspector, but just for variation and funsies, do it in code this way...
		//Parse the first character of the name of this GameObject into an int
		char c = gameObject.name[0];
		string s = c.ToString();
		int typeNum = int.Parse(s);

		//typecast that int to an ElementType
		type = (ElementType) typeNum;
	}

	void OnMouseUpAsButton()
	{
		//Tell the Mage to add this element type
		Mage.S.SelectElement(type);
	}

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
