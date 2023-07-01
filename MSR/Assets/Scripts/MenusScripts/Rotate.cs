using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    float rotation;

    // Start is called before the first frame update
    void Start()
    {
        rotation = gameObject.transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1f;
    }
}
