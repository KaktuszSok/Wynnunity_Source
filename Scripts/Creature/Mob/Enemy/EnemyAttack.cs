using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	
	public Enemy enemy;
	public EnemyTargetting enemyMovement;
	public Item_Weapon Weapon;
	public int minDmg = 1;
	public int maxDmg = 1;
	public float range = 3;
	public float knockback = 3.5f;
	public float hitCooldown = 0.5f;
	private float nextHitTime = 0;
	public GameObject projectile;
	public Transform fireTransform;

	public bool hasLOS;

	void Awake () {
		if (!enemy)
			enemy = GetComponent<Enemy> ();
		Weapon = new Item_Weapon ("", minDmg, maxDmg, range, knockback, hitCooldown);
		if (!enemyMovement)
			enemyMovement = GetComponentInChildren<EnemyTargetting> ();
	}

	void Update () {

		RaycastHit hit;
		if (enemyMovement.player) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, Player.EnemyLOS)) {
				if (hit.transform.root.tag == "Player")
					hasLOS = true;
				else {
					hasLOS = false;
				}
			} else {
				hasLOS = false;
			}
		}


		if (enemyMovement.player && hasLOS && Vector3.Distance (transform.position, enemyMovement.player.position) < Weapon.range && Time.time > nextHitTime && enemy.player.GetComponent<Health>().health != 0) {
		nextHitTime = Time.time + hitCooldown;
			if(!projectile){
				if(!enemy.player.GetComponent<Health>().invulnerable)
					enemy.player.GetComponent<Health>().health -= (float) Weapon.GetDmg()*enemy.player.GetComponent<Health>().dmgTaken;
				enemy.player.GetComponent<Health>().GetComponent<Rigidbody>().AddForce ((transform.forward + enemy.player.GetComponent<Health>().transform.up) * Weapon.knockback, ForceMode.VelocityChange);
				StartCoroutine (shakeCam());
			}
			else {
				GameObject proj = (GameObject) Instantiate (projectile, fireTransform.position, transform.rotation);
				proj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
				proj.GetComponent<Projectile>().stats = Weapon;
			}
		}
	}

	public static IEnumerator shakeCam () {
		Camera.main.transform.Rotate (0, 0, -5);
		yield return new WaitForSeconds(0.05f);
		Camera.main.transform.Rotate (0, 0, 5);
	}
}
