using UnityEditor;
[CustomEditor(typeof(MenuSelect))]
public class MenuEditor : Editor
{
    public override void OnInspectorGUI(){
        EditorGUI.BeginChangeCheck();
        SerializedProperty p=serializedObject.FindProperty("options");
        EditorGUILayout.PropertyField(p,true);
        p= serializedObject.FindProperty("rowCount");
        EditorGUILayout.PropertyField(p);
        p= serializedObject.FindProperty("selector");
        EditorGUILayout.PropertyField(p);
        p= serializedObject.FindProperty("selector2");
        EditorGUILayout.PropertyField(p);
        p= serializedObject.FindProperty("displayText");
        EditorGUILayout.PropertyField(p);
        p= serializedObject.FindProperty("displayName");
        EditorGUILayout.PropertyField(p);
        p= serializedObject.FindProperty("displayImage");
        EditorGUILayout.PropertyField(p);
        if(p.objectReferenceValue!=null){
        p= serializedObject.FindProperty("sprites");
        EditorGUILayout.PropertyField(p,true);}
        p= serializedObject.FindProperty("confirmKey");
        EditorGUILayout.PropertyField(p);
        p= serializedObject.FindProperty("analog");
        p.isExpanded=EditorGUILayout.BeginFoldoutHeaderGroup(p.isExpanded,"Comands");
            if(p.isExpanded){
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.BeginHorizontal();
                p= serializedObject.FindProperty("A");
                EditorGUILayout.PropertyField(p);
                p= serializedObject.FindProperty("B");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                p= serializedObject.FindProperty("X");
                EditorGUILayout.PropertyField(p);
                p= serializedObject.FindProperty("Y");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                p= serializedObject.FindProperty("L");
                EditorGUILayout.PropertyField(p);
                p= serializedObject.FindProperty("R");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.EndHorizontal();
                p= serializedObject.FindProperty("analogDisplay");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.BeginHorizontal();
                p= serializedObject.FindProperty("Adisplay");
                EditorGUILayout.PropertyField(p);
                p= serializedObject.FindProperty("Bdisplay");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                p= serializedObject.FindProperty("Xdisplay");
                EditorGUILayout.PropertyField(p);
                p= serializedObject.FindProperty("Ydisplay");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                p= serializedObject.FindProperty("Ldisplay");
                EditorGUILayout.PropertyField(p);
                p= serializedObject.FindProperty("Rdisplay");
                EditorGUILayout.PropertyField(p);
            EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        p= serializedObject.FindProperty("opt");
        EditorGUILayout.PropertyField(p,true);
        p= serializedObject.FindProperty("menus");
        EditorGUILayout.PropertyField(p,true);
        if(EditorGUI.EndChangeCheck())serializedObject.ApplyModifiedProperties();
    }
}
