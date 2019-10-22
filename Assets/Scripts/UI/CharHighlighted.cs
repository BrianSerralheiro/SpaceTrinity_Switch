using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharHighlighted : MonoBehaviour
{
    [SerializeField]
    private Graphic selector;

    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect.GetComponent<RectTransform>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
