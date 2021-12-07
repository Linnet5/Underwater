using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorPlacement : MonoBehaviour
{
    public GameObject[] Decor = new GameObject[7];

    private GameObject currentObject;

    private List<GameObject> allObjects; // All decor objects for this tile.

    [SerializeField] private float offsetY;

    private int previousXflora;
    private int previousXshell;
    private int previousZflora;
    private int previousZshell;

    void Awake() {
        allObjects = new List<GameObject>();
        previousXflora = 0;
        previousZflora = 0;
    }
    public void PlaceDecor(int x, float y, int z, Vector3 worldPos) {
        
        // FIX PLACEMENT SO IT'S CONSISTENT (USE NOISE AGAIN??)

        //Flora
        if((y > -0.5f && y < 0.1f) || (y > 0.15 && y < 0.18) || (y > 0.20 && y < 0.22) || (y > 0.25 && y < 0.30)  || (y > 0.45 && y < 0.55)) {
                //int index = Mathf.RoundToInt(Mathf.Abs(worldPos.x) + x + Mathf.Abs(worldPos.z) + z);
                //index = index%5;
                currentObject = GameObject.Instantiate(Decor[0]);
                currentObject.transform.position = new Vector3(worldPos.x + x, y + offsetY, worldPos.z + z);
                allObjects.Add(currentObject);
        }


        //Seashells
        if(y > 0.79 && y < 0.82 && (worldPos.x + x%2 == 0)) {

                int index = Mathf.RoundToInt(Mathf.Abs(worldPos.x + x) + Mathf.Abs(worldPos.z + z));
                index = index%2 + 5;
                currentObject = GameObject.Instantiate(Decor[index]);
                if(currentObject) {
                    currentObject.transform.position = new Vector3(worldPos.x + x, y, worldPos.z + z);
                    allObjects.Add(currentObject);
                }
        }

        
    }

    public void DestroyDecor() {
        //  For now all decor is detroyed and reloaded upon entering new chunk. This is not optimal.
        //  We want the decor to be seamless like the terrain.
        foreach(GameObject element in allObjects) {
            Destroy(element);
        }
        allObjects.Clear();
    }
}
