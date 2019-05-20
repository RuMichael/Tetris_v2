using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{    

    float timer;
    public struct Point
    {
        public float i;
        public float j;
    }

    #region #const
    public const int GridHeight = 20;
    public const int GridWeight = 10;
    #endregion

    #region #variable

    public int[] scoreRow = {10, 30, 80, 200};  

    public GameObject[] prefabs;
    
    public int completedRowsForUpdateDifficulty = 15;
    public int maxLevelDifficulty = 5;    
    bool isDone = false;
    public Transform[,] grid = new Transform[GridWeight, GridHeight];
    int numberOfRowsThisTurn = 0;
    int currentScore = 0;
    int countRows = 0;
    int speedDifficulty = 1;
    int countSpeedAtRows = 0;

    TetroMino previewTetromino;
    TetroMino nextTetromino;

    public Player player;
    
    public Vector3 changeGridPosition;


    public Text hub_Name;
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
        for (int y = 0; y < GridHeight; y++)
            for (int x = 0; x < GridWeight; x++)
                grid[x, y] = null;
        this.player = player;        
        previewTetromino = null;
        hub_Name.text = player.GetName;
        timer = Time.time;
        SpawnNextTetromino();
    }

    void Update()
    {
        UpdateScore();
        UpdateUI();
        UpdateDifficultySpeed();
    }

    #region #работа с UI и расчет скорости строк и сложности

    void UpdateUI()
    {
        hub_Score.text = "Score: \n" + currentScore; ;
        hub_Difficulty.text = "Difficulty: \n" + speedDifficulty.ToString(); ;
        hub_CompletedRows.text = "Completed Rows:\n" + countRows.ToString(); ;
    }

    void UpdateDifficultySpeed()
    {
        if ((countRows - countSpeedAtRows >= completedRowsForUpdateDifficulty || Input.GetKeyDown(player.GetControl[Player.comand.speedUp]) || Input.GetKeyDown(player.GetControl[Player.comand.speedUpOther])) 
        && speedDifficulty < maxLevelDifficulty)
        {
            speedDifficulty++;
            countSpeedAtRows = countRows;            
        }
        else if ((Input.GetKeyDown(player.GetControl[Player.comand.speedDown]) || Input.GetKeyDown(player.GetControl[Player.comand.speedDownOther])) && speedDifficulty > 1)
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

    #region #создание новой Тетромино

    public void SpawnNextTetromino()        
    {
        if (previewTetromino == null)
        {
            GameObject next = (GameObject)Instantiate(GetRandomTetromino(), transform);            
            changeGridPosition = new Vector3(4, 20, 0);      
            nextTetromino = next.GetComponent<TetroMino>();         
        }
        else
        {
            nextTetromino = previewTetromino;             
            nextTetromino.enabled = true;
            nextTetromino.transform.parent = transform;
            nextTetromino.transform.localPosition = Vector3.zero;
        }
        nextTetromino.SetGame = this;

        GameObject preview = (GameObject)Instantiate(GetRandomTetromino(), previewTetrominoTransform);
        previewTetromino = preview.GetComponent<TetroMino>();
        previewTetromino.enabled = false;
    }

    GameObject GetRandomTetromino()      
    {
        return prefabs[Random.Range(0, prefabs.Length)];
        //int randomTetromino = Random.Range(0, prefabsName.Length);
        //return prefab + prefabsName[randomTetromino];
    }

    #endregion

    /// <summary>
    /// запись в Grid новых Tetromino
    /// </summary>
    /// <param name="tetroMino"></param>
    public void UpdateGrid(TetroMino tetroMino)         
    {
        for (int y = 0; y < GridHeight; y++)
            for (int x = 0; x < GridWeight; x++)
                if (grid[x, y] != null && grid[x, y].parent == tetroMino.transform)
                    grid[x, y] = null;

        foreach (Transform mino in tetroMino.transform)
        {
            Point pos = ReverseVector(TetroMino.positionDeterminationMino(mino));
            if (pos.j < GridHeight)
            {                
                grid[(int)pos.i, (int)pos.j] = mino;
            }
        }
    }

    #region #проверка строк и удаление заполненых строк со смещением вниз

    /// <summary>
    /// удаление заполненых строк со смещением вниз
    /// </summary>
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

    /// <summary>
    /// возвращает значение из grid по значению Point
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Transform GetTransformAtGridPosition(Point pos)  // возвращает значение из grid по значению Point
    {
        if (pos.j > GridHeight - 1)
            return null;
        return grid[(int) pos.i, (int) pos.j];
    }

    /// <summary>
    /// проверка на превышение границ Grid
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckIsInsideGrid(Point position)     
    {
        return (position.i >= 0 && position.j >= 0 && position.i < GridWeight);
    }

    /// <summary>
    /// конвертируем позицию Transform в позицию для массива Grid
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Point ReverseVector(Vector3 position)      
    {
        return new Point{i = (int)Mathf.Round(position.x + changeGridPosition.x), j =  (int)Mathf.Round(position.y + changeGridPosition.y)};
    }

    /// <summary>
    /// Проверка на превышение верхней границы грид
    /// </summary>
    /// <param name="tetromino"></param>
    /// <returns></returns>
    public bool CheckIsAboveGrid(TetroMino tetromino) 
    {
        foreach (Transform mino in tetromino.transform)
        {
            Point pos = ReverseVector(TetroMino.positionDeterminationMino(mino));
            if (pos.j > GridHeight - 1)
                return true;
        }
        return false;
    }

    #region #завершение игровой сессии

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

    public void PlayerLost()
    {
        IsDone = true;        
        player.Difficulty = speedDifficulty;
        player.Rows = countRows;
        player.Score = currentScore;
        player.Timer = Time.time - timer;
        GameMetaData.GetInstance().SetPlayer(player);
        ManagerGame.singlton.CheckGameOver();
    }

    #endregion

}
