using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_Weapon : Item {

	public int dmgMin;
	public int dmgMax;
	public float range;
	public float knockback;
	public float hitCooldown;

	public IDInfo IDs;

	public Item_Weapon (string itemname = "", int itemdmgMn = 1, int itemdmgMx = 1, float itemrange = 4, float itemknockback = 4.5f, float itemcooldown = 0.3f)
	{
		name = itemname;
		dmgMin = itemdmgMn;
		dmgMax = itemdmgMx;
		range = itemrange;
		knockback = itemknockback;
		hitCooldown = itemcooldown;
	}

	public float GetDmg() {
		return Random.Range (dmgMin, dmgMax + 1);
	}
}
