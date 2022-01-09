using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{   
    Vector3 direction;
    Vector3 position;

    Quaternion rotation;

    float rotateFreq;

    float currTime;
    float timer;

    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        currTime = 0;
        rotateFreq = 1f;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        timer += currTime%2;
        
        //  Wave rotation
        //transform.Rotate(Vector3.up, Mathf.Sin(currTime * rotateFreq));

        //  Random Rotation
        if(timer > 2) {
            rotation = Quaternion.Euler(new Vector3(Random.Range(-80.0f, 80.0f),Random.Range(-180.0f, 180.0f), 0.0f)); 
            timer = 0;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime*rotateFreq/4f);

        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
    }

    void OnTriggerEnter(Collider other) {

        if(other.tag == "Ground") {
            timer = 0;
            transform.Rotate(new Vector3(-35f,0.0f, 0.0f), Space.Self);
            //rotation = Quaternion.Euler(new Vector3(-35f,0.0f, 0.0f)); 
            //transform.rotation = rotation;
        }
    }
}
