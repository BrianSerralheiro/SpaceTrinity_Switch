using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DialogInfo))]
public class DialogEditor : Editor
{
    float x,y,w,h;
    Vector2 pos,scroll;
    static Speech copiedSpeech;
    static bool copied;
    public override void OnInspectorGUI(){
        h=EditorGUIUtility.singleLineHeight;
        w=EditorGUIUtility.currentViewWidth-20;
        x=10;
        y=0;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(new Rect(x,y,w,h),serializedObject.FindProperty("dialogName"));
        if(EditorGUI.EndChangeCheck())serializedObject.ApplyModifiedPropertiesWithoutUndo();
        y+=h;
        SerializedProperty p=serializedObject.FindProperty("dialogs");
        for (int i = 0; i < p.arraySize; i++)
        {
            Rect rect=new Rect(x+w-h*5,y,h*2,h);
            ShowDialog(p.GetArrayElementAtIndex(i),"Dialog "+(i+1));
            if(GUI.Button(rect,"/\\")){
                p.MoveArrayElement(i,i-1);
                p.serializedObject.ApplyModifiedProperties();
            }
            rect.x+=h*2;
            if(GUI.Button(rect,"\\/")){
                p.MoveArrayElement(i,i+1);
                p.serializedObject.ApplyModifiedProperties();
            }
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
    void ShowDialog(SerializedProperty p,string s){
        p.isExpanded=EditorGUI.BeginFoldoutHeaderGroup(new Rect(x,y,w-h*5,h),p.isExpanded,s);
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
        ShowSpeech(p.FindPropertyRelative("entries"),"Entries");
        x+=10;
        w-=10;
        ShowSpeech(p.FindPropertyRelative("failed"),"Sub-Entries");
        
        x=10;
        w+=20;
        EditorGUI.EndFoldoutHeaderGroup();
    }
    void ShowSpeech(SerializedProperty prop,string  n){
        prop.isExpanded=EditorGUI.Foldout(new Rect(x,y,w,h),prop.isExpanded,n);
        if(prop.isExpanded)y+=h;
        for (int i = 0; i < prop.arraySize && prop.isExpanded; i++)
        {
            SerializedProperty entry=prop.GetArrayElementAtIndex(i);
            x+=10;
            entry.isExpanded=EditorGUI.Foldout(new Rect(x,y,w-h*6,h),entry.isExpanded,"Speech "+i);
            y+=h;
            if(copied && GUI.Button(new Rect(x+w-h*6,y-h,h,h),"p")){
                PasteSpeech(entry);
                prop.serializedObject.ApplyModifiedProperties();
            }
            if(GUI.Button(new Rect(x+w-h*5,y-h,h,h),"c")){
                CopySpeech(entry);
            }
            if(GUI.Button(new Rect(x+w-h*3,y-h,h,h),"/\\")){
                prop.MoveArrayElement(i,i-1);
                prop.serializedObject.ApplyModifiedProperties();
            }
            if(GUI.Button(new Rect(x+w-h*4,y-h,h,h),"\\/")){
                prop.MoveArrayElement(i,i+1);
                prop.serializedObject.ApplyModifiedProperties();
            }
            if(GUI.Button(new Rect(x+w-h*2,y-h,h,h),"X")){
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
            if(chars.isExpanded)scroll=GUI.BeginScrollView(new Rect(x,y,w,h*6),scroll,new Rect(x,y,sw*(chars.arraySize+1),h*5));
            for (int j = 0; j < chars.arraySize && chars.isExpanded; j++)
            {
                sy=y;
                SerializedProperty c=chars.GetArrayElementAtIndex(j);
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("picture"),GUIContent.none);
                sy+=h;
                EditorGUI.PropertyField(new Rect(sx,sy,sw/2,h),c.FindPropertyRelative("lit"),GUIContent.none);
                EditorGUI.PropertyField(new Rect(sx+sw/2,sy,sw/2,h),c.FindPropertyRelative("shake"),GUIContent.none);
                sy+=h;
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("proportion"),GUIContent.none);
                sy+=h;
                EditorGUI.PropertyField(new Rect(sx,sy,sw,h),c.FindPropertyRelative("position"),GUIContent.none);
                sy+=h;
                if(GUI.Button(new Rect(sx,sy,sw,h),"remove"+j))chars.DeleteArrayElementAtIndex(j);
                
                sy+=h;
                sx+=sw;

            }
            if(EditorGUI.EndChangeCheck())chars.serializedObject.ApplyModifiedProperties();
            if(chars.isExpanded && GUI.Button(new Rect(sx,y,sw,h),"Add Actor"+chars.arraySize)){
                chars.InsertArrayElementAtIndex(chars.arraySize);
                chars.serializedObject.ApplyModifiedProperties();
            }
            if(chars.isExpanded)GUI.EndScrollView();
            y=sy+h;
            x-=20;
            float dw=w/3;
            GUI.Label(new Rect(x,y,dw,h),"Name");
            GUI.Label(new Rect(x+dw,y,w-dw,h),"Text Key");
            y+=h;
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(new Rect(x,y,dw,h),entry.FindPropertyRelative("name"),GUIContent.none);
            EditorGUI.PropertyField(new Rect(x+dw,y,w-dw,h),entry.FindPropertyRelative("text"),GUIContent.none);
            if(EditorGUI.EndChangeCheck())entry.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            y+=h;
            GUI.Box(new Rect(0,y,w+h*2,h),"");
            y+=h;
        }
        //y+=h;
        if(prop.isExpanded && GUI.Button(new Rect(x,y,w,h),"Add Speech")){
            prop.InsertArrayElementAtIndex(prop.arraySize);
            prop.serializedObject.ApplyModifiedProperties();
        }
        y+=h;
    }
    void CopySpeech(SerializedProperty prop){
        Speech newSpeech;
        newSpeech.name=prop.FindPropertyRelative("name").stringValue;
        newSpeech.text=prop.FindPropertyRelative("text").stringValue;
        SerializedProperty chars=prop.FindPropertyRelative("characters");
        newSpeech.characters=new Character[chars.arraySize];
        for (int j = 0; j < chars.arraySize && chars.isExpanded; j++)
        {
            SerializedProperty c=chars.GetArrayElementAtIndex(j);
            Character newChar;
            newChar.picture=c.FindPropertyRelative("picture").objectReferenceValue as Sprite;
            newChar.lit=c.FindPropertyRelative("lit").boolValue;
            newChar.shake=c.FindPropertyRelative("shake").boolValue;
            newChar.proportion=c.FindPropertyRelative("proportion").floatValue;
            newChar.position=c.FindPropertyRelative("position").floatValue;
            newSpeech.characters[j]=newChar;
        }
        copiedSpeech=newSpeech;
        copied=true;
    }
    void PasteSpeech(SerializedProperty prop){
        prop.FindPropertyRelative("name").stringValue=copiedSpeech.name;
        prop.FindPropertyRelative("text").stringValue=copiedSpeech.text;
        SerializedProperty chars=prop.FindPropertyRelative("characters");
        chars.arraySize=copiedSpeech.characters.Length;
        for (int j = 0; j < chars.arraySize && chars.isExpanded; j++)
        {
            SerializedProperty c=chars.GetArrayElementAtIndex(j);
            Character character=copiedSpeech.characters[j];
            c.FindPropertyRelative("picture").objectReferenceValue=character.picture;
            c.FindPropertyRelative("lit").boolValue=character.lit;
            c.FindPropertyRelative("shake").boolValue=character.shake;
            c.FindPropertyRelative("proportion").floatValue=character.proportion;
            c.FindPropertyRelative("position").floatValue=character.position;
            c.serializedObject.ApplyModifiedProperties();
        }
        copied=false;
    }
}
