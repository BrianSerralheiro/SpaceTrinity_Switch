using UnityEngine;
[System.Serializable]
public struct PilotInfo
{
   public Sprite normal,happy,sad;
   public Color color;
   public static PilotInfo[] pilot=new PilotInfo[2];
}
