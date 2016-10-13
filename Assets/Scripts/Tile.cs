﻿using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	//public fields
	public string type;

	//private fields
	private string _tex;
	private int _height = 0;
	private Vector3 _pos;

	//Properties
	//heigh moves the Tile up or down. Walls have height = 1
	public int height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
			AdjustHeight();
		}
	}

	//Sets the texture of the Tile based on a string. It requires LayoutTiles
//	public string tex
//	{
//		get
//		{
//			return _tex;
//		}
//		set
//		{
//			_tex = value;
//			name = "TilePrefab_" + _tex; //Sets the name of this GameObject
//			Texture2D t2D = LayoutTiles.S.GetTileTex(_tex);
//
//			if (t2D == null)
//			{
//				Utils.tr("ERROR", "Tile.type{set}=", value, "No matching Texture2D in LayoutTiles.S.tileTextures!");
//			}
//			else
//			{
//				GetComponent<Renderer>().material.mainTexture = t2D;
//			}
//		}
//	}

	//Methods
	public void AdjustHeight()
	{
		//Moves the block up or down based on _height
		Vector3 vertOffset = Vector3.back * (_height - 0.5f); //The -0.5f shifts the Tile down 0.5 units so that it's top surface is
		//at z = 0 when pos.z = 0 and height = 0
		transform.position = _pos + vertOffset;
	}
}
