using Assets.Scripts.Classes;
using Assets.Scripts.Services.DataStorageService;
using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float cnst_moveSpeed = 10.0f;
    private IDataStorage _dataStorage;
    private PlayerBodyModel playerBodyModel;
    private float screenSize;
    private float screenRectWidth;
    private int playerMaxLength;
    private bool tryToGrowPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        _dataStorage = GameManagerScript.Instance._dataStorage;

        if (_dataStorage == null)
        {
            throw new NullReferenceException("Unable to retrieve the data storage service from the game controller");
        }

        screenSize = _dataStorage.GetScreenSize();
        screenRectWidth = _dataStorage.GetScreenRectWidth();
        playerMaxLength = _dataStorage.GetPlayerMaxLength();

        InitializePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.A) && this.transform.position.x - cnst_moveSpeed / 2 > -screenSize - 0.5f)
        {
            this.transform.Translate(new Vector3(-1 * cnst_moveSpeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.D) && this.transform.position.x + cnst_moveSpeed / 2 < screenSize + 0.5f)
        {
            this.transform.Translate(new Vector3(1 * cnst_moveSpeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.W) && this.transform.position.y + cnst_moveSpeed / 2 < (screenSize / (2 * screenRectWidth)) - 0.5f)
        {
            this.transform.Translate(new Vector3(0, 1 * cnst_moveSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S) && this.transform.position.y - cnst_moveSpeed / 2 > -(screenSize / (2 * screenRectWidth)) + 0.5f)
        {
            this.transform.Translate(new Vector3(0, -1 * cnst_moveSpeed * Time.deltaTime));
        }
    }

    void GrowPlayer(GameObject objectToDestroy)
    {
        Destroy(objectToDestroy);

        if (tryToGrowPlayer)
        {
            tryToGrowPlayer = playerBodyModel.IncreasePlayerSize();
        }
        else
        {
            playerBodyModel.UpdateSegmentCount();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.ToLower() == GameTagsEnum.Drag.ToString().ToLower())
        {
            this.transform.Translate(new Vector3(0, -1 * cnst_moveSpeed * 0.5f * Time.deltaTime));
        }
        else if (collision.gameObject.tag.ToLower() == GameTagsEnum.ReverseDrag.ToString().ToLower())
        {
            this.transform.Translate(new Vector3(0, 1 * cnst_moveSpeed * 0.5f * Time.deltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var currentTag = collision.gameObject.tag.ToLower();

        if (currentTag == GameTagsEnum.Obstacle.ToString().ToLower())
        {
            GameManagerScript.Instance.GameOver(EndingTypeEnum.Default);
        }
        else if (currentTag == GameTagsEnum.Hook.ToString().ToLower())
        {
            GameManagerScript.Instance.GameOver(EndingTypeEnum.Weight);
        }
        else if (currentTag == GameTagsEnum.Tail.ToString().ToLower())
        {
            if (playerBodyModel.SegmentCount >= playerMaxLength)
            {
                GameManagerScript.Instance.GameOver(EndingTypeEnum.Secret);
            }
            else
            {
                GameManagerScript.Instance.GameOver(EndingTypeEnum.Ouroborous);
            }
        }
        else if (currentTag == GameTagsEnum.Scale.ToString().ToLower())
        {
            GrowPlayer(collision.gameObject);
        }
    }

    void InitializePlayer()
    {
        var playerBodyObj = _dataStorage.GetUniquePlayerPart(Constants.GameObjects.PlayerBody);
        var playerBody = Instantiate(playerBodyObj, this.transform.position - new Vector3(0, this.transform.localScale.y, 0), new Quaternion());
        var bodyJoint = playerBody.GetComponent<RelativeJoint2D>();

        bodyJoint.connectedBody = this.GetComponent<Rigidbody2D>();
        bodyJoint.autoConfigureOffset = false;

        var playerTailObj = _dataStorage.GetUniquePlayerPart(Constants.GameObjects.PlayerTail);
        var playerTail = Instantiate(playerTailObj, playerBody.transform.position - new Vector3(0, playerBody.transform.localScale.y, 0), new Quaternion());
        var tailJoint = playerTail.GetComponent<RelativeJoint2D>();

        tailJoint.connectedBody = playerBody.GetComponent<Rigidbody2D>();
        tailJoint.autoConfigureOffset = false;

        playerBodyModel = new PlayerBodyModel(playerBody, playerTail, playerBodyObj, playerMaxLength);
    }
}
