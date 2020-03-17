using UnityEngine.UI;
using UnityEngine;

public class EquipLock : MonoBehaviour
{
    [SerializeField]
    Color offColor,onColor=Color.white;
    [SerializeField]
    int id;
    Image image;
    void Start()
    {
        image=GetComponent<Image>();
        if(!Locks.Boss(id-1)){
            image.raycastTarget=false;
            image.color=Color.black;
            enabled=false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        image.raycastTarget=Ship.equips[0]!=id && Ship.equips[1]!=id;
        image.color=image.raycastTarget?onColor:offColor;
    }
}
