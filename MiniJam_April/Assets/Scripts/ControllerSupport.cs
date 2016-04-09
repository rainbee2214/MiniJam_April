using UnityEngine;
using System.Collections;

public class ControllerSupport : MonoBehaviour
{
    public static ControllerSupport controller;

    public float speed = 1f;

    Vector2 input;
    Block currentBlock;
    [HideInInspector]
    public SpriteRenderer sr;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (!BlockController.controller.usingControllerKeyboard)
        {
            sr.enabled = false;
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.position;
        }
        else
        {
            sr.enabled = true;
            //treat the targetPosition as a mouseButton
            input.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("PlaceBlock"))
            {
                PlaceBlock();
            }
            else if (Input.GetButtonDown("CreateBlock"))
            {
                CreateBlock();
            }
            else if (Input.GetButtonDown("DestroyBlocks"))
            {
                Debug.Log("Destroying block " + SnapToGrid());
                //Debug.Log("Is there a block where I clicked? " + BlockController.controller.PositionUsedEh(SnapToGrid()));
                if (BlockController.controller.PositionUsedEh(SnapToGrid()))
                {
                    Debug.Log("There is something here!");

                    //Explode Block
                    //BlockController.controller.usedPositions[SnapToGrid()].gameObject.SetActive(false);
                    //BlockController.controller.RemovePosition(SnapToGrid());
                    BlockController.controller.usedPositions[SnapToGrid()].ExplodeBlock();

                    AudioController.controller.PlayExplodeSound();
                }
            }
        }
    }

    public void SetCurrentBlock(Block b)
    {
        currentBlock = b;
    }

    public void SetBlockNull()
    {
        currentBlock = null;
    }

    public void PlaceBlock()
    {
        if (currentBlock == null) return;
        Debug.Log("Placing block");
        // only make this null if the place was actually placed - how do I check this?

        currentBlock.OnMouseUpSim();
        //currentBlock = null;
    }

    public void CreateBlock()
    {
        if (currentBlock != null) return;
        Debug.Log("Creating block");
        currentBlock = BlockController.controller.GetTopBlock();
        // Simuate a right click in the Block Controller
    }

    void FixedUpdate()
    {
        Mathf.Clamp(input.x, Camera.main.transform.position.x - Camera.main.orthographicSize * 16f / 9f, Camera.main.transform.position.x + Camera.main.orthographicSize * 16f / 9f);
        Mathf.Clamp(input.y, Camera.main.transform.position.y - Camera.main.orthographicSize, Camera.main.transform.position.y + Camera.main.orthographicSize);
        transform.position = (Vector2)transform.position + input * speed * Time.deltaTime;
    }


    //this function is everywhere, time to make it more generic - no time right now!
    public Vector2 SnapToGrid()
    {
        Vector2 t = transform.position;
        t.x = Mathf.RoundToInt(t.x) + (Mathf.RoundToInt(t.y) % 2 == 0 ? BlockController.controller.deltaX : 0);
        t.y = Mathf.RoundToInt(t.y) * BlockController.controller.deltaY;
        return t;
    }
}
