using Assets.Scripts.Classes;
using Assets.Scripts.Services.DataStorageService;
using Assets.Scripts.Services.RandomGeneratorService;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }
    public IDataStorage _dataStorage;

    private IRandomGenerator _randomGenerator;
    private int obstacleDelayTime = 1;
    private int pickupDelayTime = 1;
    private int numberOfObstacles = 1;
    private int numberOfPickups = 1;
    private DateTime lastObstacleTime;
    private DateTime lastPickupTime;
    private List<ObjectToSpawn> obstaclesToSpawn;
    private List<ObjectToSpawn> pickupsToSpawn;

    public List<GameObject> obstacles;
    public List<GameObject> pickups;
    public Camera sceneCamera;
    public int minDelayBetweenSpawningObstacles = 1;
    public int maxDelayBetweenSpawningObstacles = 5;
    public int minDelayBetweenSpawningPickups = 5;
    public int maxDelayBetweenSpawningPickups = 20;
    public int minNumberOfObstacles = 1;
    public int maxNumberOfObstacles = 3;
    public List<float> pickupWeights;

    // Start is called before the first frame update
    void Start()
    {
        _randomGenerator = new RandomGenerator();
        obstaclesToSpawn = new List<ObjectToSpawn>();
        pickupsToSpawn = new List<ObjectToSpawn>();
        lastObstacleTime = DateTime.Now;
        lastPickupTime = DateTime.Now;

        if (pickupWeights == null)
        {
            pickupWeights = new List<float>();
        }

        if (!obstacles.Any())
        {
            // Retrieve obstacles
        }

        if (sceneCamera == null)
        {
            sceneCamera = FindObjectOfType<Camera>();

            if (sceneCamera == null)
            {
                throw new NullReferenceException("Unable to find the camera for the game.");
            }
        }

        _dataStorage = new DataStorage(sceneCamera.orthographicSize);

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateObstacles();
        GeneratePickups();
    }

    private void GenerateObstacles()
    {
        if (DateTime.Now >= lastObstacleTime.AddSeconds(obstacleDelayTime))
        {
            obstaclesToSpawn.Clear();

            numberOfObstacles = _randomGenerator.GetRangedRandomInt(minNumberOfObstacles, maxNumberOfObstacles);

            for (int i = 0; i < numberOfObstacles; ++i)
            {
                var obstacleObjectIndex = _randomGenerator.GetRangedRandomInt(0, obstacles.Count());
                var obstacleObject = obstacles.ElementAt(obstacleObjectIndex);
                var position = new Vector3(_randomGenerator.GetRangedRandomFloat(0, sceneCamera.orthographicSize * 2) - sceneCamera.orthographicSize,
                    (sceneCamera.orthographicSize * 2) + _randomGenerator.GetRangedRandomFloat(0, sceneCamera.orthographicSize * 2), obstacleObject.transform.position.z);
                //var rotation = new Quaternion(_randomGenerator.GetRandomInt(), _randomGenerator.GetRandomInt(), _randomGenerator.GetRandomInt(), _randomGenerator.GetRandomInt());

                obstaclesToSpawn.Add(new ObjectToSpawn(obstacleObject, position));
            }

            // Spawn obstacles
            foreach (var obstacle in obstaclesToSpawn)
            {
                Instantiate(obstacle.Object, obstacle.SpawnPosition, obstacle.SpawnRotation);
            }

            obstacleDelayTime = _randomGenerator.GetRangedRandomInt(minDelayBetweenSpawningObstacles, maxDelayBetweenSpawningObstacles);
            lastObstacleTime = DateTime.Now;
        }
    }

    private void GeneratePickups()
    {
        if (DateTime.Now >= lastPickupTime.AddSeconds(pickupDelayTime))
        {
            pickupsToSpawn.Clear();

            numberOfPickups = _randomGenerator.GetWeightedRandomInt(pickupWeights);

            for (int i = 0; i < numberOfPickups; ++i)
            {
                var pickupObjectIndex = _randomGenerator.GetRangedRandomInt(0, pickups.Count());
                var pickupObject = pickups.ElementAt(pickupObjectIndex);
                var position = new Vector3(_randomGenerator.GetRangedRandomFloat(0, sceneCamera.orthographicSize * 2) - sceneCamera.orthographicSize,
                    (sceneCamera.orthographicSize * 2) + _randomGenerator.GetRangedRandomFloat(0, sceneCamera.orthographicSize * 2), pickupObject.transform.position.z);
                //var rotation = new Quaternion(_randomGenerator.GetRandomInt(), _randomGenerator.GetRandomInt(), _randomGenerator.GetRandomInt(), _randomGenerator.GetRandomInt());

                pickupsToSpawn.Add(new ObjectToSpawn(pickupObject, position));
            }

            // Spawn obstacles
            foreach (var pickup in pickupsToSpawn)
            {
                Instantiate(pickup.Object, pickup.SpawnPosition, pickup.SpawnRotation);
            }

            pickupDelayTime = _randomGenerator.GetRangedRandomInt(minDelayBetweenSpawningPickups, maxDelayBetweenSpawningPickups);
            lastPickupTime = DateTime.Now;
        }
    }
}
