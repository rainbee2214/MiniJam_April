using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct GridPosition
{
    float x;
    float y;

    //maybe some other things?
}
public class BlockController : MonoBehaviour
{
    public static BlockController controller;

    GameObject blockPrefab;
    List<Block> blocks;
    public int poolSize = 30;
    int topBlock;
    [HideInInspector]
    public float deltaY = 0.85f;
    [HideInInspector]
    public float deltaX = 0.5f;

    public bool usingControllerKeyboard = false;

    Sprite[] baseBlockSprites;

    [HideInInspector]
    public Dictionary<Vector2, Block> usedPositions;

    public GameObject startingGround;

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
        usedPositions = new Dictionary<Vector2, Block>();
        blockPrefab = Resources.Load<GameObject>("Prefabs/Block");
        baseBlockSprites = Resources.LoadAll<Sprite>("Sprites/BaseBlocks");
        CreateBlockPool();

        for (int i = 0; i < startingGround.transform.childCount; i++)
        {
            Transform t = startingGround.transform.GetChild(i);
            SetNewBlock(t.transform.position, t.GetComponent<Block>());
        }

        //RandomizeBlocks();
    }

    void CreateBlockPool()
    {
        blocks = new List<Block>();
        for (int i = 0; i < poolSize; i++)
        {
            blocks.Add(Instantiate<GameObject>(blockPrefab).GetComponent<Block>());
            blocks[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right mouse click?");
            GetTopBlock();
        }
    }
    public void CircularPlacement()
    {

    }

    public bool PositionUsedEh(Vector2 p)
    {
        foreach (KeyValuePair<Vector2, Block> position in usedPositions)
        {
            if (position.Key == p) return true;
        }
        return false;
    }

    public void RemovePosition(Vector2 p)
    {
        //remove the dictionary position at b
        usedPositions.Remove(p);
    }

    public void SetNewBlock(Vector2 p, Block b)
    {
        usedPositions[p] = b;
    }

    public Block GetTopBlock()
    {
        //place the top block and increment
        Block b = blocks[topBlock];

        b.SetSprite(GetRandomBaseSprite());

        b.moving = true;
        b.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        b.gameObject.SetActive(true);
        topBlock++;
        if (topBlock >= blocks.Count) topBlock = 0; //the block pool may not be big enough if this happens
        return b;
    }

    public void RandomizeBlocks()
    {
        float x = 0, y = 0;
        for (int i = 0; i < poolSize; i++)
        {
            if (i >= poolSize / 2)
            {
                x = i % (poolSize / 2) + deltaX;
                y = 1 * deltaY;
            }
            else
            {
                x = i;
                y = 0;
            }
            blocks[i].Setup((Vector2)transform.position + new Vector2(x, y), GetRandomBaseSprite());
            blocks[i].gameObject.SetActive(true);
        }
    }

    public Sprite GetRandomBaseSprite()
    {
        return baseBlockSprites[Random.Range(0, baseBlockSprites.Length)];
    }
}
