using UnityEngine;
public class InputManager : ScriptableObject
{
    public PlayerInput[] players;
    public void Load()
    {
        PlayerInput.players=players;
    }
}
[System.Serializable]
public struct PlayerInput{
    public static bool recentConect,recentDisconect;
    public int id;
    public string shoot,special,equip1,equip2,Lbumper,Rbumper,vertical,horizontal,name;
    public static PlayerInput[] players;
    public bool got,conected;
    public Vector3 GetAxis()
    {
        return new Vector3(Input.GetAxis(horizontal),Input.GetAxis(vertical),0);
    }
    public PlayerInput Select(){
        got=true;
        return this;
    }
    public static bool GetButtomDown(string s){
        switch(s){
            case "shot":
                return Input.GetButtonDown(players[0].shoot);
            case "special":
                return Input.GetButtonDown(players[0].special);
            case "equip1":
                return Input.GetButtonDown(players[0].equip1);
            case "equip2":
                return Input.GetButtonDown(players[0].equip2);
            case "Lbumper":
                return Input.GetButtonDown(players[0].Lbumper);
            case "Rbumper":
                return Input.GetButtonDown(players[0].Rbumper);
            default:
                return false;
        }
    }public bool GetKey(string s){
        switch(s){
            case "shoot":
                return Input.GetButton(shoot);
            case "special":
                return Input.GetButton(special);
            case "equip":
                return Input.GetButton(equip1);
            default:
                return false;
        }
    }
    public static float GetDir(int i,bool vert){
        if(vert)
            return Input.GetAxisRaw(players[i].vertical);
        return Input.GetAxisRaw(players[i].horizontal);
    }
    public static bool GetKeyEquip(int i){
        if(i<players.Length)return Input.GetButton(players[i].equip1);
        return false;
    }
    public static bool GetKeySpecial(int i){
        if(i<players.Length)return Input.GetButton(players[i].special);
        return false;
    }
    public static bool GetKeyShot(int i){
        if(i<players.Length)return Input.GetButton(players[i].shoot);
        return false;
    }
    public static bool GetKeyEquipDown(int i){
        if(i<players.Length)return Input.GetButtonDown(players[i].equip1);
        return false;
    }
    public static bool GetKeySpecialDown(int i){
        if(i<players.Length)return Input.GetButtonDown(players[i].special);
        return false;
    }
    public static bool GetKeyShotDown(int i){
        if(i<players.Length)return Input.GetButtonDown(players[i].shoot);
        return false;
    }
    public static void WaitInput(){
        recentConect=false;
        recentDisconect=false;
        for(int i=0;i<players.Length;i++){
            if(players[i].conected){
                if(Input.GetButtonDown(players[i].equip1))DisConnected(i);
            }
            else{
                if(Input.GetButtonDown(players[i].special))Connect(i);
            }
        }
    }
    public static void DisConnected(int i){
        players[i].conected=false;
        recentDisconect=true;
    }
    public static void Connect(int i){
        players[i].conected=true;
        recentConect=true;
    }
    public static bool Conected(int i){
        return players[i].conected;
    }
    public static void Unload(){
        players[0].got=false;
    }
    public static PlayerInput Get(){
        if(players[0].got)return players[1].Select();
        return players[0].Select();
    }
}
