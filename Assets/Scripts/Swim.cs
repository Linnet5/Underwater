using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : MonoBehaviour
{
    Rigidbody rb;
    Transform cameraLook;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraLook = GameObject.FindGameObjectWithTag("MainCamera").transform;
        speed = 6;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W)) {
            rb.position += Vector3.forward * Time.fixedDeltaTime * speed;
        }
        else if(Input.GetKey(KeyCode.S)) {
            rb.position -= Vector3.forward * Time.fixedDeltaTime * speed;
        }
        else if(Input.GetKey(KeyCode.A)) {
            rb.position -= Vector3.forward * Time.fixedDeltaTime * speed;
        }
        else if(Input.GetKey(KeyCode.D)) {
            rb.position += Vector3.forward * Time.fixedDeltaTime * speed;
        }
    }
}
