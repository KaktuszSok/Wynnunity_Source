using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

	public bool player = false;
	public Player PlayerInfo;
	public float health = 10;
	public int maxHealth = 10;
	public int regen = 5;
	public float regenDelay = 10;
	private float nextRegen;
	public bool invulnerable = false;
	public bool dead = false;
	public Transform respScreen;
	public Transform spawnPos;
	public float dmgTaken = 1f;
	public float knockTaken = 1f;

	// Use this for initialization
	void Start () {
		nextRegen = regenDelay + Time.time;
		if (!PlayerInfo)
			PlayerInfo = GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		health = Mathf.Clamp (health, 0, maxHealth);
		if (health == 0 && !dead && !player) {
			dead = true;
			StartCoroutine (Die ());

		} else if (health == 0 && !dead) {
			dead = true;
			respScreen.gameObject.SetActive(true);
			PlayerInfo.walk.enabled = false;
			PlayerInfo.punch.enabled = false;
			PlayerInfo.TB.enabled = false;
			PlayerInfo.TH.enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			GetComponent<Rigidbody> ().isKinematic = true;
		} else if (!dead) {

			if(Time.time >= nextRegen && health < maxHealth && regen > 0 || Time.time >= nextRegen && regen < 0 ) {
				nextRegen = Time.time + regenDelay;
				health += regen;
			}

		}
	}

	public IEnumerator Die () {
		for (float i = 0; i < 90; i += 180*Time.deltaTime) {
			transform.Rotate (Vector3.forward*180*Time.deltaTime, Space.Self);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds (0.75f);
		Destroy (gameObject);
	}

	public void Respawn () {

		health = maxHealth;
		PlayerInfo.mana.MP = PlayerInfo.mana.maxMP;
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;
		GetComponent<Rigidbody> ().isKinematic = false;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		nextRegen = Time.time + regenDelay;
		PlayerInfo.walk.enabled = true;
		PlayerInfo.punch.enabled = true;
		PlayerInfo.TB.enabled = true;
		PlayerInfo.TH.enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		dead = false;
		invulnerable = false;
	}
}
