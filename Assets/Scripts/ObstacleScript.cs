using Assets.Scripts.Classes;
using Assets.Scripts.Services.DataStorageService;
using System;
using System.Diagnostics;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    const int MaxLifetime = 60000;
    public float movementSpeed = 5f;
    private long lastTimeCheckedInMs = 0;
    private long totalLifetime = 0;
    private Stopwatch stopwatch;
    private IDataStorage _dataStorage;
    private float screenSize;

    // Start is called before the first frame update
    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();

        _dataStorage = GameManagerScript.Instance._dataStorage;

        if (_dataStorage == null)
        {
            throw new NullReferenceException("Unable to retrieve the data storage service from the game controller");
        }

        screenSize = _dataStorage.GetScreenSize();
    }

    // Update is called once per frame
    void Update()
    {
        totalLifetime = stopwatch.ElapsedMilliseconds;
        MoveObstacle();
    }

    private void MoveObstacle()
    {
        if (totalLifetime > lastTimeCheckedInMs + 1000)
        {
            if (transform.position.y < -(screenSize * 2))
            {
                Destroy(gameObject);
            }

            lastTimeCheckedInMs = totalLifetime;
        }
        
        if (totalLifetime > MaxLifetime)
        {
            Destroy(gameObject);
        }

        transform.Translate(0, -1 * movementSpeed * Time.deltaTime, 0);
    }
}
