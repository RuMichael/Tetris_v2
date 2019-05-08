using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{    
    public struct Point
    {
    public float i;
    public float j;
    }

    #region //const
    public const int GridHeight = 20;
    public const int GridWeight = 10;
    

    public const int scoreOneRow = 10;
    public const int scoreTwoRow = 30;
    public const int scoreThreeRow = 80;
    public const int scoreFourRow = 200;
    #endregion

    #region //variable

    bool isDone = false;
    public Transform[,] grid = new Transform[GridWeight, GridHeight];
    int numberOfRowsThisTurn = 0;
    int currentScore = 0;
    int countRows = 0;
    int speedDifficulty = 1;
    int countSpeedAtRows = 0;

    bool isStart = true;

    TetroMino previewTetromino;
    TetroMino nextTetromino;

    public Player player;
    
    public Vector2 changeGridPosition;

    public Text hub_Score;
    public Text hub_Difficulty;
    public Text hub_CompletedRows;
 
    public Transform previewTetrominoTransform;

    #endregion


    void Start()
    {      
        isDone = false;
        numberOfRowsThisTurn = 0;
        currentScore = 0;
        countRows = 0;
        speedDifficulty = 1;
        countSpeedAtRows = 0;
    }    

    void UpdateDifficultySpeed()
    {
        if ((countRows - countSpeedAtRows >= 15 || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) && speedDifficulty < 5)
        {
            speedDifficulty++;
            countSpeedAtRows = countRows;            
        }
        else if ((Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) && speedDifficulty > 1)
        {
            speedDifficulty--;
            countSpeedAtRows = countRows;            
        }
        nextTetromino.fallspeed = 1.1f - (0.15f * speedDifficulty);
    }

    public void GoStart(Player player)
    {
        this.player = player;
        SpawnNextTetromino();   
    }
        
    void UpdateScore()
    {
        switch (numberOfRowsThisTurn)
        {
            case 1:
                currentScore += scoreOneRow;
                countRows += 1;
                break;
            case 2:
                currentScore += scoreTwoRow;
                countRows += 2;
                break;
            case 3:
                currentScore += scoreThreeRow;
                countRows += 3;
                break;
            case 4:
                currentScore += scoreFourRow;
                countRows += 4;
                break;
        }
        numberOfRowsThisTurn = 0;

    }

    public void SpawnNextTetromino()        //спавн Tetromino написано по видеоуроку, превью сделано по аналогии 
    {
        if (isStart)
        {
            GameObject next = (GameObject)GameObject.Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), transform); 
            changeGridPosition = new Vector2(transform.position.x - 4, transform.position.y - 20);      
            nextTetromino = next.GetComponent<TetroMino>();
            nextTetromino.SetGame = this;

            GameObject preview = (GameObject)GameObject.Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoTransform); 
            previewTetromino = preview.GetComponent<TetroMino>();     
            previewTetromino.enabled = false;  

            isStart = false;
        }
        else
        {
            nextTetromino = previewTetromino;
            //changeGridPosition = new Vector2(transform.position.x - 4, transform.position.y - 20);  
            nextTetromino.SetGame = this;
            nextTetromino.enabled = true;
            nextTetromino.transform.parent = transform;
            nextTetromino.transform.position = transform.position;

            GameObject preview = (GameObject)GameObject.Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoTransform); 
            previewTetromino = preview.GetComponent<TetroMino>();     
            previewTetromino.enabled = false;  
        }
    }
    

    #region с видеоуроков

    void Update()
    {
        UpdateScore();
        UpdateUI();
        UpdateDifficultySpeed();
    }

    void UpdateUI()
    {
        hub_Score.text = GetScore;
        hub_Difficulty.text = GetDifficulty;
        hub_CompletedRows.text = GetCompletedRows;
    }

    public string GetScore
    {
        get{ return "Score: \n" + currentScore;}
    }    
    public string GetDifficulty
    {
        get{ return "Difficulty: \n" + speedDifficulty.ToString();}
    }
    public string GetCompletedRows
    {
        get{ return "Completed Rows:\n" + countRows.ToString();}
    }


    bool IsFullRowAt(int y)     //проверяем заполнена ли строка "у" в grid
    {
        for (int x = 0; x < GridWeight; x++)        
            if (grid[x, y] == null)
                return false;
        numberOfRowsThisTurn++;
        return true;
    }

    void DeleteMinoAt(int y)    //удаляем объекты Mino строки "y" с рабочей области и значение в массиве grid 
    {
        for (int x = 0; x < GridWeight; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
        
    }

    void MoveRowDown(int y)     // отпустить строку на 1 позицию
    {
        for (int x = 0; x < GridWeight; x++)
            if (grid[x,y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }        
    }

    void MoveAllRowDown(int y)  // отпустить все строки с позиции "у"
    {
        for (int i = y; i < GridHeight; i++)        
            MoveRowDown(i);
        
    }

    public void DeleteRow()
    {
        for (int y = 0; y < GridHeight; y++)        
            if (IsFullRowAt(y))
            {
                DeleteMinoAt(y);
                MoveAllRowDown(y + 1);
                --y;
            }
        
    }

    public void UpdateGrid(TetroMino tetroMino)         //запись в Grid новых Tetromino
    {
        for (int y = 0; y < GridHeight; y++)        
            for (int x = 0; x < GridWeight; x++)            
                if (grid[x, y] != null)
                    if (grid[x, y].parent == tetroMino.transform)
                        grid[x, y] = null;
                    
        foreach (Transform mino in tetroMino.transform)
        {
            Point pos = ReverseVector(mino.position);
            if (pos.j < GridHeight)
                grid[(int)pos.i, (int) pos.j] = mino;
        }
    }

    public Transform GetTransformAtGridPosition(Point pos)  // возвращает значение из grid по значению Vector2
    {
        if (pos.j > GridHeight - 1)
            return null;
        else
            return grid[(int) pos.i, (int) pos.j];
    }

    
    public bool CheckIsInsideGrid(Point position)     //проверка на превышение границ Grid
    {
        return (position.i >= 0 && position.j >= 0 && position.i < GridWeight);
    }

    public Point ReverseVector(Vector2 position)      //
    {
        return new Point{ i =  Mathf.Round(position.x - changeGridPosition.x), j =  Mathf.Round(position.y - changeGridPosition.y)};
    }

    string GetRandomTetromino()     //рандомный выбор tetromino для спавна 
    {
        string randomTetrominoName = "";
        int randomTetromino = Random.Range(1, 8);

        switch (randomTetromino)
        {
            case 1:
                randomTetrominoName = "Prefabs/Tetromino_I";
                break;
            case 2:
                randomTetrominoName = "Prefabs/Tetromino_J";                                              
                break;
            case 3:
                randomTetrominoName = "Prefabs/Tetromino_L";
                break;
            case 4:
                randomTetrominoName = "Prefabs/Tetromino_O";
                break;
            case 5:
                randomTetrominoName = "Prefabs/Tetromino_S";
                break;
            case 6:
                randomTetrominoName = "Prefabs/Tetromino_T";
                break;
            case 7:
                randomTetrominoName = "Prefabs/Tetromino_Z";
                break;
        }

        return randomTetrominoName;
    }

    public bool CheckIsAboveGrid(TetroMino tetromino)
    {
            foreach (Transform mino in tetromino.transform)
            {
                Point pos = ReverseVector(mino.position);
                if (pos.j > GridHeight - 1)
                    return true;
            }
        
        return false;
    }

    public bool IsDone
    {
        get{
            return isDone;
        }
        set{
            isDone = value;
            if(value)
            {
                nextTetromino.enabled = false;
                enabled = false;
            }
        }
    }

    public void GameOver()
    {
        IsDone = true;
    }

    #endregion


}
