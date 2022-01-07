using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    GameObject[] ground;

    //Script checks deletion condition every 5 seconds.
    void Start() {
        InvokeRepeating("Delete", 5.0f, 5.0f);
    }
    void Delete() {
        ground = GameObject.FindGameObjectsWithTag("Ground");
        float shortestDistance = 9999.0f;
        foreach(GameObject tile in ground) {
            float distance = (tile.transform.position - gameObject.transform.position).magnitude;
            if(distance < shortestDistance)
                shortestDistance = distance;
        }

        if(shortestDistance > 50) {
            Destroy(gameObject);
        }
    }
}
