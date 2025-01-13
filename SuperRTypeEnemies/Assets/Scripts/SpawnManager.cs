using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int enemieWave;
    [SerializeField] private int numOfWaves;

    private Vector3[] _waveGroupsMoves = {Vector3.down, Vector3.up,Vector3.up,Vector3.down,Vector3.down, Vector3.up, Vector3.down,Vector3.down};

    private bool _isLowerShip;
    private float _iSpawnXPos;
    private readonly float _enmiesXOffset = 3f;
    private int _launchedWaves;

    private GameObject _enemyPrefab;
    private bool _isAlienPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemyPrefab = enemies[0];
        _launchedWaves = 0;
        _iSpawnXPos = transform.position.x;
        InvokeRepeating("LaunchEnemyWave", 0f, 2f);
    }
    

    private void LaunchEnemyWave()
    {
        for (int i = 0; i < enemieWave; i++)
        {
            var ePrefab = Instantiate(_enemyPrefab, GetSpawnPosition(transform.position, i != 0 && i != 5), Quaternion.identity);
            
            if(ePrefab.gameObject.name.Contains("ship"))
                ePrefab.gameObject.GetComponent<EnemyController>().SetVerticalMove(_waveGroupsMoves[i]);
        }

        _launchedWaves++;
        if (_launchedWaves == numOfWaves)
        {
            CancelInvoke("LaunchEnemyWave");
            _launchedWaves = 0;
            enemieWave = 5;
            numOfWaves = 2;
            _isAlienPrefab = !_isAlienPrefab;
        }
    }
    

    private Vector3 GetSpawnPosition(Vector3 position, bool isParalel = false)
    {
        var yPos = position.y;
        
        if (isParalel)
        { 
            yPos = _isLowerShip ? position.y + 1 : position.y - 1;
            _isLowerShip = !_isLowerShip;
        }

        if (_isLowerShip || !isParalel)
        {
            _iSpawnXPos += _enmiesXOffset;
        }

        if (_isAlienPrefab)
        {
            yPos = Random.Range(position.y - 4, position.y + 4);
        }

        return new Vector3(_iSpawnXPos,yPos,position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.name.Contains("WP")) return;

        var repeatRate = 3f;
        
        if(other.gameObject.name.Equals("WPAlienSpawn"))
        {
            _enemyPrefab = enemies[1];
        }

        if (other.gameObject.name.Equals("WPLastShipSpawn"))
        {
            _enemyPrefab = enemies[0];
            numOfWaves = 2;
            repeatRate = 1f;
        }
        
        _iSpawnXPos = other.transform.position.x;
        InvokeRepeating("LaunchEnemyWave", 0f, repeatRate);
    }
}
