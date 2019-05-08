using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{

    Player player1=null;
    Player player2=null;

    Game game1;
    Game game2;

    //public Text hub_score1;
    //public Text hub_rows1;
    //public Text hub_difficulty1;
    //public Text hub_name1;

    //public Text hub_score2;
    //public Text hub_rows2;
    //public Text hub_difficulty2;
    //public Text hub_name2;

    void Start()
    {
        GameObject game = (GameObject)Instantiate(Resources.Load("Prefabs/Grid", typeof(GameObject)), new Vector2(-14f, 4.4f), Quaternion.identity);
        game1 = game.GetComponentInChildren<Game>();        
        player1 = Player.GetPlayerr();
        int startPosition = 0;
        game1.GoStart(player1, startPosition);

        if(Player.GetCount != 0)
        {
            player2 = Player.GetPlayerr();
            game = (GameObject)Instantiate(Resources.Load("Prefabs/Grid", typeof(GameObject)), new Vector2(15.8f, 4.4f), Quaternion.identity);
            game2 = game.GetComponentInChildren<Game>();    
            startPosition = 0;
            game2.GoStart(player2, startPosition);            
        }
        //else
        //{   
        //    hub_name2.text = "";
        //    hub_score2.text = "";
        //    hub_rows2.text = "";
        //    hub_difficulty2.text = "";
        //}
    }
    void Update()
    {
        //hub_score1.text = game1.GetScore;
        //hub_rows1.text = game1.GetCompletedRows;
        //hub_difficulty1.text = game1.GetDifficulty;

        //if(game2 != null)
        //{
        //    hub_score2.text = game2.GetScore;
        //    hub_rows2.text = game2.GetCompletedRows;
        //    hub_difficulty2.text = game2.GetDifficulty;
        //}
        GameOver();
    }

    void GameOver()
    {
        if(game2 == null)
        {
            if (game1.IsDone)
                SceneManager.LoadScene("GameOver");
        }
        else
            if(game1.IsDone && game2.IsDone)
                SceneManager.LoadScene("GameOver");


    }
}
