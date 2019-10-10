using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(RectTransformHelper))]
public class RectEditor : Editor
{
	public override void OnInspectorGUI()
	{

		
		if(GUILayout.Button("Snap"))
		{
			RectTransform rect = (target as RectTransformHelper).GetComponent<RectTransform>();
			Rect selfRect = rect.rect;
			Rect parentRect = (rect.parent as RectTransform).rect;
			rect.anchorMin=(new Vector2(rect.localPosition.x,rect.localPosition.y)+parentRect.size/2-rect.pivot*selfRect.size)/parentRect.size;
			rect.anchorMax=rect.anchorMin+selfRect.size/parentRect.size;

			rect.sizeDelta=Vector2.zero;
			rect.anchoredPosition=Vector2.zero;
		}
	}
}
