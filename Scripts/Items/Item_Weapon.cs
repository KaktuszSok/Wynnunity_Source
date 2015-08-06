﻿using UnityEngine;
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

	public void DealDamage(Health target, Transform user, Vector2 knockbackMultiplierXY, IDInfo IDs) {
		if (target.health != 0) {
			if (!target.invulnerable)
				target.health -= Mathf.Clamp (GetDmg () * target.dmgTaken, Mathf.NegativeInfinity, target.health);
			if(IDs)
				IDs.Steal ();
			target.GetComponent<Rigidbody> ().velocity = (user.forward * knockbackMultiplierXY.x + target.transform.up * knockbackMultiplierXY.y) * knockback;
			if (target.GetComponent<Enemy> ())
				target.GetComponent<Enemy> ().disableWalk = true;
			if (target == Player.Health) {
				EnemyAttack.shakeCam ();
				Punch.PlayHitSound ();
			} else {
				ChangeEnemyCol (Player.Punch.hitCol, target.GetComponent<Enemy> ().rends);
				Punch.PlayHitSound ();
			}
		}
	}
	
	public void DealDamage(Health target, Transform user, Vector2 knockbackMultiplierXY, IDInfo IDs, bool knockAway) {
		if (target.health != 0) {
			if (!target.invulnerable)
				target.health -= Mathf.Clamp (GetDmg () * target.dmgTaken, Mathf.NegativeInfinity, target.health);
			IDs.Steal ();
			target.GetComponent<Rigidbody> ().velocity = ((target.transform.position - user.position).normalized * knockbackMultiplierXY.x + target.transform.up * knockbackMultiplierXY.y) * knockback;
			if (target.GetComponent<Enemy> ())
				target.GetComponent<Enemy> ().disableWalk = true;
			if (target == Player.Health) {
				EnemyAttack.shakeCam ();
				Punch.PlayHitSound ();
			} else {
				ChangeEnemyCol (Player.Punch.hitCol, target.GetComponent<Enemy> ().rends);
				Punch.PlayHitSound ();
			}
		}
	}
	
	void ChangeEnemyCol(Color hc, List<MeshRenderer> rends) {
		foreach (MeshRenderer rend in rends) {
			rend.material.color = hc;
			if (rend.transform.root.GetComponent<Enemy> ()) {
				rend.transform.root.GetComponent<Enemy> ().defColourTime = Time.time + 0.5f;
				if (rend.transform.root.GetComponent<Enemy> ().FX)
					rend.transform.root.GetComponent<Enemy> ().FX.Play ();
			}
		}
	}
}
