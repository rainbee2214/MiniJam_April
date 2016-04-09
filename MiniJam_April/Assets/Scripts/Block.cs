using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{
    SpriteRenderer sr;
    public bool moving;
    Vector2 nextPosition;

    public bool stuckNowEh = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (moving)
        {
            ControllerSupport.controller.sr.enabled = false;
            if (BlockController.controller.usingControllerKeyboard)
            {
                nextPosition = ControllerSupport.controller.transform.position;
            }
            else
            {
                nextPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            ControllerSupport.controller.sr.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (moving) { transform.position = nextPosition; }
    }

    public void Setup(Vector2 position, Sprite newSprite)
    {
        transform.position = position;
        sr.sprite = newSprite;
    }

    public void SetSprite(Sprite s)
    {
        sr.sprite = s;
    }
    public void OnMouseDown()
    {
        if (!stuckNowEh)
        {
            moving = true;
        }
    }

    public void OnMouseUpSim()
    {
        OnMouseUp();
    }

    public void OnMouseDownSim()
    {
        OnMouseDown();
    }

    public void OnMouseUp()
    {
        if (!stuckNowEh)
        {
            moving = false;
            SnapToGrid();
        }
    }

    public void SnapToGrid()
    {
        //check the mouse/block position and lock to it to the nearest grid space, if that space isn't taken

        Vector2 t = transform.position;
        t.x = Mathf.RoundToInt(t.x) + (Mathf.RoundToInt(t.y) % 2 == 0 ? BlockController.controller.deltaX : 0);
        t.y = Mathf.RoundToInt(t.y) * BlockController.controller.deltaY;

        //check if this position has been used already - if so, explode?
        if (!BlockController.controller.PositionUsedEh(t))
        {
            BlockController.controller.SetNewBlock(t, this);
            stuckNowEh = true;
            transform.position = t;
            AudioController.controller.PlayRightSound();

            if (BlockController.controller.usingControllerKeyboard) ControllerSupport.controller.SetBlockNull();
        }
        else
        {
            moving = true;
            Debug.Log("Can't place  block here");
            //if (BlockController.controller.usingControllerKeyboard) ControllerSupport.controller.SetCurrentBlock(this);
            AudioController.controller.PlayWrongSound();
        }
    }

    float distance = 1f;
    public void ExplodeBlock()
    {
        gameObject.SetActive(false);
        //explode all other blocks that are touching
        //Send out rays in each direction to see if touching anything, if so, explode it as well
        //RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, distance);
        //RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, distance);
        //RaycastHit2D[] hitUp = Physics2D.RaycastAll((Vector2)transform.position - new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.up, distance);
        //RaycastHit2D[] hitUp2 = Physics2D.RaycastAll((Vector2)transform.position + new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.up, distance);
        //RaycastHit2D[] hitDown = Physics2D.RaycastAll((Vector2)transform.position - new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.down, distance);
        //RaycastHit2D[] hitDown2 = Physics2D.RaycastAll((Vector2)transform.position + new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.down, distance);

        //Debug.DrawRay((Vector2)transform.position + new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.down * distance);
        //Debug.DrawRay((Vector2)transform.position - new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.down * distance);
        //Debug.DrawRay((Vector2)transform.position + new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.up * distance);
        //Debug.DrawRay((Vector2)transform.position - new Vector2(BlockController.controller.deltaX / 2f, 0f), Vector2.up * distance);
        //Debug.DrawRay(transform.position, Vector2.right * distance);
        //Debug.DrawRay(transform.position, Vector2.left * distance);

        //if (hitRight != null && hitRight.Length > 1)
        //{
        //    Debug.Log("Block to the right!");
        //    hitRight[1].collider.GetComponent<Block>().ExplodeBlock();
        //}

        //if (hitLeft != null && hitLeft.Length > 1)
        //{
        //    Debug.Log("Block to the left!");
        //    hitLeft[1].collider.GetComponent<Block>().ExplodeBlock();
        //}

        //if (hitUp != null && hitUp.Length > 1)
        //{
        //    Debug.Log("Block up!");
        //    hitUp[1].collider.GetComponent<Block>().ExplodeBlock();
        //}

        //if (hitDown != null && hitDown.Length > 1)
        //{
        //    Debug.Log("Block down!");
        //    hitDown[1].collider.GetComponent<Block>().ExplodeBlock();
        //}

        BlockController.controller.RemovePosition(transform.position);
    }
}
