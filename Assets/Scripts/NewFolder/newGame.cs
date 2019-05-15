using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newGame : MonoBehaviour
{public struct Point
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

    newTetroMino previewTetromino;
    newTetroMino nextTetromino;

    public Player player;
    
    public Vector3 changeGridPosition;

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
        SpawnNextTetromino();
    }

    void Update()
    {
        
    }
    #region #создание новой Тетромино
    public void SpawnNextTetromino()        //спавн Tetromino написано по видеоуроку, превью сделано по аналогии 
    {
        if (previewTetromino == null)
        {
            GameObject next = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), transform);
            changeGridPosition = new Vector3(4 /*+ transform.localPosition.x*/ ,20 /*+ transform.localPosition.y*/, 0);      
            nextTetromino = next.GetComponent<newTetroMino>();         
        }
        else
        {
            nextTetromino = previewTetromino;             
            nextTetromino.enabled = true;
            nextTetromino.transform.parent = transform;
            nextTetromino.transform.localPosition = Vector3.zero;
        }
        nextTetromino.SetGame = this;

        GameObject preview = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), previewTetrominoTransform);
        previewTetromino = preview.GetComponent<newTetroMino>();
        previewTetromino.enabled = false;
    }

    string GetRandomTetromino()     //рандомный выбор tetromino для спавна 
    {
        int randomTetromino = Random.Range(0, 7);
        return prefab + prefabsName[randomTetromino];
    }
    #endregion

    public void UpdateGrid(newTetroMino tetroMino)         
    {
        for (int y = 0; y < GridHeight; y++)
            for (int x = 0; x < GridWeight; x++)
                if (grid[x, y] != null && grid[x, y].parent == tetroMino.transform)
                    grid[x, y] = null;

        foreach (Transform mino in tetroMino.transform)
        {
            Point pos = ReverseVector(newTetroMino.PositionDeterminationMino(mino));
            if (pos.j < GridHeight)
            {                
                grid[(int)pos.i, (int)pos.j] = mino;
            }
        }
    }

    public Point ReverseVector(Vector3 pos)
    {
        return new Point{i = (int) Mathf.Round(pos.x + changeGridPosition.x), j = (int) Mathf.Round(pos.y + changeGridPosition.y)};
    }
}
