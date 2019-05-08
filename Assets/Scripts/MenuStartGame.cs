using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartGame : MonoBehaviour
{
    public Text hud_Header;
    void Start()
    {
        hud_Header.text =" TETRIS ";
    }
    public void PlayTogether()
    {
        int[] array = {1,2,3,4};
        List<int> list = new List<int> {1,2,3,4};
        Player player = new Player();
        player.AddPlayer("Player1", new Dictionary<Player.comand, KeyCode> {
            {Player.comand.turn, KeyCode.UpArrow},
            {Player.comand.left, KeyCode.LeftArrow},
            {Player.comand.down, KeyCode.DownArrow},
            {Player.comand.right, KeyCode.RightArrow}
        });
        PlaySolo();
    }
    public void PlaySolo()
    {
        Player player = new Player();
        player.AddPlayer("Player2", new Dictionary<Player.comand, KeyCode> {
            {Player.comand.turn, KeyCode.W},
            {Player.comand.left, KeyCode.A},
            {Player.comand.down, KeyCode.S},
            {Player.comand.right, KeyCode.D},
            /*{Player.comand.turn, KeyCode.UpArrow},
            {Player.comand.left, KeyCode.LeftArrow},
            {Player.comand.down, KeyCode.DownArrow},
            {Player.comand.right, KeyCode.RightArrow}*/
        });
        
        SceneManager.LoadScene("Level");
    }
}
