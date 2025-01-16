using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    
    private GameObject _explosion;
    protected float ForwardSpeed { get; set; }
    protected Vector3 Direction { get; set; }
    protected SpriteRenderer SpriteRenderer { get; set; }
    
    
    protected void Intialize()
    {
        _explosion = Resources.Load("Prefabs/Explosion") as GameObject;
    }

    protected void LaunchExplosion(int numOfExplosions = 4)
    {
        //Tools.DrawExplosion(_explosion,transform.position,numOfExplosions);
        ExplosionController.DrawExplosion(_explosion,transform.position,numOfExplosions);
    }


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
