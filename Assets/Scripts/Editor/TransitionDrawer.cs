using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(MenuTransition))]
public class TransitionDrawer : PropertyDrawer
{
    enum FlowType{
        up,down,left,right,shrink,expand
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float x=position.x;
        position.height-=EditorGUIUtility.singleLineHeight/2;
        position.x+=EditorGUIUtility.singleLineHeight/2;
        GUI.Box(position,"");
        position.x=x;
        position.height=EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position,property.FindPropertyRelative("menu"));
        position.y+=position.height;
        EditorGUI.PropertyField(position,property.FindPropertyRelative("key"));
        position.y+=position.height;
        SerializedProperty flow=property.FindPropertyRelative("flow");
        FlowType flowType=FlowType.up;
        if(flow.vector3Value==Vector3.one)flow.vector3Value=Vector3.up;
        if(flow.vector3Value.x==1)flowType=FlowType.right;
        if(flow.vector3Value.x==-1)flowType=FlowType.left;
        if(flow.vector3Value.y==-1)flowType=FlowType.down;
        if(flow.vector3Value.z==1)flowType=FlowType.expand;
        if(flow.vector3Value.z==-1)flowType=FlowType.shrink;
        EditorGUI.BeginChangeCheck();
            flowType=(FlowType)EditorGUI.EnumPopup(position,"Transition type",flowType);
        if(EditorGUI.EndChangeCheck()){
            switch (flowType)
            {
                case FlowType.up:
                    flow.vector3Value=Vector3.up;
                    break;
                case FlowType.down:
                    flow.vector3Value=Vector3.down;
                    break;
                case FlowType.left:
                    flow.vector3Value=Vector3.left;
                    break;
                case FlowType.right:
                    flow.vector3Value=Vector3.right;
                    break;
                case FlowType.shrink:
                    flow.vector3Value=Vector3.back;
                    break;
                case FlowType.expand:
                    flow.vector3Value=Vector3.forward;
                    break;
            }
        }
        position.y+=position.height;
        if(flowType==FlowType.shrink || flowType==FlowType.expand)
            EditorGUI.PropertyField(position,property.FindPropertyRelative("expander"));
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(property.FindPropertyRelative("flow").vector3Value.z!=0)return EditorGUIUtility.singleLineHeight*4.5f;
        return EditorGUIUtility.singleLineHeight*3.5f;
    }
}
