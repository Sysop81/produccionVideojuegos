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
        _explosion = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Explosion");
    }
    
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DeathZone")) Destroy(gameObject);
        
        if (other.CompareTag("Shoot"))
        {
            for (int i = 0; i < 4; i++)
            {
                Instantiate(_explosion, Tools.GetAleatoryTranformPosition(transform.position,Random.Range(0.5f,1f)), Quaternion.identity);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
