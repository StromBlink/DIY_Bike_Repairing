using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creative_Controller : MonoBehaviour
{
    public Transform platform;
    // Update is called once per frame
    void Update()
    {
        platform.Rotate(new Vector3(0,Time.deltaTime*10,0));
    }
}
