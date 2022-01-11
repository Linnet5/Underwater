using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private GameObject player;
    GameObject[] ground;

    float refreshSize;

    void Start() {
        refreshSize = 125.0f;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void FixedUpdate() {
        if(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) > refreshSize || Mathf.Abs(player.transform.position.z - gameObject.transform.position.z) > refreshSize) {
            Destroy(gameObject);
        }
    } 
}
