using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public int killerId;
    private HashSet<Transform> swallow=new HashSet<Transform>();
    void Awake()
    {
        Debug.Log(transform.parent);
        int.TryParse(transform.parent.name[7].ToString(),out killerId);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name.Contains("Player") || col.name.Contains("big") || col.name.Contains("Item") || col.name.Contains("Boss") || col.name.Contains("player") || col.isTrigger)return;
        EnemyBase en=col.GetComponent<EnemyBase>();
        if(en)en.enabled=false;
        Bullet bullet=col.GetComponent<Bullet>();
        if(bullet)bullet.enabled=false;
        col.transform.parent=transform;
        col.enabled=false;
        swallow.Add(col.transform);
    }
    void Update()
    {
        foreach (Transform t in swallow)
        {
            if(!t){swallow.Remove(t);
            return;}
            t.localPosition=Vector3.MoveTowards(t.localPosition,Vector3.zero,2*Time.deltaTime);
            t.localScale=Vector3.MoveTowards(t.localScale,Vector3.zero,2*Time.deltaTime);
            if(t.localPosition==Vector3.zero){
                EnemyBase en=t.GetComponent<EnemyBase>();
                swallow.Remove(t);
                if(en){
                    en.Kill(killerId);
                }
                else Destroy(t.gameObject);
                return;
            }
        }
    }
    void OnDisable()
    {
        do{
            HashSet<Transform>.Enumerator  enu= swallow.GetEnumerator();
            while(enu.MoveNext()){
                if(enu.Current){
                    EnemyBase en=enu.Current.GetComponent<EnemyBase>();
                    if(en){
                        en.Kill(killerId);
                    }
                    else Destroy(enu.Current.gameObject);
                }
                swallow.Remove(enu.Current);
                break;
            }
        }while(swallow.Count>0);
    }
}
