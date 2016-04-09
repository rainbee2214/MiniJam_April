using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour
{
    public float moveDelay = 1f;

    public bool moving = true;
    public float inc = 0.01f;
    float percent = 0;
    Vector2 start, end;

    bool jumping = false;

    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(MoveAround());
    }

    void Update()
    {
        if (jumping)
        {
            rb2d.MovePosition(Vector2.Lerp(start, end, percent));
            percent += inc;
            if (percent >= 1)
            {
                jumping = false;
                SnapToGrid();
            }
        }
    }

    void FixedUpdate()
    {

    }

    public IEnumerator MoveAround()
    {
        while (moving)
        {
            percent = 0;
            jumping = true;
            start = transform.position;
            end = (Vector2)transform.position + new Vector2(1, 0);
            yield return new WaitForSeconds(moveDelay);
        }

        yield return null;
    }

    public void SnapToGrid()
    {
        //check the mouse/block position and lock to it to the nearest grid space, if that space isn't taken

        Vector2 t = transform.position;
        t.x = Mathf.RoundToInt(t.x) + (Mathf.RoundToInt(t.y) % 2 == 0 ? BlockController.controller.deltaX : 0);
        t.y = Mathf.RoundToInt(t.y) * BlockController.controller.deltaY;
        transform.position = t;
        if (!BlockController.controller.PositionUsedEh(transform.position))
        {
            rb2d.gravityScale = 3;
            Debug.Log("Nothing under me!! Falling to my death...");
        }
        else
        {
            rb2d.gravityScale = 0;
        }
    }
}
