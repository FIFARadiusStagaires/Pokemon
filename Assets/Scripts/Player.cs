﻿using UnityEngine;
using Classes;
using Classes.Repos;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Player : MonoBehaviour
{
    private float x;
    private float y;
    private float speed = 3f;
    public Vector3 pos;
    private Transform tr;
    public Classes.Player player; 

    private SpriteRenderer sprite;

    public Sprite playerLeft;
    public Sprite playerRight;
    public Sprite playerUp;
    public Sprite playerDown;
    [HideInInspector]
    public bool moving;

    [HideInInspector]
    public Direction dir;

    public bool surfing = false;
    public bool inBattle = false;

    PlayerRepository repo;

    void Start()
    {
        pos = transform.position;
        tr = transform;
        sprite = GetComponent<SpriteRenderer>();
        dir = Direction.Down;
    }

    private void Awake()
    {
        repo = new PlayerRepository();
        DontDestroyOnLoad(this);
        //TODO: Vind uit waarom hier een Null Reference Exception uit komt
        if (player == null)
        {
            player = repo.Load();
            //player = new Classes.Player("Henk", 1, "Male", 0, 0, 0, 0, 0, 2); //Blijkbaar is dit een null reference exception
        }
    }

    private void Update()
    {
        switch(dir)
        {
            case (Direction.Up):
                sprite.sprite = playerUp;
                break;
            case (Direction.Left):
                sprite.sprite = playerLeft;
                break;
            case (Direction.Right):
                sprite.sprite = playerRight;
                break;
            case (Direction.Down):
                sprite.sprite = playerDown;
                break;
        }
    }

    void FixedUpdate()
    {
        if (!inBattle)
        {
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))))
            {
                dir = Direction.Left;
            }
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                dir = Direction.Right;
            }
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                dir = Direction.Up;
            }
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                dir = Direction.Down;
            }
            //CheckButtonPresses();
            RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up, 1);
            RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down, 1);
            RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, 1);
            RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, 1);
            //==Inputs==//
            if (!moving)
            {
                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position == pos && hitleft.collider == null)
                {           //(-1,0)
                    pos += Vector3.left;// Add -1 to pos.x
                }
                if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position == pos && hitright.collider == null)
                {           //(1,0)
                    pos += Vector3.right;// Add 1 to pos.x
                }
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && transform.position == pos && hitup.collider == null)
                {           //(0,1)
                    pos += Vector3.up; // Add 1 to pos.y
                }
                if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && transform.position == pos && hitdown.collider == null)
                {           //(0,-1)
                    pos += Vector3.down;// Add -1 to pos.y
                }
                //The Current Position = Move To (the current position to the new position by the speed * Time.DeltaTime)
                transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
            }
        }
    }
    public void SavePosition()
    {
        x = transform.position.x;
        y = transform.position.y;
    }

    public void GoSurf(string surfDirection)
    {
        surfing = true;
        switch(surfDirection)
        {
            case "s":
                pos += Vector3.down;
                break;
            case "n":
                pos += Vector3.up;
                break;
            case "w":
                pos += Vector3.left;
                break;
            case "e":
                pos += Vector3.right;
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
    }

    public Location GetCurrentLocation()
    {
        return player.CurrentLocation;
    }
}

