using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            if (target.transform.position.y > 0)
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 4, transform.position.z);
        }
    }
}
