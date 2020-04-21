using UnityEngine;
using UnityEditor;

public class PathWindow : EditorWindow
{
    private static SerializedProperty property;
    Vector2 limit=new Vector2(100,100),scroll;
    Vector3 mid=new Vector3(50,50);
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
        if(Event.current.isScrollWheel){
            limit.y+=Event.current.delta.y;
            limit.x+=Event.current.delta.y;
            mid=limit/2;
            Repaint();
        }            
        Vector3 proportion=new Vector3(position.width/limit.x,position.height/limit.y);
        // for (int j = 0; j < limit.x && !toggle; j++)
        // {
        //     for (int k = 0; k < limit.y; k++)
        //     {
        //         GUI.Box(new Rect(j*proportion.x,k*proportion.y,proportion.x,proportion.y),"");
        //     }
        // }
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Clear")){
                nodes.ClearArray();
                nodes.serializedObject.ApplyModifiedProperties();
            }
            toggle=EditorGUILayout.Toggle("Show fields",toggle);
            EditorGUILayout.LabelField("Scale "+(int)limit.x+"X");
        GUILayout.EndHorizontal();
        if(toggle){
            scroll=EditorGUILayout.BeginScrollView(scroll,false,false);
            EditorGUILayout.PropertyField(nodes,true);
            nodes.serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
        }
        else{
        Vector3 v1;
        int i;
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
                v1=Round(v1);
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
            nodes.GetArrayElementAtIndex(nodes.arraySize-1).vector3Value=Round(Inv(Div(Event.current.mousePosition,proportion)-mid));
            Repaint();
            nodes.serializedObject.ApplyModifiedProperties();
        }
        if(dragID!=-1 && Event.current.keyCode==KeyCode.X){
            nodes.DeleteArrayElementAtIndex(dragID);
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
        v.x=(int)((v.x)*10)/10f;
        v.y=(int)((v.y)*10)/10f;
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
