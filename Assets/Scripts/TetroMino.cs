using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetroMino : MonoBehaviour
{
    public bool AllowRotation;
    public bool LimitRotation;

    float fall = 0;
    public float fallspeed = 1;  

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
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
            if (AllowRotation)
            {
                if (LimitRotation && transform.rotation.eulerAngles.z >= 90)
                    transform.Rotate(0, 0, -90);
                else
                    transform.Rotate(0, 0, 90);
                if (!CheckIsVallidPosition())
                {
                    if (LimitRotation && !(transform.rotation.eulerAngles.z >= 90))
                        transform.Rotate(0, 0, 90);
                    else
                        transform.Rotate(0, 0, -90);
                }
                else
                    FindObjectOfType<Game>().UpdateGrid(this);
            }
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) || Time.time - fall >= fallspeed) {
            transform.position += new Vector3(0, -1, 0);
            fall = Time.time;
            if (!CheckIsVallidPosition())
            {
                transform.position += new Vector3(0, 1, 0);
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
            else
                FindObjectOfType<Game>().UpdateGrid(this);
        }

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

}
