using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpellComboManager : MonoBehaviour {

	public Vector2 ClickCombo;
	public List<SpellCast> Spells;
	public float retryTime;
	public int comboPlace;
	public Player player;
	public Text SpellText;
	private float minButtonCheckTime;
	public bool RMB = false;
	public bool LMB = false;
	public List<bool> CombosDone = new List<bool>();
	
	void Start() {
		if (!player)
			player = transform.root.GetComponent<Player> ();
	}
	
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			LMB = true;
		} else {
			LMB = false;
		}
		
		if (Input.GetMouseButton (1) && Time.time >= minButtonCheckTime) {
			RMB = true;
		} else {
			RMB = false;
		}
		
		if (Time.time > retryTime && comboPlace != 0) {
			comboPlace = 0;
			if(!SpellText.text.Contains ("!"))
				SpellText.text = "";
		}
		
		if (gameObject.activeInHierarchy) {

			CombosDone.Clear ();
			foreach(SpellCast spell in Spells)
			{
				CombosDone.Add (false);
			}

			for(int i = 0; i < Spells.Count; i++) {
				if(Spells[i].combo == ClickCombo) {
					CombosDone[i] = true;
				} else {
					CombosDone[i] = false;
				}
			}

			if (LMB && Time.time < retryTime && comboPlace == 2) {
				minButtonCheckTime = Time.time + 0.1f;
				ClickCombo.y = 0;
				SpellText.text += "-Left";
				comboPlace = 3;
			}
			if (RMB && Time.time < retryTime && comboPlace == 2) {
				minButtonCheckTime = Time.time + 0.1f;
				ClickCombo.y = 1;
				SpellText.text += "-Right";
				comboPlace = 3;
			}
			if(comboPlace == 3){
				bool failedCombos = true;
				foreach(bool combo in CombosDone) {
					if(combo == true) {
						Spells[CombosDone.IndexOf(combo)].StartCoroutine (Spells[CombosDone.IndexOf(combo)].Cast());
						failedCombos = false;
					}
				}
				comboPlace = 0;
				if(failedCombos) {
					SpellText.text = "";
					retryTime = Time.time + 0.25f;
				}
			}

			if (LMB && Time.time < retryTime && comboPlace == 1) {
				retryTime = Time.time + 1;
				minButtonCheckTime = Time.time + 0.1f;
				comboPlace = 2;
				ClickCombo.x = 0;
				SpellText.text += "-Left";
			}
			if (RMB && Time.time < retryTime && comboPlace == 1) {
				retryTime = Time.time + 1;
				minButtonCheckTime = Time.time + 0.1f;
				comboPlace = 2;
				ClickCombo.x = 1;
				SpellText.text += "-Right";
			}
			
			if (RMB && Time.time > retryTime && comboPlace == 0) {
				retryTime = Time.time + 1;
				minButtonCheckTime = Time.time + 0.1f;
				comboPlace = 1;
				SpellText.text = "Right";
			}
		}
	}
}
