using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerEnemyBase : SpawnManager/*MonoBehaviour*/
{
    private bool _isInverseBase;

    private void Awake()
    {
        EnemyPrefab = Resources.Load("Prefabs/WhiteSpaceship") as GameObject;
    }

    private void Start()
    {
        EnemyWave = 6;
        EnemyCounter = 0;
        RepeatWaveTime = 0.4f;
        _isInverseBase = int.Parse(gameObject.name.Split('_')[1]) % 2 == 0;
        
        InvokeRepeating("LaunchEnemies",0f, RepeatWaveTime);
    }

    /*[SerializeField] private GameObject whiteShipPrefab;
    [SerializeField] private int enemieWave;

    private bool _isInverseBase;
    private int _enemieCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemieCounter = 0;
        _isInverseBase = int.Parse(gameObject.name.Split('_')[1]) % 2 == 0;
        
        InvokeRepeating("LaunchEnemy",0f,0.4f);
    }
    

    private void LaunchEnemy()
    {
        var shipPrefab =Instantiate(whiteShipPrefab, 
            new Vector3(transform.position.x, transform.position.y,1), Quaternion.identity);
        
        shipPrefab.GetComponent<EnemyWhiteShipController>().SetInverseMove(_isInverseBase);

        _enemieCounter++;
        
        if(_enemieCounter == enemieWave) CancelInvoke("LaunchEnemy");
    }*/
    protected override void LaunchEnemies()
    {
        var shipPrefab =Instantiate(EnemyPrefab, 
            new Vector3(transform.position.x, transform.position.y,1), Quaternion.identity);
        
        shipPrefab.GetComponent<EnemyWhiteShipController>().SetInverseMove(_isInverseBase);

        EnemyCounter++;

        if (EnemyCounter == EnemyWave) CancelInvoke("LaunchEnemies");
        
    }
}
