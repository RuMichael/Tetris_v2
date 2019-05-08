using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetroMino : MonoBehaviour
{
    public bool AllowRotation;
    public bool LimitRotation;
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
    #region //по видеоурокам

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
            MoveLeft();

        }
        else if (Input.GetKeyDown(control[Player.comand.right]) || (Input.GetKey(control[Player.comand.right]) && Time.time - moving >= 0.085f ))
        {
            
            moving = Time.time;
            MoveRight();            

        }
        else if (Input.GetKeyDown(control[Player.comand.turn]))
        {
            if (AllowRotation)
            {
                RotateTetromino();
            }
        }
        else if (Input.GetKeyDown(control[Player.comand.down]) || Time.time - fall >= fallspeed || (Input.GetKey(control[Player.comand.down]) && Time.time - moving >= 0.035f))
        {
            moving = Time.time;
            MoveDown();
        }
    }
    void MoveLeft()
    {
        transform.position += Vector3.left;
        if (!CheckIsVallidPosition())
            transform.position += Vector3.right;
        else
            game.UpdateGrid(this);
    }
    void MoveRight()
    {
        transform.position += Vector3.right;
        if (!CheckIsVallidPosition())
            transform.position += Vector3.left;
        else
            game.UpdateGrid(this);
    }
    void MoveDown()
    {
        transform.position += Vector3.down;
        fall = Time.time;
        if (!CheckIsVallidPosition())
        {
            transform.position += Vector3.up;
            enabled = false;
            game.DeleteRow();
            if (game.CheckIsAboveGrid(this))
                game.GameOver();
            else
                game.SpawnNextTetromino();
        }
        else
            game.UpdateGrid(this);
    }       
    bool CheckIsVallidPosition() //Проверяем возможность движения Tetromino
    {
        foreach (Transform mino in transform)
        {
            Game.Point pos = game.ReverseVector(mino.position);
            if (!game.CheckIsInsideGrid(pos))
                return false;
            if (game.GetTransformAtGridPosition(pos) != null && game.GetTransformAtGridPosition(pos).parent != transform)
                return false;
        }
        return true;
        
    }
    #endregion 
    void RotateTetromino() // поворот тетрамино на 90
    {
        Transform minoError;

        float centerPositionX = CheckCenterX();
        float tmpCenterPositionX;

        float minPositionY = CheckMinY();
        float tmpMinPositionY;
        if (LimitRotation && transform.rotation.eulerAngles.z >= 90)
            transform.Rotate(0, 0, -90);
        else
            transform.Rotate(0, 0, 90);

        tmpMinPositionY = CheckMinY();
        transform.position += new Vector3(0, minPositionY - tmpMinPositionY, 0); //смещаем положение тетромино до минимального значения по у(чтобы поворот не влиял на положение тетрамино по "у")

        tmpCenterPositionX = CheckCenterX(); 
        transform.position += new Vector3(centerPositionX - tmpCenterPositionX, 0, 0); // смещаем положение по х, но ориентируемся на центр тетромино

        if (!CheckIsVallidPosition(out minoError))
        {            
            if (!PositionOffset(game.ReverseVector(minoError.position).i))
            {
                transform.position -= new Vector3(centerPositionX - tmpCenterPositionX, 0, 0);
                transform.position -= new Vector3(0, minPositionY - tmpMinPositionY, 0);
                if (LimitRotation && !(transform.rotation.eulerAngles.z >= 90))
                {
                    transform.Rotate(0, 0, 90);
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }
            }else
                game.UpdateGrid(this);
        }
        else
            game.UpdateGrid(this);
    }
    bool PositionOffset(float minoErrorX)   //дополнительная после поворота Тетрамино со смещением в сторону от преграды
    {
        float minX = CheckMinX();
        float maxX = CheckMaxX();

        if (minoErrorX - minX > maxX - minoErrorX)      // проверяем с какой стороны возникла ошибка(слева\справа), в зависимости от этого сдвигаем на необходимый вектор (1-2 условных единицы поля)
            transform.position -= new Vector3(maxX - minoErrorX + 1, 0, 0);
        else
            transform.position += new Vector3(minoErrorX - minX + 1, 0, 0);

        if (!CheckIsVallidPosition())
        {
            if (minoErrorX - minX > maxX - minoErrorX)
                transform.position += new Vector3(maxX - minoErrorX + 1, 0, 0);
            else
                transform.position -= new Vector3(minoErrorX - minX + 1, 0, 0);
            return false;
        }
        else            
            return true;
    }
    float CheckMinY()       //найти минимальное значение позиции тетромино по "у"
    {
        float minPosY = 20f;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(mino.position);
            if (minoP.j < minPosY)
                minPosY = minoP.j;
        }

        return minPosY;
    } 
    int CheckCenterX()    //найти центральное(округленное до целого) значение позиции тетромино по "х"
    {
        float result = 0;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(mino.position);
            result += minoP.i;
        }
        return (int)Mathf.Round(result / 4f);
    }
    float CheckMinX()       //найти минимальное значение позиции тетромино по "х"
    {
        float minPosX = 10f;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(mino.position);
            if (minoP.i < minPosX)
                minPosX = mino.position.x;
        }
        return minPosX;
    }
    float CheckMaxX()       //найти максимальное значение позиции тетромино по "у"
    {
        float maxPosX = 0;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(mino.position);
            if (minoP.i > maxPosX)
                maxPosX = minoP.i;
        }
        return maxPosX;
    }
    bool CheckIsVallidPosition(out Transform minoError) //Проверяем возможность движения Tetromino, узнаем какая из mino была неверно установлена
    {
        foreach (Transform mino in transform)
        {
            minoError = mino;
            Game.Point pos = game.ReverseVector(mino.position);
            if (!game.CheckIsInsideGrid(pos))
                return false;
            if (game.GetTransformAtGridPosition(pos) != null && game.GetTransformAtGridPosition(pos).parent != transform)
                return false;
        }
        minoError = null;
        return true;
    }    
}
