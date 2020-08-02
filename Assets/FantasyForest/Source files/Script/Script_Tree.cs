using UnityEngine;

public class Script_Tree : MonoBehaviour {

    [Header("How hard the tree is swaying:")]
    public float windMultiplier = 1f;




    float rotationIntensity;
    float rotationFrequency;
    float currentTreeRotation;

    float updateTreeRotationFrequency=0.1f;
    float updateTreeRotationTimer;

    float checkLODTimer;

    [HideInInspector]
    public float refreshTimer; // cooldown timer to prevent refreshing the tree multiple times in a row, used when the tree becomes visible

    int currentLod;

    float time;

    LODGroup lodGroup;
    [HideInInspector]
    public bool isVisible;

    void Start() {
        rotationIntensity = UnityEngine.Random.Range(1f, 3f) * windMultiplier; //how many angles the tree bends
        rotationFrequency = UnityEngine.Random.Range(0.5f, 1f) * windMultiplier; // rotatation frequency

        lodGroup = GetComponent<LODGroup>();


        UpdateLOD();

    }

    void Update() {
        if (refreshTimer > 0) {
            refreshTimer -= 1 * Time.deltaTime;
        }
        if (isVisible) {
            time += 1 * Time.deltaTime;
            updateTreeRotationTimer -= 1 * Time.deltaTime;
            if (updateTreeRotationTimer <= 0) {
                updateTreeRotationTimer = updateTreeRotationFrequency;
                UpdateTree();
            }
        }
        checkLODTimer -= 1 * Time.deltaTime;
        if (checkLODTimer <= 0) {
            checkLODTimer = 3;
            UpdateLOD();
        }
    }
    void UpdateTree() {
        currentTreeRotation = Mathf.Sin(time * rotationFrequency) * rotationIntensity;
        transform.localRotation = Quaternion.Euler(currentTreeRotation, transform.localEulerAngles.y, currentTreeRotation);
    }

    
   public void UpdateLOD() {
        isVisible = false;
        for(int i=0; i<lodGroup.GetLODs().Length; i++) { // check current LOD level from the LODGroup component
            LOD l = lodGroup.GetLODs()[i];
            for(int a=0; a<l.renderers.Length; a++) {
                Renderer r = l.renderers[a];
                if (r.isVisible) {
                    currentLod = i;
                    isVisible = true;
                    break;
                }
            }
        }
        if (currentLod == 0) {
            updateTreeRotationFrequency = 0.025f;
        } else if (currentLod == 1) {
            updateTreeRotationFrequency = 0.05f;
        } else if (currentLod == 2) {
            updateTreeRotationFrequency = 0.1f;
        }
    }
}
