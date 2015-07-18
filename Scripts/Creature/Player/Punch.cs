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

	private bool HitEnemiesInvulnerable = false;

	public LayerMask hittableLayers;

	// Use this for initialization
	void Start () {
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
					if (hit.transform.GetComponent<Health> () && !hit.transform.GetComponent<Health> ().invulnerable)
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
						hit.transform.GetComponent<Rigidbody> ().AddForce ((transform.forward * knockDirectionMult.x + hit.transform.up * knockDirectionMult.y) * heldWeapon.knockback * knockMult, ForceMode.VelocityChange);
					}
					if (hit.transform.GetComponent<Renderer> () && hit.transform.GetComponent<Health> ()) {
						StartCoroutine (ChangeEnemyCol (hitCol, hit.transform.GetComponent<Renderer> ()));
					}
				}
			}
		} else if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, Itemrange, hittableLayers)) {
			if (hit.transform != transform) {
				if (hit.transform.GetComponent<Health> () && !hit.transform.GetComponent<Health> ().invulnerable)
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
					hit.transform.GetComponent<Rigidbody> ().AddForce ((transform.forward * knockDirectionMult.x + hit.transform.up * knockDirectionMult.y) * Itemknockback * knockMult, ForceMode.VelocityChange);
				}
				if (hit.transform.GetComponent<Renderer> () && hit.transform.GetComponent<Health> ()) {
					StartCoroutine (ChangeEnemyCol (hitCol, hit.transform.GetComponent<Renderer> ()));
				}
			}
		}
	}

	IEnumerator ChangeEnemyCol(Color hc, Renderer rend) {
		rend.material.color = hc;
		if (rend.GetComponent<Enemy> ()) {
			rend.GetComponent<Enemy> ().defColourTime = Time.time + 0.5f;
			if(rend.GetComponent<Enemy>().FX)
				rend.GetComponent<Enemy>().FX.Play ();
		}
		if(HitEnemiesInvulnerable)
			rend.transform.GetComponent<Health> ().invulnerable = true;
		yield return new WaitForSeconds (0.5f);
		if (rend) {
			if(HitEnemiesInvulnerable)
				rend.transform.GetComponent<Health> ().invulnerable = false;
		}
	}
}
