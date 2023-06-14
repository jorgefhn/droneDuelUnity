using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            Vector3 targetPosition = target.position;

            transform.position = new Vector3(target.position.x,target.position.y,target.position.z);
        }
        
    }
}
