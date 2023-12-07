using Assets.Scripts.Classes;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float cnst_moveSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector3(-1 * cnst_moveSpeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector3(1 * cnst_moveSpeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(new Vector3(0, 1 * cnst_moveSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
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
