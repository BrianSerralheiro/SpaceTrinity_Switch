using UnityEngine;

public class Homing : Bullet
{
    [SerializeField]
    Transform target;
    protected override void Start()
    {
        base.Start();
        timer=10;
    }
    void Update()
    {
        if(Ship.paused) return;
		ParticleManager.Emit(particleID,transform.position,1);
		if(bulletTime<=0)renderer.sprite=SpriteBase.I.bullets[spriteID+(blink?0:1)];
        if(target){
            timer=2;
            Vector3 v=transform.position-target.position;
            v.z=0;
            v.Normalize();
            transform.Rotate(Vector3.Cross(v,transform.up));
        }
        else {
            timer-=Time.deltaTime;
        }
		transform.Translate(0,Time.deltaTime*10,0);
		if(timer<=0) Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D col) {
        if(col.name=="enemy"){
            if(target){
                if((target.position-transform.position).sqrMagnitude>(col.transform.position-transform.position).sqrMagnitude)
                    target=col.transform;
            }
            else target=col.transform;
        }
    }
}
