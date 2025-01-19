using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// WaveGroupsMoves
    /// Group the Vector3 movements to Red Ship wave
    /// </summary>
    protected static readonly Vector3[] WaveGroupsMoves = {Vector3.down, Vector3.up,Vector3.up,Vector3.down,Vector3.down, Vector3.up, Vector3.down,Vector3.down};
    /// <summary>
    /// EnemyWave
    /// Determines the number of enemies in each wave
    /// </summary>
    protected int EnemyWave { get; set; }
    /// <summary>
    /// NumOfWaves
    /// Determines the number of waves
    /// </summary>
    protected int NumOfWaves { get; set; }
    /// <summary>
    /// EnemyPrefab
    /// Principal enemy prefab
    /// </summary>
    protected GameObject EnemyPrefab { get; set; }
    /// <summary>
    /// EnemyCounter
    /// Counter to manage the enemies of wave
    /// </summary>
    protected int EnemyCounter { get; set; }
    /// <summary>
    /// WaveCounter
    /// Number of waves
    /// </summary>
    protected int WaveCounter { get; set; }
    /// <summary>
    /// RepeatWaveTime
    /// Time to repeat the enemy wave
    /// </summary>
    protected float RepeatWaveTime { get; set; }
    /// <summary>
    /// LaunchEnemies
    /// Method to set the launch spawn behavior 
    /// </summary>
    protected abstract void LaunchEnemies();
}
