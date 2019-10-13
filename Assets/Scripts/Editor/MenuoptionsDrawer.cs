using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
[CustomPropertyDrawer(typeof(Menuoptions))]
public class MenuOptionsDrawer:PropertyDrawer
{
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        SerializedProperty selection=property.FindPropertyRelative("selection");
        Rect pos=new Rect(position.x,position.y,position.width,EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(pos,selection);
        if(selection.enumValueIndex==0){
            
            SerializedProperty worlds=property.FindPropertyRelative("worlds");
            EditorGUI.PropertyField(new Rect(position.x,position.y+pos.height,position.width,position.height),worlds,true);
        }
    } 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(property.FindPropertyRelative("selection").enumValueIndex==0)
            return EditorGUI.GetPropertyHeight(property)-EditorGUIUtility.singleLineHeight;
        return EditorGUIUtility.singleLineHeight;
    }
}