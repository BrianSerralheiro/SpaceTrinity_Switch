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
		if(bulletTime<=0)renderer.sprite=Bullet.sprites[spriteID+((int)Time.time%4)];
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
		transform.Translate(0,Time.deltaTime*bulleSpeed,0);
		if(timer<=0 || transform.position.x<-Scaler.sizeX-4 || transform.position.x>Scaler.sizeX+4 || transform.position.y<-Scaler.sizeY-4 || transform.position.y>Scaler.sizeY+4) Destroy(gameObject);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform==target)target=null;
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
