using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame singlton{get;set;}
    List<Game> games = new List<Game>();
    public Transform startPosition;

    void Start()
    {
        if (singlton != null)
            Destroy(singlton.gameObject);
        singlton = this;
        int c = 0;
        GameObject instGrid;
        Game game;
        foreach (Player item in Player.Playerss)
        {
            instGrid = (GameObject)Instantiate(Resources.Load("Prefabs/Grid", typeof(GameObject)));
            float biasPrefab = instGrid.GetComponent<RectTransform>().rect.width;
            instGrid.transform.position = new Vector3(startPosition.position.x + biasPrefab * c, startPosition.position.y, startPosition.position.z);
            game = instGrid.GetComponentInChildren<Game>();  
            games.Add(game);
            game.GoStart(item);
            c++;
        }
    }
    public void CheckGameOver()
    {
        bool check = true;
        foreach (Game game in games)        
            if (!game.IsDone)
                check = false;
        
        if (check)
        {
            UpdatePlayers();
            SceneManager.LoadScene("GameOver");
        }
    }

    void UpdatePlayers()
    {
        foreach (Game game in games)
            Player.AddPlayer(game.player);
    }
}
