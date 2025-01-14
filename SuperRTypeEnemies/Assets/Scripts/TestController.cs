using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentRotation = transform.rotation.eulerAngles;

        currentRotation.z += 50 * Time.fixedDeltaTime;
        
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
