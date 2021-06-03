using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Train : EnemyBase {
    [SerializeField]
    BulletPath path;
    BulletPath[] paths;
    [SerializeField]
    Queue<Vector3> pos = new Queue<Vector3> ();
    public Vector3[] vec;
    float queueTime, proportion, time, timerPicker = 10;
    int shotID, pathID, nextPath, trailID, impactID, thatCar;
    Core lights;
    public List<Transform> cars = new List<Transform> ();
    [SerializeField]
    public EnemyInfo info;
    Del action;

    public override void SetSprites (EnemyInfo ei) {
        proportion = ei.lifeproportion;
        SetHP (150, ei.lifeproportion);
        name += "Boss";
        damageEffect = true;
        paths = ((MultiPathEnemy) ei).paths;
        path = paths[0];
        path = BulletPath.Upscale (path);
        EnemySpawner.boss = true;
        shotID = ei.bulletsID[0];
        trailID = ei.particleID[0];
        impactID = ei.particleID[1];
        action = Movement;
        action += Shot;
        action += RandomicCarPicker;
        GameObject go = new GameObject ("lights");
        lights = go.AddComponent<Core> ().Set (ei.sprites[9], Color.clear);
        go.transform.parent = transform;
        go.transform.Translate (0, 0, -0.05f);
        for (int i = 0; i < 9 /*total de vagões*/ ; i++) {
            go = new GameObject ("enemy" + i + "Boss");
            _renderer = go.AddComponent<SpriteRenderer> ();
            _renderer.sprite = ei.sprites[1 + i % 3];
            go.AddComponent<TrainCar> ().train = this;
            cars.Add (go.transform);
            go.transform.Translate (transform.position);
            go = new GameObject ("conector");
            go.AddComponent<SpriteRenderer> ().sprite = ei.sprites[4];
            go.transform.parent = _renderer.transform;
            go.transform.localPosition = new Vector3 (0, 1.8f, 0.1f);
            if (i % 3 == 2) Build (go, ei.sprites[5]);
            go = new GameObject ("lights");
            lights = go.AddComponent<Core> ().Set (ei.sprites[6 + i % 3], Color.clear);
            go.transform.parent = _renderer.transform;
            go.transform.Translate (0, 0, -0.05f);
        }
        vec = pos.ToArray ();
    }

    public override void OnCollisionEnter2D (Collision2D col) {
        if (col.otherCollider.name == _renderer.name) base.OnCollisionEnter2D (col);
        else if (!col.collider.name.Contains ("enemy")) ParticleManager.Emit (3, col.collider.transform.position, 1);
    }

    protected override void Die () {
        if (cars.Count > 0) {
            cars.Remove (_renderer.transform);
            pos.Dequeue ();
            queueTime = 0;
            // using(Queue<Vector3>.Enumerator e=pos.GetEnumerator()){
            //     e.MoveNext();
            //     pos=new Queue<Vector3>(pos.Where(x=>x!=e.Current));
            //     Debug.Log(pos);
            // }
            ParticleManager.Emit (1, _renderer.transform.position, 1);
            Destroy (_renderer.gameObject);
            //nextPath = cars.Count / (paths.Length - 1);
            nextPath = 9-cars.Count;
            print ("Actual Path: " + nextPath);
            if (cars.Count == 0) {
                _renderer = GetComponent<SpriteRenderer> ();
                lights = _renderer.GetComponentInChildren<Core> ();
                action = Movement;
                //action += RandomicCarPicker;
                //ANGRY MODE
            } else {
                int i = Random.Range (0, cars.Count);
                _renderer = cars[i].GetComponent<SpriteRenderer> ();
                lights = _renderer.GetComponentInChildren<Core> ();
                thatCar = i;
                action = Movement;
                action += RandomicCarPicker;
                SetHP (150, proportion);
                if (int.TryParse (_renderer.name.Substring (5, 1), out i)) {
                    if (i % 3 == 0) action += Bomb;
                    if (i % 3 == 1) SetHP (85, proportion);
                    if (i % 3 == 2) action += Shot;
                }
            }
        } else {
            time = Time.time + 3;
            action = Dying;
            action += RandomicCarPicker;
            ParticleManager.Emit (1, transform.position, 1, 2);
            GetComponent<BoxCollider2D> ().enabled = false;
            Locks.Boss (2, true);
        }
    }

    void RandomicCarPicker () {
        timerPicker -= 1 * Time.deltaTime;
        if (timerPicker < 1) {
            //_renderer = cars[thatCar].GetComponent<SpriteRenderer> ();
            //lights = _renderer.GetComponentInChildren<Core> ();
            lights.Set (timerPicker);
        }
        if (cars.Count > 0 && timerPicker < 0) {
            queueTime = 0;
            timerPicker = 10;
            _renderer.color = Color.white;
            int i = Random.Range (0, cars.Count);
            _renderer = cars[i].GetComponent<SpriteRenderer> ();
            lights = _renderer.GetComponentInChildren<Core> ();
            action = Movement;
            action += RandomicCarPicker;
            SetHP (150, proportion);
            if (int.TryParse (_renderer.name.Substring (5, 1), out i)) {
                if (i % 3 == 0) action += Bomb;
                if (i % 3 == 1) SetHP (85, proportion);
                if (i % 3 == 2) action += Shot;
            }
        }
    }

    void Dying () {
        if (time < Time.time) Loader.Scene ("MenuSelection");
    }

    public override void Position (int i) {
        transform.position = new Vector3 (Scaler.sizeX / 2 * 0.6f, Scaler.sizeY + 4, 0);
        foreach (Transform t in cars) {
            t.position = transform.position;
            // t.up=transform.up;
            pos.Enqueue (transform.position);
        }
    }

    void Build (GameObject go, Sprite s) {
        for (int i = 0; i < 4; i++) {
            GameObject g = new GameObject ("turret");
            g.AddComponent<SpriteRenderer> ().sprite = s;
            g.transform.parent = go.transform.parent;
            g.transform.localPosition = new Vector3 (i / 2 * 0.9f - 0.45f, i % 2 * 0.9f + 0.6f, -0.1f);
        }
    }

    void Start () {
        // SetSprites(info);
    }

    new void Update () {
        if (Ship.paused) return;
        base.Update ();
        action?.Invoke ();
        // Vector3 vector=Vector3.Cross(transform.up,path.Direction(false));
        // transform.Rotate(vector*180*Time.deltaTime);
        // // transform.up=-path.Direction(false);
        // transform.position=BulletPath.Next(ref path,false);
        // // transform.Translate(0,-Time.deltaTime*path.speed,0);
        // if(path.Finished())path.Restart();
        //transform.Translate(0,Time.deltaTime*5,0);
        /*
            if(transform.position.y>Scaler.sizeY/2 && transform.position.x<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-90),180*Time.deltaTime);
            if(transform.position.x>Scaler.sizeX/2-2.5f && transform.position.y>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,180),180*Time.deltaTime);
            if(transform.position.y<-Scaler.sizeY/2 && transform.position.x>0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,90),180*Time.deltaTime);
            if(transform.position.x<-Scaler.sizeX/2+2.5f && transform.position.y<=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,0),180*Time.deltaTime);
        */
        // if(transform.position.x>Scaler.sizeX/2-2 && transform.position.x>=0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,91),360*Time.deltaTime);
        // if(transform.position.x<-Scaler.sizeX/2+2 && transform.position.x<0)transform.rotation=Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(0,0,-90),360*Time.deltaTime);

    }

    void Shot () {
        Vector3 v = _renderer.transform.position;
        if (v.x > Scaler.sizeX * 2 || v.x < -Scaler.sizeX * 2 || v.y > Scaler.sizeY || v.y < -Scaler.sizeY) return;
        Transform target = GetPlayer (_renderer.transform.position);
        bool b = time < Time.time;
        for (int i = 0; i < 4; i++) {
            Transform t = _renderer.transform.GetChild (1 + i);
            v = target.position - _renderer.transform.position;
            v.z = 0;
            t.Rotate (Vector3.Cross (-t.up, v) * 60 * Time.deltaTime);
            if (b) Shot (t);
        }
        if (b) time = Time.time + 1;
    }

    void Shot (Transform t) {
        GameObject go = new GameObject ("enemybullet");
        go.AddComponent<SpriteRenderer> ().sprite = Bullet.sprites[shotID];
        go.AddComponent<BoxCollider2D> ();
        Bullet bu = go.AddComponent<Bullet> ();
        bu.spriteID = shotID;
        bu.particleID = trailID;
        bu.impactID = impactID;
        bu.bulletSpeed = 12;
        bu.owner = name;
        go.transform.position = t.position - Vector3.forward / 10;
        go.transform.up = -t.up;
    }

    void Bomb () {
        Vector3 v = _renderer.transform.position;
        if (v.x > Scaler.sizeX * 2 || v.x < -Scaler.sizeX * 2 || v.y > Scaler.sizeY || v.y < -Scaler.sizeY) return;
        if (time < Time.time) {
            GameObject g = new GameObject ("enemy");
            g.AddComponent<CircleCollider2D> ().radius = 5;
            g.transform.position = v + Vector3.forward / 5;
            ParticleManager.Emit (0, v, 1);
            time = Time.time + 2;
            Destroy (g, 0.6f);
        }
    }

    void Movement () {
        lights.Add (Time.deltaTime / 2);
        transform.Translate (0, -Time.deltaTime * path.speed, 0);
        Vector3 vector = Vector3.Cross (-transform.up, path.GetNode () - transform.position);
        transform.Rotate (vector * 60 * Time.deltaTime);
        if (Vector3.Distance (transform.position, path.GetNode ()) < 1.5f) {
            path.Next ();
            if (path.Finished ()) {
                if (pathID != nextPath) {
                    pathID = nextPath;
                    path = paths[paths.Length - 1 - pathID];
                    path = BulletPath.Upscale (path);
                    if (cars.Count == 0) path.speed = 10;
                } else path.Restart ();
            }
        }
        if (queueTime < Time.time) {
            queueTime = Time.time + 2f / path.speed;
            pos.Enqueue (transform.position);
            pos.Dequeue ();
            vec = pos.ToArray ();
        }
        for (int i = 0; i < cars.Count; i++) {
            cars[i].position = Vector3.MoveTowards (cars[i].position, vec[cars.Count - 1 - i], path.speed * Time.deltaTime);
            if (i == 0) vector = Vector3.Cross (cars[i].up, transform.position - cars[i].position);
            else vector = Vector3.Cross (cars[i].up, cars[i - 1].position - cars[i].position);
            vector.x = vector.y = 0;
            cars[i].Rotate (vector * 360 * Time.deltaTime);
            if (i == 0) cars[i].GetChild (0).up = transform.position - cars[i].position;
            else cars[i].GetChild (0).up = cars[i - 1].position - cars[i].GetChild (0).position;
        }
    }
}