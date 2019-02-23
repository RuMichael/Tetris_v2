using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int GridHeight = 20;
    public static int GridWeight = 10;
    public static Transform[,] grid = new Transform[GridWeight, GridHeight];

    // Start is called before the first frame update
    void Start()
    {
        SpawnNextTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGrid(TetroMino tetroMino)         //запись в Grid новых Tetromino
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWeight; x++)
            {
                if (grid[x, y] != null)
                    if (grid[x, y].parent == tetroMino.transform)
                        grid[x, y] = null;
            }
        }
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
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(4.0f, 20.0f), Quaternion.identity);     
        
    }

    public bool CheckIsInsideGrid(Vector2 position)     //проверка на превышение границ Grid
    {
        return ((int)position.x >= 0 && (int)position.y >= 0 && (int)position.x < GridWeight);
    }

    public Vector2 Round(Vector2 position)      //выравниваем до четных чисел Vector2
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

}
