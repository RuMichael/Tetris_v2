using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetroMino : MonoBehaviour
{
    public bool AllowRotation;
    public bool LimitRotation;
    bool checkRotation = true;
    Game game;      
    Dictionary<Player.comand,KeyCode> control;
    public Game SetGame
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
        if ( Input.GetKeyDown(control[Player.comand.left]) || Input.GetKeyDown(control[Player.comand.leftOther]) 
        || ( ( Input.GetKey(control[Player.comand.left]) || Input.GetKey(control[Player.comand.leftOther]) ) && Time.time - moving >= 0.085f ) )
        {
            moving = Time.time;
            Move(Vector3.left);
        }
        else if ( Input.GetKeyDown(control[Player.comand.right]) || Input.GetKeyDown(control[Player.comand.rightOther]) 
        || ( ( Input.GetKey(control[Player.comand.right]) || Input.GetKey(control[Player.comand.rightOther]) ) && Time.time - moving >= 0.085f ) )
        {
            moving = Time.time;
            Move(Vector3.right);   
        }
        else if (Input.GetKeyDown(control[Player.comand.turn]) || Input.GetKeyDown(control[Player.comand.turnOther]) )
        {
            if (AllowRotation)            
                RotateTetromino();            
        }
        else if ( Input.GetKeyDown(control[Player.comand.down]) || Input.GetKeyDown(control[Player.comand.downOther]) || Time.time - fall >= fallspeed 
        || ( ( Input.GetKey(control[Player.comand.down]) || Input.GetKey(control[Player.comand.downOther]) ) && Time.time - moving >= 0.035f))
        {
            moving = Time.time;
            Move(Vector3.down);
        }
    }
    
    void Move(Vector3 move)     //движение влево вправо вниз
    {
        transform.localPosition += move;
        if (move == Vector3.down)
            fall = Time.time;
        if (!CheckIsVallidPosition())
        {
            transform.localPosition -= move;
            if (move == Vector3.down)
            {
                enabled = false;
                game.DeleteRow();
                if (game.CheckIsAboveGrid(this))
                    game.PlayerLost();
                else
                    game.SpawnNextTetromino();
            }
        }
        else
            game.UpdateGrid(this);
    }       

    bool CheckIsVallidPosition() //Проверяем возможность движения Tetromino
    {
        foreach (Transform mino in transform)
        {
            Game.Point pos = game.ReverseVector(positionDeterminationMino(mino));
            Transform transformAtGridPosition;
            if (!game.CheckIsInsideGrid(pos) || ((transformAtGridPosition = game.GetTransformAtGridPosition(pos)) != null && transformAtGridPosition.parent != transform))
                return false;
        }
        return true;
    }

    bool CheckIsVallidPosition(out Transform minoError) //Проверяем возможность движения Tetromino, узнаем какая из mino была неверно установлена
    {   
        minoError = null;
        bool check = true;
        float maxPosX = 0;
        foreach (Transform mino in transform)
        {
            Game.Point pos = game.ReverseVector(positionDeterminationMino(mino));
            Transform transformAtGridPosition;
            if (!game.CheckIsInsideGrid(pos) || ((transformAtGridPosition = game.GetTransformAtGridPosition(pos)) != null && transformAtGridPosition.parent != transform))
            {
                check = false;
                if(maxPosX < Mathf.Abs(mino.localPosition.x))
                {
                    minoError = mino;
                    maxPosX = Mathf.Abs(mino.localPosition.x);
                }
            }
        }
        if (check)
            minoError = null;
        return check;
    }

    void RotateTetromino() 
    {
        Transform minoError;
        float minPosYLast = CheckMinPosY();
        if (LimitRotation)
        {
            if(checkRotation)
                RotateUp();
            else
                RotateDown();
            checkRotation = !checkRotation;
        }
        else        
            RotateUp();        

        float minPosYNext = CheckMinPosY();
        if (minPosYLast!= minPosYNext)
            transform.localPosition += new Vector3(0, minPosYLast - minPosYNext, 0);
        if (!CheckIsVallidPosition(out minoError))
        {
            if((minoError != null && !PositionOffsetMino(minoError.localPosition.x)) || minoError == null )
            {
                if (LimitRotation)
                {
                    if(checkRotation)
                        RotateUp();
                    else
                        RotateDown();
                    checkRotation = !checkRotation;
                }
                else            
                    RotateDown();
                if (minPosYLast!= minPosYNext)
                    transform.localPosition -= new Vector3(0, minPosYLast - minPosYNext, 0);
            }
        }
        else game.UpdateGrid(this);
    }
    bool PositionOffsetMino(float minoErrorX)
    {
        transform.localPosition -= new Vector3(minoErrorX,0,0);
        if (!CheckIsVallidPosition())
        {
            transform.localPosition += new Vector3(minoErrorX,0,0);
            return false;
        }
        game.UpdateGrid(this);
        return true;
    }    
    public static Vector3 positionDeterminationMino(Transform mino)
    {
        return mino.parent.localPosition + mino.localPosition;
    }     
    void RotateUp()
    {
        foreach (Transform mino in transform) 
            mino.localPosition = new Vector3(mino.localPosition.y * (-1), mino.localPosition.x, 0);
        

    }
    void RotateDown()
    {
        foreach (Transform mino in transform)     
            mino.localPosition = new Vector3(mino.localPosition.y, mino.localPosition.x * (-1), 0);
        
    }

    float CheckMinPosY()
    {
        float result = 0;
        foreach(Transform mino in transform)
        {
            if (mino.localPosition.y < result)
                result = mino.localPosition.y;

        }
        return result;
    }

}
