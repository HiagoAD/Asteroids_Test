using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportableObject : MonoBehaviour
{
    void LateUpdate()
    {
        if(transform.position.x > 5) transform.position -= new Vector3(10, 0, 0);
        else if(transform.position.x < -5) transform.position += new Vector3(10, 0, 0);

        if(transform.position.y > 5) transform.position -= new Vector3(0, 10, 0);
        else if(transform.position.y < -5) transform.position += new Vector3(0, 10, 0);
    }
}
