using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{
    public GameObject prefabGrid;
    public static ManagerGame singlton{get;set;}
    List<Game> games = new List<Game>();
    public Transform startPositionSolo;
    public Transform[] startPositionPlayer2 = new Transform[2];
    public Transform[] startPositionPlayer3 = new Transform[3];

    void Start()
    {
        if (singlton != null)
            Destroy(singlton.gameObject);
        singlton = this;

        Transform[] tmpPosition ;
        List<Player> players = GameMetaData.GetInstance().GetPlayers;

        if (players.Count == 1)        
        {
            tmpPosition = new Transform[1];
            tmpPosition[0] = startPositionSolo;
        }
            
        else if (players.Count == 2)
            tmpPosition = startPositionPlayer2;
        else 
            tmpPosition = startPositionPlayer3;
            
        GameObject instGrid;
        Game game;
        

        for (int i = 0; i < players.Count; i++)
        {
            instGrid = (GameObject)Instantiate(prefabGrid,tmpPosition[i]);
            game = instGrid.GetComponentInChildren<Game>();  
            games.Add(game);
            game.GoStart(players[i]);
        }
    }
    public void CheckGameOver()
    {
        bool check = true;
        foreach (Game game in games)        
            if (!game.IsDone)
                check = false;        
        if (check)        
            SceneManager.LoadScene("GameOver");        
    }
}
