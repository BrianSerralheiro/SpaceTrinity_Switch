using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(WorldInfo))]
public class WorldInfoEditor : Editor
{
    AudioClip clip;
    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();
        WorldInfo w=target as WorldInfo;
        if(w!=null){
            EditorGUI.BeginChangeCheck();
            if(string.IsNullOrEmpty(w.songName)){
                clip=EditorGUILayout.ObjectField("BG Music",null,typeof(AudioClip),false)as AudioClip;
                if(clip!=null)w.songName=clip.name;
            }
            else {
                GUILayout.BeginHorizontal();
                    GUILayout.Label("BG Music "+w.songName);
                    if(GUILayout.Button("Remove"))w.songName="";
                GUILayout.EndHorizontal();
            }
            if(string.IsNullOrEmpty(w.bossSong)){
                clip=EditorGUILayout.ObjectField("Boss Music",null,typeof(AudioClip),false)as AudioClip;
                if(clip!=null)w.bossSong=clip.name;
            }
            else {
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Boss Music "+w.bossSong);
                    if(GUILayout.Button("Remove"))w.bossSong="";
                GUILayout.EndHorizontal();
            }
           if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(w);
        }
    }
}
