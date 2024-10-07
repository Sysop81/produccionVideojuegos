using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    private float _limit = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get X axis player position
        float x = player.transform.position.x;
        
        // Check if X player position if out of limit. 
        if (x > _limit)
        {
            x = _limit;
        }else if (x < - _limit)
        {
            x = - _limit;
        }
        
        // Set a position camera with player positions
        gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
