using UnityEngine;

public class Spiderling : EnemyBase
{
    Transform follow;
    Sprite s1,s2;
    public override void SetSprites(EnemyInfo ei)
    {
        s1=ei.sprites[1];
        s2=ei.sprites[2];
    }
    public void MoveTo(Transform t){
        follow=t;
    }
    public void Web(Transform t){
        transform.parent=t;
        transform.localRotation=Quaternion.identity;
    }
    public void OnDie(){
        Die();
    }
    new void Update()
    {
        if(Ship.paused)return;
        base.Update();
        if(transform.parent){
            transform.Translate(0,-Time.deltaTime*14,0);
            if(transform.position.y<-Scaler.sizeY-2)Die();
            _renderer.sprite=Time.time%0.5f>0.25?s1:s2;
        }
        else {
            transform.Rotate(0,0,Time.deltaTime*360);
            transform.position=Vector3.MoveTowards(transform.position,follow.position+Vector3.back/2,Time.deltaTime*10);
            if(Vector3.Distance(transform.position,follow.position+Vector3.back/2)==0)Web(follow);
        }
    }
}
