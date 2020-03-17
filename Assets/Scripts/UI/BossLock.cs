using UnityEngine.UI;
using UnityEngine;

public class BossLock : MonoBehaviour
{
    Image image;
    [SerializeField]
    int id;
    [SerializeField]
    Color live,defeat;
    void Start()
    {
        image=GetComponent<Image>();
        image.color=Locks.Boss(id)?defeat:live;
    }

    void Update()
    {
        
    }
}
