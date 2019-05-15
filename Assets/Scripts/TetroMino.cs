using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetroMino : MonoBehaviour
{
    public bool AllowRotation;
    public bool LimitRotation;
    Game game;      
    Dictionary<Player.comand,KeyCode> control;

    //public Vector3 changeGridPosition;

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
                    game.GameOver();
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
        foreach (Transform mino in transform)
        {
            minoError = mino;
            Game.Point pos = game.ReverseVector(positionDeterminationMino(mino));
            Transform transformAtGridPosition;
            if (!game.CheckIsInsideGrid(pos) || ((transformAtGridPosition = game.GetTransformAtGridPosition(pos)) != null && transformAtGridPosition.parent != transform))
                return false;
        }
        minoError = null;
        return true;
    }

    void RotateTetromino() 
    {
        if (LimitRotation /*&& transform.rotation.eulerAngles.z >= 90 */)
            RotateUp();
        else
            RotateDown();
        if (!CheckIsVallidPosition())
        {
            if (LimitRotation /*&& transform.rotation.eulerAngles.z >= 90 */)
                RotateDown();
            else
                RotateUp();
        }
        else game.UpdateGrid(this);
    }
    void RotateTetrominoS() // поворот тетрамино на 90
    {
        //Transform minoError;

        //float beforeCenterPositionX = CheckCenterX();
        //float afterCenterPositionX;

        //float beforeMinPositionY = CheckMinY();
        //float afterMinPositionY;
        if (LimitRotation /* && transform.rotation.eulerAngles.z >= 90*/)
            RotateUp();
        else
            RotateDown();

        //afterCenterPositionX = CheckCenterX();
       // transform.localPosition += new Vector3(beforeCenterPositionX - afterCenterPositionX, 0, 0); // смещаем положение по х, но ориентируемся на центр тетромино
        //afterMinPositionY = CheckMinY();
        //transform.localPosition += new Vector3(0, beforeMinPositionY - afterMinPositionY, 0); //смещаем положение тетромино до минимального значения по у(чтобы поворот не влиял на положение тетрамино по "у")

        //if (!CheckIsVallidPosition(out minoError))
        //{            
           // if (!PositionOffset(game.ReverseVector(transform.localPosition + minoError.localPosition).i))
          //  {
          //      transform.localPosition -= new Vector3(beforeCenterPositionX - afterCenterPositionX, 0, 0);
           //     transform.localPosition -= new Vector3(0, beforeMinPositionY - afterMinPositionY, 0);
            //    if (LimitRotation && !(transform.rotation.eulerAngles.z >= 90))
            //    {
             //       transform.Rotate(0, 0, 90);
              //  }
               // else
               // {
               //     transform.Rotate(0, 0, -90);
               // }
           // }else
            //    game.UpdateGrid(this);
        //}
        //else
         //   game.UpdateGrid(this);
    }

    bool PositionOffset(float minoErrorX)   //дополнительная после поворота Тетрамино со смещением в сторону от преграды
    {
        float minX = CheckMinX();
        float maxX = CheckMaxX();

        if (minoErrorX - minX > maxX - minoErrorX)      // проверяем с какой стороны возникла ошибка(слева\справа), в зависимости от этого сдвигаем на необходимый вектор (1-2 условных единицы поля)
            transform.localPosition -= new Vector3(maxX - minoErrorX + 1, 0, 0);
        else
            transform.localPosition += new Vector3(minoErrorX - minX + 1, 0, 0);

        if (!CheckIsVallidPosition())
        {
            if (minoErrorX - minX > maxX - minoErrorX)
                transform.localPosition += new Vector3(maxX - minoErrorX + 1, 0, 0);
            else
                transform.localPosition -= new Vector3(minoErrorX - minX + 1, 0, 0);
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
            Game.Point minoP = game.ReverseVector(positionDeterminationMino(mino));
            if (minoP.j < minPosY)
                minPosY = minoP.j;
        }
        return minPosY;
    } 
    float CheckCenterX()    //найти центральное(округленное до целого) значение позиции тетромино по "х"
    {
        float result = 0;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(positionDeterminationMino(mino));
            result += minoP.i;
        }
        return Mathf.Round(result / 4f);
    }
    float CheckMinX()       //найти минимальное значение позиции тетромино по "х"
    {
        float minPosX = 10f;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(positionDeterminationMino(mino));
            if (minoP.i < minPosX)
                minPosX = minoP.i;
        }
        return minPosX;
    }
    float CheckMaxX()       //найти максимальное значение позиции тетромино по "у"
    {
        float maxPosX = 0;
        foreach (Transform mino in transform)
        {
            Game.Point minoP = game.ReverseVector(positionDeterminationMino(mino));
            if (minoP.i > maxPosX)
                maxPosX = minoP.i;
        }
        return maxPosX;
    }

    Vector3 positionDeterminationTetroMino()
    {
        return new Vector3(0,0,0); 
    }

    public static Vector3 positionDeterminationMino(Transform mino)
    {
        /* Transform tetroMino = mino.parent;
        Vector3 minoPos = mino.localPosition;
        float x,y;
        int angle = (int)Mathf.Round(tetroMino.localEulerAngles.z/90);      
        //Debug.Log(angle);
        if (angle != 0 && angle%2 == 1 )
        {
            x = minoPos.x;
            y = minoPos.y;
            minoPos.x = y * (-1);
            minoPos.y = x;
        }
        if (angle != 0 && angle < 1 )
        {
            minoPos.x *= (-1);
            minoPos.y *= (-1);
        }
        return tetroMino.localPosition + minoPos; */
        return mino.parent.localPosition+ mino.localPosition;
    }
     
     void RotateUp()
    {
        float x,y;
        foreach (Transform mino in transform)        
        {
            x = mino.localPosition.x;
            y = mino.localPosition.y;
            mino.localPosition = new Vector3(y * (-1), x, 0);
        }

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

}
