using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMoveLeft : MonoBehaviour
{

    [SerializeField] private float xSpeed;
    
    /// <summary>
    /// Method Update [Life cycle]
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.Translate(-xSpeed * Time.deltaTime, 0, 0);
    }
}
