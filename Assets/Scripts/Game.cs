using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static int GridHeight = 20;
    public static int GridWeight = 10;
    public static Transform[,] grid = new Transform[GridWeight, GridHeight];

    public static int scoreOneRow = 10;
    public static int scoreTwoRow = 30;
    public static int scoreThreeRow = 80;
    public static int scoreFourRow = 200;

    static int numberOfRowsThisTurn = 0;
    int currentScore = 0;
    int countRows = 0;
    int speedDifficulty = 1;
    int countSpeedAtRows = 0;

    bool isStart = true;
    GameObject previewTetromino;

    public Text hub_score;
    public Text hub_rows;
    public Text hub_difficulty;

    void Start()
    {
        SpawnNextTetromino();
    }

    void Update()
    {
        UpdateScore();
        UpdateUI();
        UpdateDifficultySpeed();
    }

    void UpdateDifficultySpeed()
    {
        if ((countRows - countSpeedAtRows >= 15 || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) && speedDifficulty < 5)
        {
            speedDifficulty++;
            countSpeedAtRows = countRows;
            TetroMino.fallspeed = 1.0f - (0.15f * speedDifficulty);
        }
        else if ((Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) && speedDifficulty > 1)
        {
            speedDifficulty--;
            countSpeedAtRows = countRows;
            TetroMino.fallspeed = 1.0f - (0.15f * speedDifficulty);
        }
        
    }

    void UpdateUI()
    {
        hub_score.text = "Score: \n" + currentScore.ToString();
        hub_difficulty.text = "Difficulty: \n" + speedDifficulty.ToString();
        hub_rows.text = "Completed Rows:\n" + countRows.ToString();        
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
                grid[x, y - 1].position += new Vector3(0,-1,0);
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
            Vector2 pos = Round(mino.position);
            if (pos.y < GridHeight)
                grid[(int)pos.x, (int)pos.y] = mino;
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 pos)  // возвращает значение из grid по значению Vector2
    {
        if (pos.y > GridHeight - 1)
            return null;
        else
            return grid[(int)pos.x, (int)pos.y];
    }

    public void SpawnNextTetromino()        //спавн Tetromino
    {
        GameObject nextTetromino;
        if (isStart)
        {
            nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(4.0f, 20.0f), Quaternion.identity);
            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(16.0f, 15.0f), Quaternion.identity);
            previewTetromino.GetComponent<TetroMino>().enabled = false;
            isStart = false;
        }else
        {
            nextTetromino = previewTetromino;
            nextTetromino.GetComponent<TetroMino>().enabled = true;
            nextTetromino.transform.position = new Vector2(4.0f, 20.0f);

            previewTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(16.0f, 15.0f), Quaternion.identity);
            previewTetromino.GetComponent<TetroMino>().enabled = false;
        }

           
        
    }

    public bool CheckIsInsideGrid(Vector2 position)     //проверка на превышение границ Grid
    {
        return ((int)position.x >= 0 && (int)position.y >= 0 && (int)position.x < GridWeight);
    }

    public Vector2 Round(Vector2 position)      //выравниваем до четных чисел Vector2, передаем новый элемент
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
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
                Vector2 pos = Round(mino.position);
                if (pos.y > GridHeight - 1)
                    return true;
            }

        
        return false;
    }

    public void GameOver()
    {
        TetroMino.fallspeed = 1.6f;
        Application.LoadLevel("GameOver");        
    }
    

}
