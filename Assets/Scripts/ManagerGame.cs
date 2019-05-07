using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{

    Player player1;
    Player player2;

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
        game1.player= player1;

        if(Player.GetCount == 2)
            player2 = Player.GetPlayer();
    }
    void Update()
    {
        
    }
}
