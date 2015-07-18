using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public int ManaConsumed = 6;
	public int SpamMana = 0;
	public float SpamResetTime = 3f;
	public float NextSpamResetTime;

	public void DealDamage(Health target, Item_Weapon weapon, Vector2 knockbackMultiplierXY, IDInfo IDs) {
		if (!target.invulnerable)
			target.health -= (float)weapon.GetDmg () * target.dmgTaken;
		IDs.Steal ();
		target.GetComponent<Rigidbody>().AddForce ((transform.forward*knockbackMultiplierXY.x + target.transform.up*knockbackMultiplierXY.y) * weapon.knockback, ForceMode.VelocityChange);
		if (target.GetComponent<Enemy> ())
			target.GetComponent<Enemy> ().disableWalk = true;
		if (target == Player.Health)
			StartCoroutine (EnemyAttack.shakeCam ());
		else {
			ChangeEnemyCol(Player.Punch.hitCol, target.GetComponent<Renderer>());
		}
	}

	public void DealDamage(Health target, Item_Weapon weapon, Vector2 knockbackMultiplierXY, IDInfo IDs, bool knockAway) {
		if(!target.invulnerable)
			target.health -= weapon.GetDmg()*target.dmgTaken;
		IDs.Steal ();
		target.GetComponent<Rigidbody>().AddForce (((target.transform.position - transform.position).normalized*knockbackMultiplierXY.x + target.transform.up*knockbackMultiplierXY.y) * weapon.knockback, ForceMode.VelocityChange);
		if (target.GetComponent<Enemy> ())
			target.GetComponent<Enemy> ().disableWalk = true;
		if (target == Player.Health)
			StartCoroutine (EnemyAttack.shakeCam ());
		else {
			ChangeEnemyCol(Player.Punch.hitCol, target.GetComponent<Renderer>());
		}
	}

	void ChangeEnemyCol(Color hc, Renderer rend) {
		rend.material.color = hc;
		if (rend.GetComponent<Enemy> ()) {
			rend.GetComponent<Enemy> ().defColourTime = Time.time + 0.5f;
			if (rend.GetComponent<Enemy> ().FX)
				rend.GetComponent<Enemy> ().FX.Play ();
		}
	}
}
