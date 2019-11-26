using UnityEngine;
using UnityEditor;

public class PathWindow : EditorWindow
{
    private static SerializedProperty property;
    Vector2 limit=new Vector2(8,10);
    Vector3 mid=new Vector3(4,5);
    static int dragID;
    static bool toggle;
    static readonly Color[] colors={Color.black,Color.blue,Color.cyan,Color.gray,Color.green,Color.magenta,Color.red,Color.white,Color.yellow};
    public static void ShowWindow(SerializedProperty s)
	{
        property=s;
        dragID=-1;
		EditorWindow.GetWindow(typeof(PathWindow));
	}
    void OnGUI()
    {
        wantsMouseMove=true;
        if(property==null)return;
        SerializedProperty nodes=property.FindPropertyRelative("nodes");
        SerializedProperty curves=property.FindPropertyRelative("curves");
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Clear")){
                nodes.ClearArray();
                curves.ClearArray();
            }
            toggle=EditorGUILayout.Toggle("Show fields",toggle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.MinMaxSlider("Limits",ref limit.x,ref limit.y,4,20);
            limit.x=(int)limit.x;
            limit.y=(int)limit.y;
            EditorGUILayout.LabelField(limit.x+" "+limit.y);

            if(EditorGUI.EndChangeCheck())mid=limit/2;
        GUILayout.EndHorizontal();
        if(toggle){
            EditorGUILayout.PropertyField(nodes,true);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("curves"),true);
            nodes.serializedObject.ApplyModifiedProperties();
        }
        else{
        Vector3 v1;
        int i;
        Vector3 proportion=new Vector3(position.width/limit.x,position.height/limit.y);
        bool draging=Event.current.type==EventType.MouseDown && Event.current.button==0;
        if(Event.current.type==EventType.MouseUp && Event.current.button==0 && dragID>=0){
            nodes.GetArrayElementAtIndex(dragID).vector3Value=Round(nodes.GetArrayElementAtIndex(dragID).vector3Value);
            dragID=-1;
            nodes.serializedObject.ApplyModifiedProperties();
        }
        for (i = 0; i < nodes.arraySize; i++)
        {
            v1=nodes.GetArrayElementAtIndex(i).vector3Value;
            if(draging && MouseAround(Mul(Inv(v1)+mid,proportion)))
                dragID=i;
            else if(dragID==i){
                v1=Inv(Div(Event.current.mousePosition,proportion)-mid);
                nodes.GetArrayElementAtIndex(i).vector3Value=v1;
                Repaint();
            }
            Handles.color=colors[i%colors.Length];
            GUI.Label(new Rect((v1.x+mid.x)*proportion.x,(-v1.y+mid.y)*proportion.y,EditorGUIUtility.singleLineHeight*4,EditorGUIUtility.singleLineHeight),ToString(v1));
            if(i>0)Handles.DrawLine(Mul(Inv(v1)+mid,proportion),Mul(Inv(nodes.GetArrayElementAtIndex(i-1).vector3Value)+mid,proportion));
            else Handles.DrawLine(Mul(Inv(v1)+mid,proportion),Mul(mid,proportion));
        }
        if(dragID==-1 && Event.current.type==EventType.MouseUp && Event.current.button==1){
            nodes.InsertArrayElementAtIndex(nodes.arraySize);
            curves.InsertArrayElementAtIndex(curves.arraySize);
            nodes.GetArrayElementAtIndex(nodes.arraySize-1).vector3Value=Inv(Div(Event.current.mousePosition,proportion)-mid);
            Repaint();
            nodes.serializedObject.ApplyModifiedProperties();
        }
        if(dragID!=-1 && Event.current.keyCode==KeyCode.Space){
            nodes.DeleteArrayElementAtIndex(dragID);
            curves.DeleteArrayElementAtIndex(dragID);
            nodes.serializedObject.ApplyModifiedProperties();
            dragID=-1;
        }
        }
    }
    Vector3 Inv(Vector3 v){
        v.y*=-1;
        return v;
    }
    Vector3 Round(Vector3 v){
        v.x=(int)(v.x*100)/100f;
        v.y=(int)(v.y*100)/100f;
        return v;
    }
    bool MouseAround(Vector3 v){
        Vector3 v2=Event.current.mousePosition;
        return Vector3.Distance(v,v2)<EditorGUIUtility.singleLineHeight;
    }
    string ToString(Vector3 v){
        return v.x+":"+v.y;
    }
    Vector3 Div(Vector3 v1,Vector3 v2){
        v1.x/=v2.x;
        v1.y/=v2.y;
        return v1;
    }
    Vector3 Mul(Vector3 v1,Vector3 v2){
        v1.x*=v2.x;
        v1.y*=v2.y;
        return v1;
    }
}
