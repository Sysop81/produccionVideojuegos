using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnManager : MonoBehaviour
{
    protected static readonly Vector3[] WaveGroupsMoves = {Vector3.down, Vector3.up,Vector3.up,Vector3.down,Vector3.down, Vector3.up, Vector3.down,Vector3.down};
    protected int EnemyWave { get; set; }
    protected int NumOfWaves { get; set; }
    protected GameObject EnemyPrefab { get; set; }
    protected int EnemyCounter { get; set; }
    protected int WaveCounter { get; set; }
    
    protected float RepeatWaveTime { get; set; }

    protected abstract void LaunchEnemies();
}
