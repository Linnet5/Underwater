using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorPlacement : MonoBehaviour
{
    public GameObject[] Decor = new GameObject[5];
    private GameObject currentObject;
    private List<GameObject> allObjects; // All decor objects for this tile.
    [SerializeField] private float offsetY;
    private Vector3 playerPosition;
    private int refreshDistance;

    const int MAXREFRESHDISTANCE = 40;

    void Awake() {
        allObjects = new List<GameObject>();
    }
    public void PlaceDecor(int x, float y, int z, Vector3 worldPos, bool firstTime) {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        if(firstTime)
            refreshDistance = 0;
        else
            refreshDistance = MAXREFRESHDISTANCE;

        if((y > -0.5f && y < 0.1f) || (y > 0.15 && y < 0.18) || (y > 0.20 && y < 0.22) || (y > 0.25 && y < 0.30)  || (y > 0.45 && y < 0.55)) {
            //Only generate Decor outside the radius
            if(Mathf.Sqrt(Mathf.Pow(worldPos.x + x - playerPosition.x, 2) + Mathf.Pow(worldPos.z + z - playerPosition.z, 2)) > refreshDistance) {
                currentObject = GameObject.Instantiate(Decor[0]); //Grass
                currentObject.transform.position = new Vector3(worldPos.x + x, y + offsetY, worldPos.z + z);
                allObjects.Add(currentObject);
            }
        }

        if((y > 0.5 && y < 0.75) || (y > 0.77)) {
            if(Mathf.Sqrt(Mathf.Pow(worldPos.x + x - playerPosition.x, 2) + Mathf.Pow(worldPos.z + z - playerPosition.z, 2)) > refreshDistance) {
                //On certain heights, there is a 0.1% chance that a rock will spawn
                if (Random.Range(0.0f, 1.0f) > 0.999f) {
                    int index = Mathf.RoundToInt(Mathf.RoundToInt(Random.Range(0.0f, 1.0f)));
                    index = index%2 + 1;
                    currentObject = GameObject.Instantiate(Decor[index]); //Rock
                    currentObject.transform.position = new Vector3(worldPos.x + x, y - offsetY, worldPos.z + z);
                    allObjects.Add(currentObject);
                }
            }
        }

        //Seashells
        if(y > 0.79 && y < 0.8) {
            if(Mathf.Sqrt(Mathf.Pow(worldPos.x + x - playerPosition.x, 2) + Mathf.Pow(worldPos.z + z - playerPosition.z, 2)) > refreshDistance) {
                //On certain heights, there is a 5% chance that a seashell will spawn
                if (Random.Range(0.0f, 1.0f) > 0.95f) {
                    int index = Mathf.RoundToInt(Mathf.Abs(worldPos.x + x) + Mathf.Abs(worldPos.z + z));
                    index = index%2 + 3;
                    currentObject = GameObject.Instantiate(Decor[index]);
                    currentObject.transform.position = new Vector3(worldPos.x + x, y, worldPos.z + z);
                    allObjects.Add(currentObject);
                }
            }
        }
    }

    public void DestroyDecor(Vector3 playerPosition) {
        // Decor is only destroyed if it's a certain distance away from the player. Respawning of objects in close proximity looks bad.
        // Decor can also be destroyed if the ground is not detected above or below the object root. (To collect garbage that may not have been despawned)
        refreshDistance = MAXREFRESHDISTANCE;
        foreach(GameObject element in allObjects) {
            if(Mathf.Sqrt(Mathf.Pow(playerPosition.x - element.transform.position.x, 2) + Mathf.Pow(playerPosition.z - element.transform.position.z, 2)) > refreshDistance) {
                Destroy(element);
            }
        }
        allObjects.RemoveAll(element => element == null); 
    }
}
