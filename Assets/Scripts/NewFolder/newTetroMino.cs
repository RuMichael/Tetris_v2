using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class newTetroMino : MonoBehaviour
{
    public bool AllowRotation;
    public bool LimitRotation;
    newGame game;      
    Dictionary<Player.comand,KeyCode> control;

    public newGame SetGame
    {
        set {
            game = value;
            control = game.player.GetControl;
        }
    }
    float fall = 0;
    public float fallspeed = 1.6f;
    float moving = 0;

    void Start()
    {

    }    
    void Update()
    {        
        CheckUserInput();
    }

    void CheckUserInput()  // работа с кнопками
    {
        if (Input.GetKeyDown(control[Player.comand.left]) || (Input.GetKey(control[Player.comand.left]) && Time.time - moving >= 0.085f ))
        {
            moving = Time.time;
            Move(Vector3.left);
        }
        else if (Input.GetKeyDown(control[Player.comand.right]) || (Input.GetKey(control[Player.comand.right]) && Time.time - moving >= 0.085f ))
        {
            moving = Time.time;
            Move(Vector3.right);   
        }
        else if (Input.GetKeyDown(control[Player.comand.turn]))
        {
            if (AllowRotation)            
                RotateTetromino();            
        }
        else if (Input.GetKeyDown(control[Player.comand.down]) || Time.time - fall >= fallspeed || (Input.GetKey(control[Player.comand.down]) && Time.time - moving >= 0.035f))
        {
            moving = Time.time;
            Move(Vector3.down);
        }
    }

    void RotateTetromino()
    {
        if (LimitRotation)
            RotateUp();
        else
            RotateDown();
    }   

    void RotateUp()
    {
        //List<Transform> minos = new List<Transform>();
        float x,y;
        foreach (Transform mino in transform)        
        {
            x = mino.localPosition.x;
            y = mino.localPosition.y;
            mino.localPosition = new Vector3(y * (-1), x, 0);
        }
        /*minos.Add(mino);
        for (int i=0;i<minos.Count;i++)
        {
            x = minos[0].localPosition.x;
            y = minos[0].localPosition.y;
            minos[0].localPosition = new Vector3(y*(-1),x,0);
        }*/

    }
    void RotateDown()
    {
        float x,y;
        foreach (Transform mino in transform)        
        {
            x = mino.localPosition.x;
            y = mino.localPosition.y;
            mino.localPosition = new Vector3(y, x * (-1), 0);
        }
    }
    
    void Move(Vector3 move)     //движение влево вправо вниз
    {
        transform.localPosition += move;
        if (move == Vector3.down)
            fall = Time.time;
        /* if (!CheckIsVallidPosition())
        {
            transform.localPosition -= move;
            if (move == Vector3.down)
            {
                enabled = false;
                game.DeleteRow();
                if (game.CheckIsAboveGrid(this))
                    game.GameOver();
                else
                    game.SpawnNextTetromino();
            }
        }
        else
            game.UpdateGrid(this);*/
    }   

    public static Vector3 PositionDeterminationMino(Transform mino)
    {
        return mino.parent.localPosition + mino.localPosition;
    }
}
