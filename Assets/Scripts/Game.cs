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
    #endregion
    
    #region //variable

    public int[] scoreRow = {10, 30, 80, 200};  
    public string prefab = "Prefabs/";
    public string[] prefabsName = {
            "Tetromino_I",
            "Tetromino_J",
            "Tetromino_L",
            "Tetromino_O",
            "Tetromino_S",
            "Tetromino_T",
            "Tetromino_Z"};
    public int completedRowsForUpdateDifficulty = 15;
    public int maxLevelDifficulty = 5;    
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
    public void GoStart(Player player)
    {
        this.player = player;
        SpawnNextTetromino();
        previewTetromino = null;
    }

    void Update()
    {
        UpdateScore();
        UpdateUI();
        UpdateDifficultySpeed();
    }

    #region // работа с UI и расчет скорости строк и сложности

    void UpdateUI()
    {
        hub_Score.text = "Score: \n" + currentScore; ;
        hub_Difficulty.text = "Difficulty: \n" + speedDifficulty.ToString(); ;
        hub_CompletedRows.text = "Completed Rows:\n" + countRows.ToString(); ;
    }

    void UpdateDifficultySpeed()
    {
        if ((countRows - countSpeedAtRows >= completedRowsForUpdateDifficulty || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) && speedDifficulty < maxLevelDifficulty)
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

    void UpdateScore()
    {
        if (numberOfRowsThisTurn>0 && numberOfRowsThisTurn<=4)
        {
            currentScore += scoreRow[numberOfRowsThisTurn-1];
            countRows += numberOfRowsThisTurn;
        }
        numberOfRowsThisTurn = 0;
    }

    #endregion

    #region //создание новой Тетромино

    public void SpawnNextTetromino()        //спавн Tetromino написано по видеоуроку, превью сделано по аналогии 
    {
        if (previewTetromino == null)
        {
            GameObject next = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), transform);
            changeGridPosition = new Vector2(4 /*+ transform.localPosition.x*/ ,20 /*+ transform.localPosition.y*/);      
            nextTetromino = next.GetComponent<TetroMino>();            
        }
        else
        {
            nextTetromino = previewTetromino;             
            nextTetromino.enabled = true;
            nextTetromino.transform.parent = transform;
        }
        nextTetromino.SetGame = this;

        GameObject preview = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoTransform);
        previewTetromino = preview.GetComponent<TetroMino>();
        previewTetromino.enabled = false;
    }

    string GetRandomTetromino()     //рандомный выбор tetromino для спавна 
    {
        int randomTetromino = Random.Range(0, 7);
        return prefab + prefabsName[randomTetromino];
    }

    #endregion

    public void UpdateGrid(TetroMino tetroMino)         //запись в Grid новых Tetromino
    {
        for (int y = 0; y < GridHeight; y++)
            for (int x = 0; x < GridWeight; x++)
                if (grid[x, y] != null && grid[x, y].parent == tetroMino.transform/*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/)
                    grid[x, y] = null;

        foreach (Transform mino in tetroMino.transform)
        {
            Point pos = ReverseVector(mino.localPosition);
            if (pos.j < GridHeight)
                grid[(int)pos.i, (int)pos.j] = mino;
        }
    }

    #region //проверка строк и удаление заполненых строк со смещением вниз

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

    void MoveAllRowDown(int y)  // отпустить все строки с позиции "у"
    {
        for (int i = y; i < GridHeight; i++)        
            MoveRowDown(i);        
    }

    void MoveRowDown(int y)     // отпустить строку на 1 позицию
    {
        for (int x = 0; x < GridWeight; x++)
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].localPosition += Vector3.down;
            }
    }

    #endregion




    public Transform GetTransformAtGridPosition(Point pos)  // возвращает значение из grid по значению Point
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
        return new Point{ i =  Mathf.Round(position.x + changeGridPosition.x), j =  Mathf.Round(position.y + changeGridPosition.y)};
    }

    

    public bool CheckIsAboveGrid(TetroMino tetromino) //Проверка на превышение верхней границы грид
    {
            foreach (Transform mino in tetromino.transform)
            {
                Point pos = ReverseVector(mino.localPosition);
                if (pos.j > GridHeight - 1)
                    return true;
            }
        return false;
    }

    #region // завершение игровой сессии

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
