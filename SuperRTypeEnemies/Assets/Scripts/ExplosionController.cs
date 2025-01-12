using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float time;
    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer < time)
        {
            _timer += Time.deltaTime;
            return;
        }
        
        Destroy(gameObject);
    }
}
