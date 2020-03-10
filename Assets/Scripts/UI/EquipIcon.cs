using UnityEngine.UI;
using UnityEngine;

public class EquipIcon : MonoBehaviour
{
    [SerializeField]
    int playerID,equipID,equiped;
    [SerializeField]
    Image icon,bg;
    [SerializeField]
    Text timer;
    [SerializeField]
    Sprite[] icons;
    static Sprite[] _icons;
    void Start()
    {
        if(playerID==0 && equipID==1 && PlayerInput.Conected(1))gameObject.SetActive(false);
        if(playerID==1 && !PlayerInput.Conected(1))gameObject.SetActive(false);
        if(Ship.equips[equipID]==0)gameObject.SetActive(false);
        if(_icons==null)_icons=icons;
        equiped=Ship.equips[playerID+equipID];
        icon.sprite=bg.sprite=_icons[equiped--];
        timer.text="";
    }
    void Update()
    {
        if(Ship.equipTime[equiped]>Time.time){
            if(Time.time+1>Ship.equipTime[equiped]){
                float f=Ship.equipTime[equiped]-Time.time;
                icon.fillAmount=1-f;
                timer.text=((int)(f*10)/10f).ToString();
            }
            else{
                icon.fillAmount=0;
                timer.text=(int)(Ship.equipTime[equiped]-Time.time)+"";
            }
        }
        else {
            icon.fillAmount=Ship.equipTime[equiped]==1?0:1;
            timer.text="";
        }
    }
}
