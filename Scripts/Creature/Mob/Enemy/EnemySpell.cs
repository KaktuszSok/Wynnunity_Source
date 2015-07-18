using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpell : MonoBehaviour {

	public List<Spell> Spells;
	public EnemyAttack enemy;
	public List<float> spellRanges;
	public float minSpellDelay = 10f;
	public float maxSpellDelay = 20f;
	public float chancePerSecond = 10f;
	public float tryCastTime;
	public Spell Spell;
	public ParticleSystem WarmupFX;
	private float defSpd;
	public float DisableTurnBeforeEndCast = 0.25f;
	public List<float> DTBEC;

	void Start() {
		defSpd = enemy.enemy.speed;
		if (!enemy)
			enemy = transform.root.GetComponent<EnemyAttack> ();
	}

	void Update() {
		if (enemy.enemy.HP.health == 0) {
			StopAllCoroutines();
		}
		if (enemy.enemy.player && Spells.Count != 0 && Spell == null && enemy.enemy.player.GetComponent<Health>().health != 0) {
			Spell = Spells [Random.Range (0, Spells.Count)];
		} else if(enemy.enemy.player && enemy.hasLOS && Spells.Count != 0 && enemy.enemy.player.GetComponent<Health>().health != 0) {
			float spellRange = spellRanges [Spells.IndexOf (Spell)];
			DisableTurnBeforeEndCast = DTBEC[Spells.IndexOf (Spell)];
			if (Vector3.Distance (enemy.transform.position, enemy.enemy.player.transform.position) < spellRange) {
				if(Time.time > tryCastTime && Time.time < tryCastTime + maxSpellDelay) {
					if(Random.Range (0f, 100f) <= chancePerSecond*Time.deltaTime){
						StartCoroutine(StartCasting (Spell, enemy.Weapon));
						tryCastTime = Time.time + minSpellDelay;
						Spell = null;
					}
				} else if(Time.time > tryCastTime + maxSpellDelay){
					StartCoroutine(StartCasting (Spell, enemy.Weapon));
					tryCastTime = Time.time + minSpellDelay;
					Spell = null;
				}
			}
		}
	}

	public IEnumerator StartCasting(Spell spell, Item_Weapon weapon) {
		WarmupFX.Play ();
		enemy.enemy.speed = 0;
		yield return new WaitForSeconds (WarmupFX.duration - DisableTurnBeforeEndCast);
		enemy.enemy.disableTurnTime = Time.time + DisableTurnBeforeEndCast;
		yield return new WaitForSeconds (WarmupFX.duration - WarmupFX.time);
		enemy.enemy.speed = defSpd;
		Cast (spell, weapon);
	}

	public void Cast(Spell spell, Item_Weapon weapon) {
		if (spell is Spell_Multihit) {
			Spell_Multihit S = spell as Spell_Multihit;
			S.StartCoroutine (S.CastSpell (enemy.Weapon));
		}
		if (spell is Spell_Charge) {
			Spell_Charge S = spell as Spell_Charge;
			S.StartCoroutine (S.CastSpell (enemy.Weapon));
		}
		if (spell is Spell_Shockwave) {
			Spell_Shockwave S = spell as Spell_Shockwave;
			S.StartCoroutine (S.CastSpell (enemy.Weapon));
		}
	}
}
