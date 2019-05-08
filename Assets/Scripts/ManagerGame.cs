using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{
    List<Game> games = new List<Game>();
    public Transform startPosition;

    void Start()
    {
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
    void Update()
    {
        GameOver();
    }

    void GameOver()
    {
        /*if(game2 == null)
        {
            if (game1.IsDone)
                SceneManager.LoadScene("GameOver");
        }
        else
            if(game1.IsDone && game2.IsDone)
                SceneManager.LoadScene("GameOver");*/


    }
}
