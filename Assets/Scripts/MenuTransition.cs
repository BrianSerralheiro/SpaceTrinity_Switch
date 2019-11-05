using UnityEngine.UI;
using UnityEngine;
[System.Serializable]
public struct MenuTransition
{
    public KeyCode key;
    public MenuSelect menu, closing;
    public bool scaler;
    public Vector3 flow;
    public Image expander;
    private float timer;
    public bool GetKeyDown(){
        return Input.GetKeyDown(key);
    }
    public void Open(){
        if(flow.z==1)
            menu?.Expand(expander);
            else if(flow.z==-1){
                menu.gameObject.SetActive(true);
                menu.Open(TimedOpen);
            }
        else {
            float f=(float)Screen.width/(float)Screen.height;
            menu?.Open(new Vector3(flow.x*f*10,flow.y*10));
        }
    }
    void TimedClose(){
		if((timer+=Time.deltaTime)>0.5f){
            closing.Open(null);
            closing.gameObject.SetActive(false);
        }
    }
    void TimedOpen(){
		if((timer+=Time.deltaTime)>0.5f)
            menu.GetInput();
    }
    public void Close(MenuSelect m){
        if(flow.z==1){
            closing=m;
            timer=0;
            m?.Open(TimedClose);
        }
        else if(flow.z==-1){
            m.Shrink(expander.rectTransform);
        }
        else{
            float f=(float)Screen.width/(float)Screen.height;
            m.Close(new Vector3(-flow.x*f*10,-flow.y*10));
        };
    }

}
