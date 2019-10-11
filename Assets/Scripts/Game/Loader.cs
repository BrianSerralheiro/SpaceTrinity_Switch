using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static string sceneToLoad;
    void Start()
    {
        if(string.IsNullOrEmpty(sceneToLoad)){
            SceneManager.LoadSceneAsync("SelectionTest");
        }else SceneManager.LoadSceneAsync(sceneToLoad);
        sceneToLoad=null;
    }
    void Update()
    {
        transform.Rotate(0,0,180*Time.deltaTime);
    }
    public static void Scene(string s){
        if(string.IsNullOrEmpty(sceneToLoad)){
            sceneToLoad=s;
            SceneManager.LoadSceneAsync("loading");
        }
    }
}
