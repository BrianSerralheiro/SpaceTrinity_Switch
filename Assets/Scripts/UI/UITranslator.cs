using UnityEngine.UI;
using UnityEngine;

using LanguagePack;
[RequireComponent(typeof(Text))]
public class UITranslator:MonoBehaviour
{
    string text;
    Text UItext;
    void Awake()
    {
        UItext=GetComponent<Text>();
    }
    void Update()
    {
        if(UItext.text!=text)UItext.text=text=Language.GetMenu(UItext.text);
    }
}
