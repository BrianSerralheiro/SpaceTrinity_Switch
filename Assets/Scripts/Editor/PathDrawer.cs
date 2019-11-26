using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(BulletPath))]
public class PathDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width/=2;
        EditorGUI.PropertyField(position,property.FindPropertyRelative("speed"));
        position.x+=position.width;
        if(GUI.Button(position,"Edit path")){
            PathWindow.ShowWindow(property);
        }
    }
}
