using UnityEngine;

public class Batzilla : EnemyBase {
	private Transform torso, head, target, moon;
	private Transform[] arms = new Transform[2];
	private SpriteRenderer headRenderer, torsoRenderer;
	private Core dark;
	private TrailRenderer[] slash = new TrailRenderer[2];
	private Vector3 slashscl = new Vector3 (5000, 0, 0), slashrot = new Vector3 (0, 0, 0);
	private float timer = 1.5f, time = 0, torsoAngle, armSpeed = 30, offset;
	private float[] armAngle = new float[2];
	private Vector3 hor = new Vector3 (2, 0), ver = new Vector3 (0, 2, 0.01f), armOffset = new Vector3 (0, 0.1f);
	private EnemyInfo bat;
	private int stunID, shotId, armID, waveID, eyes, trailID, impactID;
	private bool colliders {
		set {
			foreach (Collider2D col in GetComponentsInChildren<Collider2D> ()) {
				col.enabled = value;
			}
		}
	}
	Del update;

	public override void SetSprites (EnemyInfo ei) {
		BossWarning.Show ();
		SoundManager.Play (2);
		name += "Boss";
		damageEffect = true;
		EnemySpawner.boss = true;
		SetHP (1800, ei.lifeproportion);
		fallSpeed = 0;
		GameObject go = new GameObject ("legs");
		_renderer = go.AddComponent<SpriteRenderer> ();
		_renderer.sprite = ei.sprites[1];
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.back / 50;
		Transform trans = go.transform;
		go = new GameObject ("torso");
		torsoRenderer = go.AddComponent<SpriteRenderer> ();
		torsoRenderer.sprite = ei.sprites[2];
		torso = go.transform;
		torso.parent = trans;
		go.transform.localPosition = new Vector3 (0, -0.3f, -.01f);
		go = new GameObject ("head");
		headRenderer = go.AddComponent<SpriteRenderer> ();
		headRenderer.sprite = ei.sprites[3];
		head = go.transform;
		head.parent = torso;
		go.transform.localPosition = new Vector3 (0, 3.7f, -.01f);
		go = new GameObject ("wingLbig");
		go.AddComponent<SpriteRenderer> ().sprite = ei.sprites[4];
		go.AddComponent<PolygonCollider2D> ();
		go.transform.parent = torso;
		go.transform.localPosition = new Vector3 (-2, 2, -.01f);
		arms[0] = go.transform;
		go = new GameObject ("slash");
		slash[0] = go.AddComponent<TrailRenderer> ();
		slash[0].enabled = false;
		slash[0].time = 1;
		slash[0].startWidth = 1.2f;
		slash[0].endWidth = 0.1f;
		slash[0].material = ei.material;
		go.transform.parent = arms[0];
		go.transform.localPosition = new Vector3 (-4, 1, .01f);
		go = new GameObject ("wingRbig");
		go.AddComponent<SpriteRenderer> ().sprite = ei.sprites[5];
		go.AddComponent<PolygonCollider2D> ();
		go.transform.parent = torso;
		go.transform.localPosition = new Vector3 (2, 2, -.01f);
		arms[1] = go.transform;
		go = new GameObject ("slash");
		slash[1] = go.AddComponent<TrailRenderer> ();
		slash[1].enabled = false;
		slash[1].time = 1f;
		slash[1].startWidth = 1.2f;
		slash[1].endWidth = 0.1f;
		slash[1].material = slash[0].material;
		go.transform.parent = arms[1];
		go.transform.localPosition = new Vector3 (4, 1, .01f);

		moon = PropSpawner.ManualSpawn (1);
		moon.position = Vector3.up * 12;

		go = new GameObject ("dark");
		Texture2D t = new Texture2D (1, 1);
		t.SetPixels (new Color[] { Color.black });
		t.Apply (false);
		dark = go.AddComponent<Core> ().Set (Sprite.Create (t, new Rect (0, 0, 1, 1), new Vector2 (0.5f, 0.5f)), new Color (0f, 0f, 0f, 0f));
		dark.white = new Color (0f, 0f, 0f, 1f);
		go.transform.localScale = new Vector3 (5000, 5000);
		go.transform.position = new Vector3 (0, 0, -0.09f);
		bat = (ei as CarrierInfo).spawnable;
		shotId = ei.bulletsID[0];
		stunID = ei.bulletsID[2];
		waveID = ei.particleID[0];
		eyes = ei.particleID[1];
		trailID = ei.particleID[2];
		impactID = ei.particleID[3];
		update = Intro;
	}

