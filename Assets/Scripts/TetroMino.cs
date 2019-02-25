using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetroMino : MonoBehaviour
{
    public bool AllowRotation;
    public bool LimitRotation;
    

    float fall = 0;
    public static float fallspeed = 1.6f;  

    void Start()
    {
        
    }
    
    void Update()
    {        
        CheckUserInput();
    }

    void CheckUserInput()  // работа с кнопками
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            transform.position += new Vector3(-1, 0, 0);
            if (!CheckIsVallidPosition())
                transform.position += new Vector3(1, 0, 0);
            else
                FindObjectOfType<Game>().UpdateGrid(this);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            transform.position += new Vector3(1, 0, 0);
            if (!CheckIsVallidPosition())
                transform.position += new Vector3(-1, 0, 0);
            else
                FindObjectOfType<Game>().UpdateGrid(this);

        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            if (AllowRotation)
            {                
                RotateTetromino();
            }
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) || Time.time - fall >= fallspeed) {
            transform.position += new Vector3(0, -1, 0);
            fall = Time.time;
            if (!CheckIsVallidPosition())
            {
                transform.position += new Vector3(0, 1, 0);
                enabled = false;
                FindObjectOfType<Game>().DeleteRow();
                if (FindObjectOfType<Game>().CheckIsAboveGrid(this)) 
                    FindObjectOfType<Game>().GameOver();
                else
                    FindObjectOfType<Game>().SpawnNextTetromino();
            }
            else
                FindObjectOfType<Game>().UpdateGrid(this);
        }
    }

    void RotateTetromino()
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
        transform.position += new Vector3(0, minPositionY - tmpMinPositionY, 0);

        tmpCenterPositionX = CheckCenterX();
        transform.position += new Vector3(centerPositionX - tmpCenterPositionX, 0, 0);



        if (!CheckIsVallidPosition(out minoError))
        {
            if (!PositionOffset(minoError.position.x))
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
            }
        }
        else
            FindObjectOfType<Game>().UpdateGrid(this);
    }

    bool PositionOffset(float minoErrorX)
    {
        float minX=CheckMinX();
        float maxX=CheckMaxX();

        if (minoErrorX - minX > maxX - minoErrorX)
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
            FindObjectOfType<Game>().UpdateGrid(this);
        return true;
    }

    float CheckMinY()
    {
        float minPosY = 20f;
        foreach (Transform mino in transform)        
            if (mino.position.y < minPosY)
                minPosY = mino.position.y;
        
        return minPosY;
    }

    float CheckCenterX()
    {
        float[] centerPosX = new float[4];
        int i = 0;
        float result=0;
        foreach (Transform mino in transform)
        {
            centerPosX[i] = mino.position.x;
            i++;
        }
        for (i = 0; i < centerPosX.Length; i++)        
            result += centerPosX[i];

        return Mathf.Round(result/4f);
    }

    float CheckMinX()
    {
        float minPosX = 10f;
        foreach (Transform mino in transform)
            if (mino.position.x < minPosX)
                minPosX = mino.position.x;

        return minPosX;
    }

    float CheckMaxX()
    {
        float maxPosX = 0;
        foreach (Transform mino in transform)
            if (mino.position.x > maxPosX)
                maxPosX = mino.position.x;

        return maxPosX;
    }

    bool CheckIsVallidPosition() //Проверяем возможность движения Tetromino
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (!FindObjectOfType<Game>().CheckIsInsideGrid(pos))
                return false;
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
                return false;
        }
        return true;
    }

    bool CheckIsVallidPosition(out Transform minoError) //Проверяем возможность движения Tetromino, узнаем какая из mino была неверно установлена
    {
        foreach (Transform mino in transform)
        {
            minoError = mino;
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (!FindObjectOfType<Game>().CheckIsInsideGrid(pos))
                return false;
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
                return false;
        }
        minoError = null;
        return true;
    }

}
