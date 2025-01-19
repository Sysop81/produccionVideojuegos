using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManagerCamera : SpawnManager
{
    
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] verticalSpawnPoints;
    
    private readonly float _enmiesXOffset = 3f;
    private bool _isLowerShip;
    private float _iSpawnXPos;
    private bool _isAlienPrefab;
    private bool _isBombSpawn;
    
    /// <summary>
    /// Method Start [Life cycles]
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Loading initial parent properties and local
        EnemyPrefab = enemies[0];
        EnemyWave = 8;
        NumOfWaves = 3;
        WaveCounter = 0;
        RepeatWaveTime = 2f;
        _iSpawnXPos = transform.position.x;
        
        // Launch repeating loop 
        InvokeRepeating(nameof(LaunchEnemies), 0f, RepeatWaveTime);
    }
    
    /// <summary>
    /// Method LaunchEnemies
    /// This legacy method launches waves of enemies
    /// </summary>
    protected override void LaunchEnemies()
    {
        // Launch an enemy wave
        for (int i = 0; i < EnemyWave; i++)
        {
            var ePrefab = Instantiate(EnemyPrefab, GetSpawnPosition(transform.position, i != 0 && i != 5), Quaternion.identity);
            
            // If enemy type is a ship. Set the initial Vector3 movement in the enemy controller
            if(ePrefab.gameObject.name.Contains("ship"))
                ePrefab.gameObject.GetComponent<EnemyRedShipController>().SetVerticalMove(WaveGroupsMoves[i]);
        }
        // Update and manage the counter to cancel invoke and reset the spawner properties
        WaveCounter++;
        if (WaveCounter == NumOfWaves)
        {
            CancelInvoke(nameof(LaunchEnemies));
            WaveCounter = 0;
            EnemyWave = 5;
            NumOfWaves = 2;
            
            // Update flag to launch an Alien enemy type 
            _isAlienPrefab = !_isAlienPrefab;
        }
    }
    
    /// <summary>
    /// Method GetSpawnPosition
    /// This method manages Y axis position of any enemy to spawn
    /// </summary>
    /// <param name="position"></param>
    /// <param name="isParalel"></param>
    /// <returns>Vector3 with the spawn position</returns>
    private Vector3 GetSpawnPosition(Vector3 position, bool isParalel = false)
    {
        // Instatiate a local variable to set the initial Y value
        var yPos = position.y;
        
        // If the movement it is a parallel with another enemy. increase or decrease Y axis based on the boolean (_isLowerShip) value
        if (isParalel)
        { 
            yPos = _isLowerShip ? position.y + 1 : position.y - 1;
            _isLowerShip = !_isLowerShip;
        }
        // If it is a upper ship or is not a parallel movement. Updates the initial spawn position adding the offset to new row of enemies
        if (_isLowerShip || !isParalel) _iSpawnXPos += _enmiesXOffset;
        
        // If it is an Alien type set a random position
        if (_isAlienPrefab) yPos = Random.Range(position.y - 4, position.y + 4);
        
        // Finally retrurn the Vector3 with new position
        return new Vector3(_iSpawnXPos,yPos,position.z);
    }
    
    /// <summary>
    /// Method ManageVerticalSpawners
    /// This menthod Active or not the Vertical Bomber spawners
    /// </summary>
    private void ManageVerticalSpawners()
    {
        foreach (var sp in verticalSpawnPoints) sp.SetActive(_isBombSpawn);
    }
    
    /// <summary>
    /// Trigger OnTriggerEnter2D
    /// </summary>
    /// <param name="other">Collision object </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the collision object isn't a way point set an early return
        if (!other.name.Contains("WP")) return;
        
        // Activate a vertical Bombers spawners
        if (other.gameObject.name.Contains("WPBombSpawn"))
        {
            _isBombSpawn = !_isBombSpawn;
            ManageVerticalSpawners();
            return;
        }
        
        // Update a wave time property with the default value
        RepeatWaveTime = 6f;
        
        // Change to Alien prefab
        if(other.gameObject.name.Equals("WPAlienSpawn")) EnemyPrefab = enemies[1];
        
        // Updating values to launch the last red ships wave
        if (other.gameObject.name.Equals("WPLastShipSpawn"))
        {
            EnemyPrefab = enemies[0];
            NumOfWaves = 2;
            RepeatWaveTime = 1f;
        }
        // Update the initial spawn position with the object collision position and launch the invoke
        _iSpawnXPos = other.transform.position.x;
        InvokeRepeating(nameof(LaunchEnemies), 0f, RepeatWaveTime);
    }
}
