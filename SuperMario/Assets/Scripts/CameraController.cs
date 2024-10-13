using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private GameObject _player;
    [SerializeField] private float offsetX = 2f; //  Horizontal offset 
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Only if game is stopped
        if (Time.timeScale == 0) return;
        
        // Get actual player position
        Vector3 playerPosition = _player.transform.position;

        // Only update a camera position if player is ahead 
        if (playerPosition.x > transform.position.x - offsetX)
        {
            transform.position = new Vector3(playerPosition.x + offsetX, transform.position.y, transform.position.z);
        }
    }
}
