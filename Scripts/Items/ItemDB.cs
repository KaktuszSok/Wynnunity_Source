using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDB {
	public static List<Item_Weapon> Weapons = new List<Item_Weapon>();
	private static string[] WeaponLines;

	public static void GetAllWeapons(bool logItems = true, bool clearLog = true) {
		Weapons.Clear ();
		if(clearLog && Application.isEditor)
			System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll").GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke (null,null);
		if(Application.isEditor)
			WeaponLines = System.IO.File.ReadAllLines("Wynnunity_Build/Data/Weapons.txt");
		else
			WeaponLines = System.IO.File.ReadAllLines("Data/Weapons.txt");
		for (int i = 0; i < WeaponLines.Length;) {
			Item_Weapon weapon = new Item_Weapon();
			weapon.name = WeaponLines[i++];
			weapon.ID = Weapons.Count;
			weapon.dmgMin = int.Parse(WeaponLines[i++]);
			weapon.dmgMax = int.Parse (WeaponLines[i++]);
			weapon.range = float.Parse (WeaponLines[i++]);
			weapon.knockback = float.Parse (WeaponLines[i++]);
			weapon.hitCooldown = float.Parse (WeaponLines[i++]);
			Weapons.Add (weapon);
			if(logItems)
				Debug.Log (weapon.name + " {MinDmg: " + weapon.dmgMin +
				           ", MaxDmg: " + weapon.dmgMax +
				           ", range: " + weapon.range +
				           ", knockback: " + weapon.knockback +
				           ", hitCooldown: " + weapon.hitCooldown + "}");
		}
	}
}
