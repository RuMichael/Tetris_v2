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
        float scale = (GameMetaData.GetInstance().GetPlayers.Count <= 2) ? 0.9f : 0.7f;
        foreach (Player item in GameMetaData.GetInstance().GetPlayers)
        {
            instGrid = (GameObject)Instantiate(Resources.Load("Prefabs/Grid", typeof(GameObject)));
            float biasPrefab = instGrid.GetComponent<RectTransform>().rect.width * scale;
            instGrid.transform.localScale = new Vector3(scale,scale,0);
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
            SceneManager.LoadScene("GameOver");
        }
    }
}
