using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject camera;
    public Transform cam;


    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camera.transform; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
