using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemPanel : MonoBehaviour {

	public Item weapon;
	private Text text;
	// Use this for initialization
	void Start () {
		weapon = Player.Punch.heldItem;
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		weapon = Player.Punch.heldItem;
		if (weapon is Item_Weapon) {
			Item_Weapon wpn = (Item_Weapon)weapon;
			text.text = "<b>Item Statistics:</b>\n" +
				"<color=yellow>" + wpn.name + "</color>\n" +
				"<color=purple>Dam: " + wpn.dmgMin + "-" + wpn.dmgMax + "</color>\n" +
				"<color=red>Range: " + wpn.range + "</color>\n" +
				"<color=navy>Knockback: " + wpn.knockback + "</color>\n" +
				"<color=grey>Cooldown: " + wpn.hitCooldown + "s</color>";
		} else {
			text.text = "<b>Item Statistics:</b>\n" +
				"<color=yellow>" + weapon.name + "</color>\n";
		}
	}
}
