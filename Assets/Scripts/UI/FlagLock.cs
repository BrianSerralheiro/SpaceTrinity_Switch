using UnityEngine.UI;
using UnityEngine;
using LanguagePack;
[RequireComponent(typeof(Image))]
public class FlagLock : MonoBehaviour
{
    [SerializeField]
    Color off;
    [SerializeField]
    int id;
    Image image;
    void Awake()
    {
        image=GetComponent<Image>();
        image.color=Language.id==id?Color.white:off;
    }

    void Update()
    {
        image.color=Language.id==id?Color.white:off;
    }
}
