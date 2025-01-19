using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// Explosion prefab
    /// </summary>
    private GameObject _explosion;
    /// <summary>
    /// ForwardSpeed. Enemy forward speed
    /// </summary>
    protected float ForwardSpeed { get; set; }
    /// <summary>
    /// Direction. Enemy direction to apply the forward speed
    /// </summary>
    public Vector3 Direction { get; set; }
    
    /// <summary>
    /// SpriteRender. Componet to manage the sprite
    /// </summary>
    protected SpriteRenderer SpriteRenderer { get; set; }
    
    /// <summary>
    /// Method Intialize
    /// This method instantiate an explosion prefab
    /// </summary>
    protected void Intialize()
    {
        _explosion = Resources.Load("Prefabs/Explosion") as GameObject;
    }
    
    /// <summary>
    /// Method LaunchExplosion
    /// This method calling the explosion static class method to launch an enemy explosion
    /// </summary>
    /// <param name="numOfExplosions"></param>
    protected void LaunchExplosion(int numOfExplosions = 4)
    {
        ExplosionController.DrawExplosion(_explosion,transform.position,numOfExplosions);
    }

    /// <summary>
    /// Trigger OnTriggerEnter2D
    /// </summary>
    /// <param name="other"></param>    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DeathZone")) Destroy(gameObject);
        
        if (other.CompareTag("Shoot"))
        {
            LaunchExplosion();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
