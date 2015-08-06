using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Punch : MonoBehaviour {
	
	public List<GameObject> itemIcons;
	public Animation punchAnim;
	public string Itemname = "";
	public int ItemdamageMin = 1;
	public int ItemdamageMax = 1;
	public float Itemrange = 3f;
	public float Itemknockback = 1f;
	public float ItemCooldown = 0.3f;
	public Item heldItem;
	public Color hitCol;
	private float nextHitTime = 0f;
	private Walk walk;
	public float WalkKnockbackMult = 1.5f;
	public float SprintKnockbackMult = 2.25f;
	public float knockMult = 1f;
	public Vector2 knockDirectionMult = Vector2.one;
	public IDInfo WpnIDInfo;
	public static AudioSource[] hitSounds;
	public LayerMask hittableLayers;

	// Use this for initialization
	void Start () {
		hitSounds = GetComponents<AudioSource> ();
		heldItem = ItemDB.Weapons [0];
		heldItem.ID = 0;
		walk = GetComponent<Walk> ();
	}
	
	// Update is called once per frame
	void Update () {

		punchAnim = itemIcons [Mathf.Clamp (heldItem.ID, 0, itemIcons.Count - 1)].GetComponent<Animation>();
		if (heldItem is Item_Weapon) {
			itemIcons [Mathf.Clamp (heldItem.ID, 0, itemIcons.Count - 1)].GetComponent<WeaponInfo> ().weapon = heldItem as Item_Weapon;
		}

		for (int i = 0; i < itemIcons.Count; i++) {
			if(i != Mathf.Clamp (heldItem.ID, 0, itemIcons.Count - 1))
				itemIcons[i].SetActive(false);
			else
				itemIcons[i].SetActive (true);
		}
		if (Input.GetMouseButtonDown (0)) {
			if(Time.time >= nextHitTime)
			{
				Hit ();
				punchAnim.Stop ();
				punchAnim.Play();
				if(heldItem is Item_Weapon)
				{
					Item_Weapon heldWeapon = (Item_Weapon) heldItem;
					nextHitTime = Time.time + heldWeapon.hitCooldown;
				} else {
					nextHitTime = Time.time + ItemCooldown;
				}
			}
		}
	}
	void Hit () {
		RaycastHit hit;
		if (heldItem is Item_Weapon) {
			Item_Weapon heldWeapon = (Item_Weapon)heldItem;
			if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, heldWeapon.range, hittableLayers)) {
				if (hit.transform != transform) {
					if(hit.transform.GetComponent<Health>() && hit.transform.GetComponent<Health>().health != 0) {
						if (!hit.transform.GetComponent<Health> ().invulnerable)
							hit.transform.GetComponent<Health> ().health -= (float) heldWeapon.GetDmg()*hit.transform.GetComponent<Health>().dmgTaken;
						itemIcons[Mathf.Clamp (heldItem.ID, 0, itemIcons.Count)].GetComponent<IDInfo>().Steal ();
						if (hit.transform.GetComponent<Rigidbody> ()) {
							if (!walk.moving) {
								knockMult = 1;
							} else if (walk.speed == walk.baseSpeed)
								knockMult = WalkKnockbackMult;
							else if (walk.speed == walk.sprintSpeed)
								knockMult = SprintKnockbackMult;
							if (hit.transform.GetComponent<Enemy> ()) {
								hit.transform.GetComponent<Enemy> ().disableWalk = true;
							}
							hit.transform.GetComponent<Rigidbody> ().velocity = (transform.forward * knockDirectionMult.x + hit.transform.up * knockDirectionMult.y) * heldWeapon.knockback * knockMult;
						}
						if (hit.transform.GetComponent<Enemy> () && hit.transform.GetComponent<Health> ()) {
							ChangeEnemyCol (hitCol, hit.transform.GetComponent<Enemy> ().rends);
							PlayHitSound();
						}
					}
				}
			}
		} else if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, Itemrange, hittableLayers)) {
			if (hit.transform != transform) {
				if(hit.transform.GetComponent<Health> () && hit.transform.GetComponent<Health> ().health != 0) {
					if (!hit.transform.GetComponent<Health> ().invulnerable)
						hit.transform.GetComponent<Health> ().health -= (float) 1*hit.transform.GetComponent<Health>().dmgTaken;
					if (hit.transform.GetComponent<Rigidbody> ()) {
						if (!walk.moving) {
							knockMult = 1;
						} else if (walk.speed == walk.baseSpeed)
							knockMult = WalkKnockbackMult;
						else if (walk.speed == walk.sprintSpeed)
							knockMult = SprintKnockbackMult;
						if (hit.transform.GetComponent<Enemy> ()) {
							hit.transform.GetComponent<Enemy> ().disableWalk = true;
						}
						hit.transform.GetComponent<Rigidbody> ().velocity = (transform.forward * knockDirectionMult.x + hit.transform.up * knockDirectionMult.y) * Itemknockback * knockMult;
					}
					if (hit.transform.GetComponent<Enemy> () && hit.transform.GetComponent<Health> ()) {
						ChangeEnemyCol (hitCol, hit.transform.GetComponent<Enemy> ().rends);
						PlayHitSound();
					}
				}
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

	public static void PlayHitSound() {
		hitSounds [Random.Range (0, hitSounds.Length)].Play ();
	}
}
