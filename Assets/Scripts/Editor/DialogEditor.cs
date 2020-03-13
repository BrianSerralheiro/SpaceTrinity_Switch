using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DialogInfo))]
public class DialogEditor : Editor
{
    float x,y,w,h;
    Vector2 pos;
    public override void OnInspectorGUI(){
        h=EditorGUIUtility.singleLineHeight;
        w=EditorGUIUtility.currentViewWidth-20;
        x=10;
        y=0;
        // GUI.Box(new Rect(x,y,w,h*8),"");
        // DrawDefaultInspector();
        SerializedProperty p=serializedObject.FindProperty("dialogs");
        for (int i = 0; i < p.arraySize; i++)
        {
            Rect rect=new Rect(x+w-h*5,y,h*2,h);
            ShowDialog(p.GetArrayElementAtIndex(i),i);
            GUI.Button(rect,"/\\");
            rect.x+=h*2;
            GUI.Button(rect,"\\/");
            rect.x+=h*2;
            rect.width=h;
            if(GUI.Button(rect,"x")){
                p.DeleteArrayElementAtIndex(i);
                p.serializedObject.ApplyModifiedProperties();
                return;
            }
        }
        if(GUI.Button(new Rect(x,y,w,h),"Add new Dialog")){
            p.InsertArrayElementAtIndex(p.arraySize);
            p.serializedObject.ApplyModifiedProperties();
        }
        // GUI.Box(new Rect(x,y,w,h),"");
        
        EditorGUILayout.GetControlRect(false,y+h);

    }
    void ShowDialog(SerializedProperty p,int id){
        p.isExpanded=EditorGUI.BeginFoldoutHeaderGroup(new Rect(x,y,w-h*5,h),p.isExpanded,"Dialog"+id);
        y+=h;
        if(!p.isExpanded){
            EditorGUI.EndFoldoutHeaderGroup();
            return;
        }
        x+=20;
        w-=20;
        SerializedProperty prop=p.FindPropertyRelative("conditions");
        prop.isExpanded=EditorGUI.Foldout(new Rect(x,y,w,h),prop.isExpanded,"Conditions");
        y+=h;
        if(prop.isExpanded){
            x+=4;
            w-=4;
            float s2=x;
            SerializedProperty vector=p.FindPropertyRelative("vector");
            if(prop.arraySize==0){
                prop.InsertArrayElementAtIndex(0);
                prop.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.BeginChangeCheck();
            vector.vector2Value=GUI.BeginScrollView(new Rect(x,y,w,h*4),vector.vector2Value,new Rect(x,y,w/4*(prop.arraySize+1),h*3));
            if(EditorGUI.EndChangeCheck())p.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            for (int i = 0; i < prop.arraySize; i++)
            {
                float s1=w/4;
                SerializedProperty c=prop.GetArrayElementAtIndex(i);
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(new Rect(s2,y,s1,h),c.FindPropertyRelative("conditionType"),GUIContent.none);
                y+=h;
                //s2+=s1;
                EditorGUI.PropertyField(new Rect(s2,y,s1,h),c.FindPropertyRelative("id"),GUIContent.none);
                y+=h;
                if(prop.arraySize>1 && GUI.Button(new Rect(s2,y,s1,h),"remove"))prop.DeleteArrayElementAtIndex(i);
                if(EditorGUI.EndChangeCheck())c.serializedObject.ApplyModifiedProperties();
                y+=h;
                s2+=s1;
                y-=h*3;
            }
            if(GUI.Button(new Rect(s2,y+h,w/4,h),"Add Condition")){
                prop.InsertArrayElementAtIndex(prop.arraySize);
                prop.serializedObject.ApplyModifiedProperties();
            }
            GUI.EndScrollView();
            y+=h*4;
            x-=4;
            w+=4;
        }
        // y+=h;
        prop=p.FindPropertyRelative("entries");
        prop.isExpanded=EditorGUI.Foldout(new Rect(x,y,w,h),prop.isExpanded,"Entries");
        if(prop.isExpanded)y+=h;
        for (int i = 0; i < prop.arraySize && prop.isExpanded; i++)
        {
            SerializedProperty entry=prop.GetArrayElementAtIndex(i);
            x+=10;
            entry.isExpanded=EditorGUI.Foldout(new Rect(x,y,w-h,h),entry.isExpanded,"Speech "+i);
            y+=h;
            if(GUI.Button(new Rect(x+w-h,y-h,h,h),"X")){
                prop.DeleteArrayElementAtIndex(i);
                prop.serializedObject.ApplyModifiedProperties();
                continue;
            }
            if(!entry.isExpanded){
                x-=10;
                continue;
            }
            SerializedProperty chars=entry.FindPropertyRelative("characters");
            x+=10;
            chars.isExpanded=EditorGUI.Foldout(new Rect(x,y,w,h),chars.isExpanded,"Actors");
            if(chars.isExpanded)y+=h;
            float sw=w/4,sx=x,sy=y;
            EditorGUI.BeginChangeCheck();
            for (int j = 0; j < chars.arraySize && chars.isExpanded; j++)
            {
                sy=y;
                SerializedProperty c=chars.GetArrayElementAtIndex(j);
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("picture"),GUIContent.none);
                sy+=h;
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("lit"),GUIContent.none);
                sy+=h;
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("proportion"),GUIContent.none);
                sy+=h;
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("position"),GUIContent.none);
                sy+=h;
                if(GUI.Button(new Rect(sx,sy,sw,h),"remove"))chars.DeleteArrayElementAtIndex(j);
                
                sy+=h;
                sx+=sw;

            }
            if(EditorGUI.EndChangeCheck())chars.serializedObject.ApplyModifiedProperties();
            if(chars.isExpanded && GUI.Button(new Rect(sx,y,sw,h),"Add Actor")){
                chars.InsertArrayElementAtIndex(chars.arraySize);
                chars.serializedObject.ApplyModifiedProperties();
            }
            y=sy+h;
            x-=20;
            GUI.Label(new Rect(x,y,w,h),"Text");
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(new Rect(x,y,w,h*3),entry.FindPropertyRelative("text"),GUIContent.none);
            if(EditorGUI.EndChangeCheck())entry.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            y+=h*3;
        }
        //y+=h;
        if(prop.isExpanded && GUI.Button(new Rect(x,y,w,h),"Add Speech")){
            prop.InsertArrayElementAtIndex(prop.arraySize);
            prop.serializedObject.ApplyModifiedProperties();
        }
        y+=h;
        x=10;
        w+=20;
        EditorGUI.EndFoldoutHeaderGroup();
    }
}
