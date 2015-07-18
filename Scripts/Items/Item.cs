using UnityEngine;
using System.Collections;

public class Item {

	public string name;
	public int ID = -1;

	public Item()
	{
		name = "";
	}

	public Item(string itemName)
	{
		name = itemName;
	}
}
