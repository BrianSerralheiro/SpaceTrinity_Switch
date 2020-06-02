using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStage : MonoBehaviour
{
    [SerializeField]
    MenuSelect menu;
    [SerializeField]
    WorldInfo world7,world8;
    [SerializeField]
    RectTransform selector;
    RectTransform selected;
    [SerializeField]
    GameObject crack;
    float time;
    void Start()
    {
        crack.SetActive(Locks.Boss(6));
        if(Locks.Boss()<6)gameObject.SetActive(false);
    }
    void Update()
    {
        if(PlayerInput.GetKeySpecial(0)){
            if(PlayerInput.GetKeySpecialDown(0)){
                selected=menu.GetOption();
                time=Time.time+2;
            }
            selector.position=Vector3.MoveTowards(selector.position,transform.position,Time.deltaTime/2);
            selector.anchorMin=Vector2.MoveTowards(selector.anchorMin,((RectTransform)transform).anchorMin,Time.deltaTime/2);
            selector.anchorMax=Vector2.MoveTowards(selector.anchorMax,((RectTransform)transform).anchorMax,Time.deltaTime/2);
            selector.rotation=Quaternion.RotateTowards(selector.rotation,transform.rotation,30*Time.deltaTime);
            menu.enabled=false;
            if(time<Time.time){
                Loader.Scene("WorldLoader");
                EnemySpawner.world=Locks.Boss(6)?world8:world7;
            }
        }else if(selected){
            selector.position=Vector3.MoveTowards(selector.position,selected.position,10*Time.deltaTime);
            selector.anchorMin=Vector2.MoveTowards(selector.anchorMin,selected.anchorMin,10*Time.deltaTime);
            selector.anchorMax=Vector2.MoveTowards(selector.anchorMax,selected.anchorMax,10*Time.deltaTime);
            selector.rotation=Quaternion.RotateTowards(selector.rotation,selected.rotation,90*Time.deltaTime);
            if(selector.position==selected.position){
                menu.enabled=true;
                selected=null;
            }
        }
    }
}
