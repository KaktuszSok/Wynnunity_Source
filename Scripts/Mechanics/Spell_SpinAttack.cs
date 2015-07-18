using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_SpinAttack : Spell {

	public Health health;
	public float dmgMult = 1.5f;
	public float knockback = 7;
	public List<Health> updatedHits = new List<Health>();
	public bool doneCasting = true;
	public Vector2 knockMultipliers = Vector2.one;
	public ParticleSystem FX;

	void Start() {

	}

	void Update() {
		if (health.health == 0) {
			updatedHits.Clear();
		}
		for (int i = 0; i < updatedHits.Count; i++) {
			if(updatedHits[i] == null)
				updatedHits.RemoveAt(i);
		}
	}
	public IEnumerator CastSpell(Item_Weapon Weapon) {
		transform.SetParent (null);
		doneCasting = false;
		Item_Weapon adjustedWeapon = new Item_Weapon (Weapon.name, (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMin * dmgMult), 1, Mathf.Infinity), (int) Mathf.Clamp (Mathf.RoundToInt(Weapon.dmgMax * dmgMult), 1, Mathf.Infinity), Weapon.range, knockback, 0);
		List<Health> hitObjs = new List<Health> ();
		FX.Play ();
			foreach (Health hit in updatedHits) {
			if (hit != health && hit.health != 0 && !hitObjs.Contains (hit)) {
				if (!transform.root.GetComponent<Enemy> () && hit.transform.root.GetComponent<Enemy> () ||
				    transform.root.GetComponent<Enemy> () && !hit.transform.root.GetComponent<Enemy> ()) {
						DealDamage (hit, adjustedWeapon, knockMultipliers, Weapon.IDs, true);
						hitObjs.Add (hit);
				}
			}
		}
		yield return new WaitForEndOfFrame ();
		doneCasting = true;
		updatedHits.Clear ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.GetComponent<Health> ()) {
			if(col.GetComponent<Health>() != health)
			{
				updatedHits.Add (col.GetComponent<Health>());
			}
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.GetComponent<Health> ()) {
			if(updatedHits.Contains (col.GetComponent<Health>()))
			{
				updatedHits.Remove (col.GetComponent<Health>());
			}
		}
	}
}