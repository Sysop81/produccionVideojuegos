using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaibowController : MonoBehaviour
{

    private float _time;
    private const float MAX_TIME = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        _time = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= MAX_TIME)
        {
            Destroy(gameObject);
        }
    }
}
