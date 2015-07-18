using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Item_Weapon stats;
	private Rigidbody rb;
	public float FiredVel;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddRelativeForce (Vector3.forward * FiredVel, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.GetComponent<Health> ()) {
			col.gameObject.GetComponent<Rigidbody>().AddForce ((transform.forward + Player.Health.transform.up) * stats.knockback, ForceMode.VelocityChange);
			if(col.transform.tag == "Player")
			{
				StartCoroutine (EnemyAttack.shakeCam());
			} else if(col.GetComponent<Enemy>()){
				StartCoroutine(ChangeEnemyCol(Player.Punch.hitCol, col.GetComponent<Renderer>()));
			}
			if(!col.gameObject.GetComponent<Health>().invulnerable)
				col.gameObject.GetComponent<Health>().health -= stats.GetDmg()*col.gameObject.GetComponent<Health>().dmgTaken;

		}
		StartCoroutine (Despawn ());
	}

	IEnumerator Despawn() {
		Destroy (GetComponent<Collider> ());
		MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer rend in rends) {
			Destroy (rend);
		}
		Destroy (GetComponent<Rigidbody> ());
		yield return new WaitForSeconds(0.5f);
		Destroy (gameObject);
	}

	IEnumerator ChangeEnemyCol(Color hc, Renderer rend) {
		rend.material.color = hc;
		yield return new WaitForSeconds (0.5f);
	}
}