	void Awake () {

	}

	void Intro () {
		transform.Translate (0, -Time.deltaTime * 2, 0);
		if (arms[0].localPosition == -hor + ver + armOffset * offset) {
			if (offset == 1) offset = -1;
			else offset = 1;
		}
		if (transform.position.y < Scaler.sizeY / 2) update = Wait;
		transform.localScale = new Vector3 (1, 1, 1) * .7f;
	}

	void Wait () {
		if (!target) target = GetPlayer ();
		if (Mathf.Abs (target.position.x - transform.position.x) > 5) transform.Translate ((transform.position.x > target.position.x? - 2 : 2) * Time.deltaTime, 0, 0, Space.World);
		torsoAngle = 0;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.identity, Time.deltaTime * 20);
		if (transform.position.y > Scaler.sizeY / 2 + 1) transform.Translate (0, -Time.deltaTime * 2, 0, Space.World);
		if (transform.position.y < Scaler.sizeY / 2) {
			transform.Translate (0, Time.deltaTime * 3, 0);
			if (transform.position.y > Scaler.sizeY / 2) time = Time.time + 2;
		}
		if (arms[0].localPosition == -hor + ver + armOffset * offset) {
			if (offset == 1) offset = -1;
			else offset = 1;
		} else if (time < Time.time) {
			switch (Random.Range (0, 3)) {
				case 0:
					update = Rise;
					armID = Random.Range (0, 2);
					break;
				case 1:
					Shoot (new Vector3 (-2, 4, -0.01f));
					Shoot (new Vector3 (2, 4, -0.01f));
					break;
				case 2:
					Bat ();
					break;
			}
		}
	}

	void Rise () {
		if (time < Time.time) {
			transform.Translate (0, Time.deltaTime * 3, 0);
			fallSpeed = Mathf.MoveTowards (fallSpeed, 0, Time.deltaTime * 4);
			dark.Add (Time.deltaTime * 2);
			if (arms[0].localPosition == -hor + ver + armOffset * offset) {
				if (offset == 1) offset = -1;
				else offset = 1;
			}
			if (transform.position.y > Scaler.sizeY - 2) {
				time = Time.time + 4;
				dark.Set (1);
				colliders = false;
				ParticleManager.Emit (eyes, target.position + Vector3.up * 3, 1);
				GameObject go = new GameObject ("webshot");
				go.transform.position = target.position + Vector3.up * 3;
				go.AddComponent<WebShot> ().Set (stunID, go.transform.position, 0, 3, 3, 0, "enemy");
				EnemySpawner.AddPost(go);
			}
		} else {
			if (time < Time.time + 1f) {
				if (hp < 500 && Random.value > 0.5f) {
					Vector3 h = head.position, v = target.position;
					for (int i = 0; i < 8; i++) {
						head.position = new Vector3 (v.x + Mathf.Sin (i * 22.5f) * 10, v.y + Mathf.Cos (i * 22.5f) * 10, 0.1f);
						Bat ();
					}
					head.position = h;
					update = Wait;
					dark.Set (0);
					time = Time.time + 4;
				} else {
					transform.position = new Vector3 (target.position.x + (armID == 0 ? 4 : -4), transform.position.y);
					slash[armID].enabled = true;
					update = Fall;
				}
				colliders = true;
			}
		}
	}

	void Fall () {
		offset = 0;
		dark.Min (Time.deltaTime * 10);
		transform.Translate (0, fallSpeed * Time.deltaTime, 0, Space.World);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, 0, armID == 0 ? 15 : -15), 10 * Time.deltaTime);
		torsoAngle = armID == 0 ? 10 : -10;
		fallSpeed = Mathf.MoveTowards (fallSpeed, -8, Time.deltaTime * 4);
		armAngle[0] = armID == 0 ? -15 : -5;
		armAngle[1] = armID == 0 ? 5 : 15;
		if (transform.position.y < -Scaler.sizeY / 2) {
			armAngle[armID] = armID == 0 ? 45 : -45;
			armSpeed = 180;
			torsoAngle = armID == 0 ? 15 : -15;
			update = Slash;
		}
	}

	void Slash () {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.identity, 10 * Time.deltaTime);
		if (arms[armID].localRotation == Quaternion.Euler (0, 0, armAngle[armID])) {
			update = Wait;
			armAngle[0] = armAngle[1] = 0;
			torsoAngle = 0;
			slash[armID].enabled = false;
			target = null;
			time = Time.time + 2;
		}
	}

	new void Update () {
		if (Ship.paused) return;
		base.Update ();
		moon.position = Vector3.MoveTowards (moon.position, Vector3.zero, Time.deltaTime / 2);
		update?.Invoke ();
		ParticleManager.Emit (waveID, transform.position - transform.up * 3, transform.up, 1);
		if (torso) {
			arms[0].localRotation = Quaternion.RotateTowards (arms[0].localRotation, Quaternion.Euler (0, 0, armAngle[0]), armSpeed * Time.deltaTime);
			arms[1].localRotation = Quaternion.RotateTowards (arms[1].localRotation, Quaternion.Euler (0, 0, armAngle[1]), armSpeed * Time.deltaTime);
			torso.localRotation = Quaternion.RotateTowards (torso.localRotation, Quaternion.Euler (0, 0, torsoAngle), armSpeed * Time.deltaTime);
			arms[0].localPosition = Vector3.MoveTowards (arms[0].localPosition, -hor + ver + armOffset * offset, Time.deltaTime / 5);
			arms[1].localPosition = Vector3.MoveTowards (arms[1].localPosition, hor + ver + armOffset * offset, Time.deltaTime / 5);
		}
	}

	protected override void Die () {
		ParticleManager.Emit (1, head.position, 1, 2);
		Locks.Boss (5, true);
		Destroy (head.gameObject);
		Destroy (dark.gameObject);
		EnemySpawner.points[killerid] += 1000;
		time = Time.time + 3;
		colliders = false;
		update = Dying;
	}

	void Dying () {
		if (time < Time.time + 2 && torso) {
			Destroy (torso.parent.gameObject);
		}
		transform.Rotate (0, 0, 5 * Time.deltaTime);
		transform.Translate (0, -Time.deltaTime, 0);
		if (Time.time % 0.5f < 0.1f) ParticleManager.Emit (1, transform.position, 1, Random.value * 3);
		if (time < Time.time) Loader.Scene ("MenuSelection");
	}

	void Shoot (Vector3 v) {
		GameObject go = new GameObject ("enemybullet");
		go.AddComponent<SpriteRenderer> ().sprite = Bullet.sprites[shotId];
		go.AddComponent<BoxCollider2D> ();
		Bullet b = go.AddComponent<Bullet> ();
		b.owner = name;
		b.spriteID = shotId;
		b.particleID = trailID;
		b.impactID = impactID;
		go.transform.position = transform.position + v;
		go.transform.up = -transform.up;
		time = Time.time + 0.5f;
	}

	private new void OnCollisionEnter2D (Collision2D col) {
		if (update != Intro && !col.otherCollider.name.Contains ("wing")) base.OnCollisionEnter2D (col);
		else ParticleManager.Emit (3, col.collider.transform.position, 1);
	}

	void Bat () {
		GameObject go = new GameObject ("enemy");
		Bat b = go.AddComponent<Bat> ();
		b.target = head.position + (GetPlayer ().position - head.position).normalized * 5f;
		b.SetSprites (bat);
		go.AddComponent<SpriteRenderer> ().sprite = bat.sprites[0];
		go.AddComponent<BoxCollider2D> ();
		Rigidbody2D r = go.AddComponent<Rigidbody2D> ();
		r.isKinematic = true;
		r.useFullKinematicContacts = true;
		go.transform.position = head.position;
		time = Time.time + 1;
	}

	public override void Position (int i) {
		transform.position = new Vector3 (0, Scaler.sizeY + 4, 0);
	}
}