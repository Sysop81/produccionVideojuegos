using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject whiteShipPrefab;
    [SerializeField] private int enemieWave;

    private bool _isInverseBase;
    private int _enemieCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemieCounter = 0;
        _isInverseBase = int.Parse(gameObject.name.Split('_')[1]) % 2 == 0;
        
        InvokeRepeating("LaunchEnemy",0f,0.3f);
    }
    

    private void LaunchEnemy()
    {
        var shipPrefab =Instantiate(whiteShipPrefab, 
            new Vector3(transform.position.x, transform.position.y,1), Quaternion.identity);
        
        shipPrefab.GetComponent<EnemyWhiteShipController>().SetInverseMove(_isInverseBase);

        _enemieCounter++;
        
        if(_enemieCounter == enemieWave) CancelInvoke("LaunchEnemy");
    }
}
