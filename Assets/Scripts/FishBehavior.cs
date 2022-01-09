using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{   
    Vector3 direction;
    Vector3 position;

    GameObject player;

    Quaternion rotation;

    float rotateFreq;

    float currTime;
    float timer;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currTime = 0;
        rotateFreq = 1f;
        speed = 3f;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        timer += currTime%2;
        
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);

        //  Random Rotation
        if((transform.position - player.transform.position).magnitude < 15.0f) {
            rotation = Quaternion.LookRotation(transform.position - player.transform.position);
            speed = 10.0f;
            rotateFreq = 2f;
            StartCoroutine("ResetSpeed");
        }
        else if(timer > 2) {
            rotation = Quaternion.Euler(new Vector3(Random.Range(-80.0f, 80.0f),Random.Range(-180.0f, 180.0f), 0.0f)); 
            timer = 0;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime*rotateFreq/4f);
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Ground") {
            timer = 0;
            transform.Rotate(new Vector3(-35f,0.0f, 0.0f), Space.Self);
        }
    }

    IEnumerator ResetSpeed() {
        yield return new WaitForSeconds(2.0f);
        speed = 3.0f;
        rotateFreq = 1.0f;
    }
}
