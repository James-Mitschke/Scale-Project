using Assets.Scripts.Classes;
using Assets.Scripts.Services.DataStorageService;
using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float cnst_moveSpeed = 10.0f;
    private IDataStorage _dataStorage;
    private float screenSize;
    private float screenRectWidth;

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
}
