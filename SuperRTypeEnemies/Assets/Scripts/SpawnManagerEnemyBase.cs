using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerEnemyBase : SpawnManager
{
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private int enemiesByWave;
    [SerializeField] private float repeatTime;
    private bool _isInverse;
    
    /// <summary>
    /// Method Awake [Life cycle]
    /// </summary>
    private void Awake()
    {
        // Load a parent properties
        EnemyPrefab = prefabEnemy; 
    }
    
    /// <summary>
    /// Method Start [Life cycle]
    /// </summary>
    private void Start()
    {
        // Load parent and local properties
        EnemyWave = 6;
        EnemyCounter = 0;
        RepeatWaveTime = repeatTime; 
        _isInverse = int.Parse(gameObject.name.Split('_')[1]) % 2 == 0; // Determinies the upper or down spawn position
        
        // Launch the repeating method invouke
        InvokeRepeating(nameof(LaunchEnemies),0f, RepeatWaveTime);
    }
    
    /// <summary>
    /// Method LaunchEnemies
    /// This method manages the spawn behavior
    /// </summary>
    protected override void LaunchEnemies()
    {
        // Instantia the prefab
        var prefab =Instantiate(EnemyPrefab, 
            new Vector3(transform.position.x, transform.position.y,1), Quaternion.identity);
        // Set value into inherited property            
        prefab.GetComponent<EnemyController>().Direction = !_isInverse ? Vector3.down : Vector3.up;
        
        // Manages the white ship enemy behavior
        try
        {
            prefab.GetComponent<EnemyWhiteShipController>().SetInverseMove(_isInverse);
        }catch { /* Isn't a White Ship prefab */ }
        
        // Increment the enemy counter
        EnemyCounter++;
        // Manage the cancel invoke when counter reach the limit of enemy wave   
        if (EnemyCounter == EnemyWave) CancelInvoke(nameof(LaunchEnemies));
    }
}
