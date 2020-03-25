using UnityEngine.UI;
using UnityEngine;

using LanguagePack;
[RequireComponent(typeof(Text))]
public class UITranslatorRT:MonoBehaviour
{
    public delegate void UIUpdate();
    static UIUpdate update;
    string key;
    Text UItext;
    void Awake()
    {
        UItext=GetComponent<Text>();
        key=UItext.text;
        update+=UpdateUI;
        UItext.text=Language.GetMenu(key);
    }
    void UpdateUI()
    {
        UItext.text=Language.GetMenu(key);
    }
    public static void Update(){
        update?.Invoke();
    }
    void OnDestroy()
    {
        update-=UpdateUI;
    }
}
