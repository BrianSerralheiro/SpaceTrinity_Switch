using UnityEngine;
[System.Serializable]
public struct MenuTransition
{
    public KeyCode key;
    public MenuSelect menu;
    public Vector3 flow;
    public bool GetKeyDown(){
        return Input.GetKeyDown(key);
    }
    public void Open(){
        float f=(float)Screen.width/(float)Screen.height;
        menu?.Open(new Vector3(flow.x*f*10,flow.y*10));
    }
    public void Close(MenuSelect m){
        float f=(float)Screen.width/(float)Screen.height;
        m.Close(new Vector3(-flow.x*f*10,-flow.y*10));
    }
}
