using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
