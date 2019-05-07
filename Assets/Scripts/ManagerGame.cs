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

    public Text hub_score1;
    public Text hub_rows1;
    public Text hub_difficulty1;
    public Text hub_name1;

    public Text hub_score2;
    public Text hub_rows2;
    public Text hub_difficulty2;
    public Text hub_name2;

    void Start()
    {
        GameObject game = (GameObject)Instantiate(Resources.Load("Prefabs/GameMe", typeof(GameObject)), new Vector2(4.0f, 10.0f), Quaternion.identity);
        game1 = game.GetComponent<Game>();            
        player1 = Player.GetPlayer();
        int startPosition = 0;
        game1.GoStart(player1, startPosition);

        if(Player.Players != null && Player.GetCount != 0)
        {
            player2 = Player.GetPlayer();
            game = (GameObject)Instantiate(Resources.Load("Prefabs/Grid", typeof(GameObject)), new Vector2(20.8f, 4.4f), Quaternion.identity);
            game = (GameObject)Instantiate(Resources.Load("Prefabs/GameMe", typeof(GameObject)), new Vector2(4.0f, 10.0f), Quaternion.identity);
            game2 = game.GetComponent<Game>(); 
            startPosition = 24;
            game2.GoStart(player2, startPosition);            
        }else
        {   
            hub_name2.text = "";
            hub_score2.text = "";
            hub_rows2.text = "";
            hub_difficulty2.text = "";
        }
    }
    void Update()
    {
        hub_score1.text = game1.GetScore;
        hub_rows1.text = game1.GetCompletedRows;
        hub_difficulty1.text = game1.GetDifficulty;

        if(player2!=null)
        {
            hub_score2.text = game2.GetScore;
            hub_rows2.text = game2.GetCompletedRows;
            hub_difficulty2.text = game2.GetDifficulty;
        }
    }
}
