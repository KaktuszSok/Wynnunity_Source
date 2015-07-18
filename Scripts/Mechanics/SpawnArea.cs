using UnityEngine;
using System.Collections;

public class SpawnArea : MonoBehaviour {
	
	public GameObject Mob;
	public float MinSpawnDelay = 4f;
	public float MaxSpawnDelay = 10f;
	public int SpawnAmountMin = 1;
	public int SpawnAmountMax = 3;
	public int MaxMobsInTrigger = 12;
	public float MobHalfHeight = 1f;
	private float nextSpawnTime;

	public SpawnTrigger Trigger;
	public Vector3 Area;

	public bool activated = false;

	void Start () {
		Area = transform.TransformDirection (transform.localScale / 2);
		if (!Trigger) {
			Trigger = transform.GetChild (0).GetComponent<SpawnTrigger>();
		}
	}

	void Update () {
		if (activated) {
			if(Time.time > nextSpawnTime && Trigger.MobsInTrigger.Count < MaxMobsInTrigger) {
				nextSpawnTime = Time.time + Random.Range (MinSpawnDelay, MaxSpawnDelay);
				SpawnGroup();
			}
		}
	}

	public void SpawnGroup () {
		int SpawnAmount = Random.Range (SpawnAmountMin, SpawnAmountMax - 1);
		for (int i = 0; i < SpawnAmount; i++) {
			SpawnMob();
		}
	}

	public void SpawnGroup (int minAmount, int maxAmount) {
		int SpawnAmount = Random.Range (minAmount, maxAmount - 1);
		for (int i = 0; i < SpawnAmount; i++) {
			SpawnMob();
		}
	}

	void SpawnMob () {
		Vector3 randPos = new Vector3 (Random.Range (transform.position.x - Area.x, transform.position.x + Area.x), Random.Range (transform.position.y - Area.y, transform.position.y + Area.y), Random.Range (transform.position.z - Area.z, transform.position.z + Area.z));
		Vector3 randRot = new Vector3 (0, Random.Range (-180, 180), 0);
		if (Physics.CheckSphere (randPos, 1.5f)) {
			if (Physics.CheckSphere (randPos - MobHalfHeight*Vector3.up, 0.5f)) {
				GameObject InstMob = (GameObject) Instantiate (Mob, randPos, Quaternion.Euler(randRot));
				InstMob.name = Mob.name;
				InstMob.AddComponent <MobsSpawnArea>();
				InstMob.GetComponent <MobsSpawnArea>().MSA = this;
			} else {
				SpawnMob();
			}
		} else {
			SpawnMob();
		}
	}
}
