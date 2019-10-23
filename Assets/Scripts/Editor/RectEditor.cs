using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class RectEditor
{
	[MenuItem("UI/Parent size")]
	public static void ParentSize(){
		foreach(GameObject go in Selection.gameObjects){	
			RectTransform rect = (go as GameObject).GetComponent<RectTransform>();
			if(!rect)continue;
			rect.anchorMin=Vector2.zero;
			rect.anchorMax=Vector2.one;
			rect.sizeDelta=Vector2.zero;
			rect.anchoredPosition=Vector2.zero;
		}
	}
	[MenuItem("UI/Snap")]
	public static void Snap()
	{
		foreach(GameObject go in Selection.gameObjects){	
			RectTransform rect = (go as GameObject).GetComponent<RectTransform>();
			if(!rect)continue;
			Rect selfRect = rect.rect;
			Rect parentRect = (rect.parent as RectTransform).rect;
			rect.anchorMin=(new Vector2(rect.localPosition.x,rect.localPosition.y)+parentRect.size/2-rect.pivot*selfRect.size)/parentRect.size;
			rect.anchorMax=rect.anchorMin+selfRect.size/parentRect.size;
			rect.sizeDelta=Vector2.zero;
			rect.anchoredPosition=Vector2.zero;
		}
	}
	[MenuItem("UI/Clear Missing Scripts")]
    static void SelectGameObjects()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();
 
        List<Object> list = new List<Object>();
        foreach (GameObject g in rootObjects)
        {
            ClearObject(list,g);
        }
        if (list.Count > 0)
        {
            Selection.objects = list.ToArray();
        }
        else
        {
            Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts!");
        }
    }
	private static void ClearObject(List<Object> list,GameObject go){
		Component[] components = go.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			Component currentComponent = components[i];
			if (currentComponent == null)
			{
				list.Add(go);
				Selection.activeGameObject = go;
				break;
			}
		}
		for(int i=0;i<go.transform.childCount;i++){
			ClearObject(list,go.transform.GetChild(i).gameObject);
		}
	}
}
