using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float time;
    private float _timer;
    
    /// <summary>
    /// Method Start
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        _timer = Time.deltaTime;
    }

    /// <summary>
    /// Method Update
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (_timer < time)
        {
            _timer += Time.deltaTime;
            return;
        }
        
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Method DrawExplosion
    /// This static method draw a random explosion
    /// </summary>
    /// <param name="explosion"></param>
    /// <param name="position"></param>
    /// <param name="numOfExplosions"></param>
    public static void DrawExplosion(GameObject explosion,Vector3 position, int numOfExplosions = 4)
    {
        for (int i = 0; i < numOfExplosions; i++)
            Instantiate(explosion, Tools.GetAleatoryTranformPosition(position,Random.Range(0.5f,1f)), Quaternion.identity);
    }
}
