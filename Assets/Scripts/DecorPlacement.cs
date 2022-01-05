using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorPlacement : MonoBehaviour
{
    public GameObject[] Decor = new GameObject[7];

    private GameObject currentObject;

    private List<GameObject> allObjects; // All decor objects for this tile.

    [SerializeField] private float offsetY;

    private Vector3 playerPosition;

    private int previousXflora;
    private int previousXshell;
    private int previousZflora;
    private int previousZshell;

    private int refreshDistance;

    void Awake() {
        allObjects = new List<GameObject>();
        previousXflora = 0;
        previousZflora = 0;
        refreshDistance = 50;
    }
    public void PlaceDecor(int x, float y, int z, Vector3 worldPos) {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        //Floral Height condition to spawn
        if((y > -0.5f && y < 0.1f) || (y > 0.15 && y < 0.18) || (y > 0.20 && y < 0.22) || (y > 0.25 && y < 0.30)  || (y > 0.45 && y < 0.55)) {
                //Only generate Decor outside the radius
                if(Mathf.Sqrt(Mathf.Pow(worldPos.x + x - playerPosition.x, 2) + Mathf.Pow(worldPos.z + z - playerPosition.z, 2)) > refreshDistance) {
                    currentObject = GameObject.Instantiate(Decor[0]);
                    currentObject.transform.position = new Vector3(worldPos.x + x, y + offsetY, worldPos.z + z);
                    allObjects.Add(currentObject);
                }
        }


        //Seashells
        if(y > 0.79 && y < 0.8) {
            if(Mathf.Sqrt(Mathf.Pow(worldPos.x + x - playerPosition.x, 2) + Mathf.Pow(worldPos.z + z - playerPosition.z, 2)) > refreshDistance) {
                int index = Mathf.RoundToInt(Mathf.Abs(worldPos.x + x) + Mathf.Abs(worldPos.z + z));
                index = index%2 + 5;
                currentObject = GameObject.Instantiate(Decor[index]);
                currentObject.transform.position = new Vector3(worldPos.x + x, y, worldPos.z + z);
                allObjects.Add(currentObject);
            }
        }

        
    }

    public void DestroyDecor() {
        // Decor is only destroyed if it's a certain distance away from the player. Respawning of objects in close proximity looks bad.
        // Decor can also be destroyed if the ground is not detected above or below the object root. (To collect garbage that may not have been despawned)
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        foreach(GameObject element in allObjects) {
            if(Mathf.Sqrt(Mathf.Pow(element.transform.position.x - playerPosition.x, 2) + Mathf.Pow(element.transform.position.z - playerPosition.z, 2)) > refreshDistance) {
                Destroy(element);
            }
        }
        allObjects.Clear();
    }
}
