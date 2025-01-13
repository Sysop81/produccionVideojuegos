using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnManager : MonoBehaviour
{
    
    [SerializeField] private GameObject whiteShipPrefab;
    [SerializeField] private int enemieWave;
    
    private Vector3[] _waveMoves = {Vector3.down, Vector3.up,Vector3.down,Vector3.up,Vector3.down, Vector3.up};
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LaunchEnemyWave", 0f);
    }

    private void LaunchEnemyWave()
    {
        bool isDown = int.Parse(gameObject.name.Split('_')[1]) % 2 == 0;
        var offSet = 2f;
        var yPos = transform.position.y - offSet;
        for (int i = 0; i < enemieWave; i++)
        {
            
            var shipPrefab =Instantiate(whiteShipPrefab, 
                    new Vector3(transform.position.x, yPos,1), Quaternion.identity);
            shipPrefab.GetComponent<EnemyController3>().SetVerticalMove(_waveMoves[i], isDown);
            //yPos -= offSet;
            if (isDown) yPos += offSet;
            else yPos -= offSet;
        }
    }
}
