using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public bool ClearLog = false;
	public bool LogItems = true;
	public InputField ItemSelect;

	public static void ReloadDB (bool logitems, bool clearlog) {
		ItemDB.GetAllWeapons (logitems, clearlog);
	}

	public void ReloadDB () {
		ItemDB.GetAllWeapons (LogItems, ClearLog);
	}

	void Awake () {
		DontDestroyOnLoad (gameObject);
		ReloadDB ();
	}

	void Update () {
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.F1)) {
			Debug.Log ("[Ctrl+F1] Reloading database...");
			ReloadDB();
		}
	}

	public void UpdateHeldItem() {
		if (ItemSelect && ItemSelect.text != "" && ItemDB.Weapons.Count > int.Parse(ItemSelect.text)) {
			Player.Punch.heldItem = ItemDB.Weapons[int.Parse (ItemSelect.text)];
			if(Player.Punch.itemIcons[Mathf.Clamp (Player.Punch.heldItem.ID, 0, Player.Punch.itemIcons.Count - 1)].GetComponent<SpellCast>())
				Player.Punch.itemIcons[Mathf.Clamp (Player.Punch.heldItem.ID, 0, Player.Punch.itemIcons.Count - 1)].GetComponent<SpellCast>().SpellText.text = "";
		}
	}
}
