using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    /// <summary>
    /// Method GetAleatoryTranformPosition
    /// This method generates a random Vector3 position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="offset"></param>
    /// <returns>Vector3 with random position</returns>
    public static Vector3 GetAleatoryTranformPosition(Vector3 position, float offset = 0.5f)
    {
        var x = Random.Range(position.x -offset, position.x + offset);
        var y = Random.Range(position.y -offset, position.y + offset);
        
        return new Vector3(x,y,position.z);
    }
}
