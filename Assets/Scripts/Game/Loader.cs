using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Image))]
public class Loader : MonoBehaviour
{
    private static string sceneToLoad;
    [SerializeField]
    private Sprite[] chibis;
    void Start()
    {
        if(string.IsNullOrEmpty(sceneToLoad)){
            SceneManager.LoadSceneAsync("SelectionTest");
        }else SceneManager.LoadSceneAsync(sceneToLoad);
        sceneToLoad=null;
        GetComponent<Image>().sprite=chibis[Random.Range(0,chibis.Length)];
    }
    void Update()
    {
        transform.Rotate(0,0,90*Time.deltaTime);
    }
    public static void Scene(string s){
        if(string.IsNullOrEmpty(sceneToLoad)){
            sceneToLoad=s;
            SceneManager.LoadSceneAsync("loading");
        }
    }
}
