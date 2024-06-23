using Assets.Scripts.Classes;
using Assets.Scripts.Services.DataStorageService;
using Assets.Scripts.Services.RandomGeneratorService;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The script for managing different events throughout the course of a round of gameplay that aren't tied to any specific object within the game.
/// </summary>
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
    private int startDelay = 3;
    private bool delayingStart = true;
    private bool spawningObjects = true;
    private DateTime startTime;

    public List<GameObject> obstacles;
    public List<GameObject> pickups;
    public Camera sceneCamera;
    public int minDelayBetweenSpawningObstacles = 1;
    public int maxDelayBetweenSpawningObstacles = 5;
    public int minDelayBetweenSpawningPickups = 5;
    public int maxDelayBetweenSpawningPickups = 20;
    public int minNumberOfObstacles = 1;
    public int maxNumberOfObstacles = 3;
    public int playerMaxLength = 100;
    public List<float> pickupWeights;

    // Start is called before the first frame update
    private void Start()
    {
        _randomGenerator = new RandomGenerator();
        obstaclesToSpawn = new List<ObjectToSpawn>();
        pickupsToSpawn = new List<ObjectToSpawn>();
        lastObstacleTime = DateTime.Now;
        lastPickupTime = DateTime.Now;
        startTime = DateTime.Now;

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

        _dataStorage = new DataStorage(sceneCamera.orthographicSize, sceneCamera.rect.width, playerMaxLength);

        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!delayingStart && spawningObjects)
        {
            GenerateObstacles();
            GeneratePickups();
        }
        else if (DateTime.Now >= startTime.AddSeconds(startDelay))
        {
            delayingStart = false;
        }
    }

    /// <summary>
    /// Generates obstacles within the game at random positions from a pre-assigned list of obstacle game objects with random delays between each batch of obstacles.
    /// </summary>
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

    /// <summary>
    /// Generates pickups within the game at random positions from a pre-assigned list of pickup game objects with random delays between each batch of pickups.
    /// </summary>
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


    /// <summary>
    /// Saves the player's score from the current round of play to the leaderboard.
    /// </summary>
    /// <param name="score">The player's score to save.</param>
    /// <param name="timePlayed">The amount of time the player played for.</param>
    /// <returns>A bool representing whether the score was saved successfully.</returns>
    private bool SaveScoreToLeaderboard(int score, int timePlayed)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a page of leaderboard scores.
    /// </summary>
    /// <param name="page">The page of the leaderboard that should be retrieved.</param>
    private IEnumerable<LeaderboardScore> GetLeaderboardPage(int? page = 1)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Restarts the current scene for play.
    /// </summary>
    public void RestartGame()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the player to the main menu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Starts a game over event which will freeze the active gameplay and bring up a menu containing the player's score from the current round of play and a leaderboard alongside menu controls.
    /// </summary>
    public void GameOver(EndingTypeEnum ending = EndingTypeEnum.Default)
    {
        Time.timeScale = 0;
        spawningObjects = false;

        switch (ending)
        {
            case EndingTypeEnum.Weight:
                TriggerWeightEnding();
                break;

            case EndingTypeEnum.Ouroborous:
                TriggerOuroborousEnding();
                break;

            case EndingTypeEnum.Secret:
                TriggerSecretEnding();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Triggers the weight related ending if the player collides with a hook.
    /// </summary>
    private void TriggerWeightEnding()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Triggers the Ouroborous ending if the player collides their own tail.
    /// </summary>
    private void TriggerOuroborousEnding()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Triggers the secret ending if the player becomes too long.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void TriggerSecretEnding()
    {
        throw new NotImplementedException();
    }
}
