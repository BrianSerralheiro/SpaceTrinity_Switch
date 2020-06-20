using UnityEngine;
namespace City
{
    public class Lane:MonoBehaviour{
        [SerializeField]
        GameObject fill;
        float size=30;
        [SerializeField]
        Square[] squares;
        void Start()
        {
            foreach (Square square in squares)
            {
                square.Generate(fill,transform,size);
            }
            Destroy(this);
        }
    }
    [System.Serializable]
    public struct Square
    {
        public float fillChance;
        public Vector2 offset,gridSize;
        public GameObject[] prefabs;
        public void Generate(GameObject fill,Transform t,float size){
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    GameObject.Instantiate(Random.value>fillChance?prefabs[Random.Range(0,prefabs.Length)]:fill,t).transform.localPosition=new Vector3(offset.x*size+i*size,0,offset.y*size+j*size);
                }
            }
        }
    }
}