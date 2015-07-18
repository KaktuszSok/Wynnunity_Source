using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell_Multihit : Spell {

	public Health health;
	public int hitAmount = 10;
	public float dmgPerHit = 0.25f;
	public float knockback = 1.25f;
	public float finalKnockback = 15.25f;
	public float hitDelay = 0.25f;
	public List<Health> updatedHits = new List<Health>();
	public bool doneCasting = true;
	public Vector2 knockMultipliers = Vector2.one;
	public Vector2 knockMultipliersFinal = Vector2.one;
	public float DynamicFinalXKnock = 0.9f;
	public GameObject ExplosionFX;
	public ParticleSystem CastFX;

	void Update() {
		if (health.health == 0) {
			updatedHits.Clear();
		}
		for (int i = 0; i < updatedHits.Count; i++) {
			if(updatedHits[i] == null || updatedHits[i].health == 0)
				updatedHits.RemoveAt(i);
		}
		if (!CastFX) {
			CastFX = GetComponentInChildren<ParticleSystem>();
		}
	}
	public IEnumerator CastSpell(Item_Weapon Weapon) {
		doneCasting = false;
		CastFX.Play ();
		Item_Weapon adjustedWeapon = new Item_Weapon (Weapon.name, (int) Mathf.Clamp (Weapon.dmgMin * dmgPerHit, 0, Mathf.Infinity), (int) Mathf.Clamp (Weapon.dmgMax * dmgPerHit, 0, Mathf.Infinity), Weapon.range, knockback, 0);
		List<Health> Hits = new List<Health> (updatedHits);
		for(int i = 0; i < hitAmount; i++)
		{
			foreach (Health hit in Hits) {
				if(hit != health && hit.health != 0)
				{
					if(transform.root.GetComponent<Enemy>() && !hit.transform.root.GetComponent<Enemy>() ||
					   !transform.root.GetComponent<Enemy>() && hit.transform.root.GetComponent<Enemy>()){
							Instantiate (ExplosionFX, hit.transform.position - Vector3.up*1.375f, transform.rotation);
							DealDamage(hit, adjustedWeapon, knockMultipliers, Weapon.IDs);
							hit.GetComponent<Rigidbody>().AddForce (transform.right*-Mathf.Clamp (getDisplacementX(hit.transform)*0.1f, -0.2f, 0.2f)*adjustedWeapon.knockback, ForceMode.VelocityChange);
					}
				}
			}
			if(Hits.Count > 0)
			{
				yield return new WaitForSeconds(hitDelay);
			}
			GameObject[] tempFX = GameObject.FindGameObjectsWithTag ("TempFX_MH");
			foreach(GameObject PS in tempFX) {
				Destroy (PS);
			}
		}
		if(Hits.Count > 0)
			yield return new WaitForSeconds (0.5f);
		adjustedWeapon.dmgMin = Weapon.dmgMin;
		adjustedWeapon.dmgMax = Weapon.dmgMax;
		adjustedWeapon.knockback = finalKnockback;
		foreach (Health hit in Hits) {
			if(hit != health && hit.health != 0)
			{
				if(transform.root.GetComponent<Enemy>() && !hit.transform.root.GetComponent<Enemy>() ||
				   !transform.root.GetComponent<Enemy>() && hit.transform.root.GetComponent<Enemy>()){
					Instantiate (ExplosionFX, hit.transform.position - Vector3.up*1.375f, transform.rotation);
					Vector2 tempKMF = knockMultipliersFinal;
					if (DynamicFinalXKnock != 0) {
						knockMultipliersFinal.x = transform.InverseTransformDirection(hit.GetComponent<Rigidbody>().velocity).z*-DynamicFinalXKnock;
						knockMultipliersFinal.y *= adjustedWeapon.knockback;
						adjustedWeapon.knockback = 1;
					}
					DealDamage(hit, adjustedWeapon, knockMultipliersFinal, Weapon.IDs);
					if(DynamicFinalXKnock == 0)
						hit.GetComponent<Rigidbody>().AddForce (transform.right*-Mathf.Clamp (getDisplacementX(hit.transform)*0.1f, -0.2f, 0.2f)*adjustedWeapon.knockback, ForceMode.VelocityChange);
					else
						hit.GetComponent<Rigidbody>().AddForce (transform.right*Mathf.Clamp (getDisplacementX(hit.transform)*0.1f, -0.2f, 0.2f)*adjustedWeapon.knockback, ForceMode.VelocityChange);
					if (DynamicFinalXKnock != 0) {
						knockMultipliersFinal.x = tempKMF.x;
						knockMultipliersFinal.y = tempKMF.y;
						adjustedWeapon.knockback = finalKnockback;
					}
				}
			}
		}
		doneCasting = true;
		yield return new WaitForSeconds (hitDelay);
		GameObject[] tempFX2 = GameObject.FindGameObjectsWithTag ("TempFX_MH");
		foreach(GameObject PS in tempFX2) {
			Destroy (PS);
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.GetComponent<Health> ()) {
			if(col.GetComponent<Health>() != GetComponent<Health>())
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

	float getDisplacementX (Transform obj) {
		return transform.InverseTransformPoint (obj.position).x;
	}
}