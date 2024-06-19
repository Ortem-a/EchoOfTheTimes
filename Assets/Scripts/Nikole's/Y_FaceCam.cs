using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_FaceCam : MonoBehaviour
{
    Vector3 cameraDir;

    void Update()
    {
        cameraDir = Camera.main.transform.forward;
        cameraDir.x = 0;
        cameraDir.z = 0;

        transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
