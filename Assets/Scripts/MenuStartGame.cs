using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartGame : MonoBehaviour
{
    Stack<Player> tempStorage = new Stack<Player>();
    public Text hud_Header;
    bool playSolo = true;
    void Start()
    {
        hud_Header.text =" TETRIS ";  
        playSolo = true;      
    }

    
    public void PlayTogether()
    {        
        Player player = new Player("Player2", new Dictionary<Player.comand, KeyCode> {
                {Player.comand.turn, KeyCode.KeypadEnter},
                {Player.comand.left, KeyCode.LeftArrow},
                {Player.comand.down, KeyCode.DownArrow},
                {Player.comand.right, KeyCode.RightArrow},
                {Player.comand.speedUp, KeyCode.KeypadPlus},
                {Player.comand.speedDown, KeyCode.KeypadMinus},

                {Player.comand.turnOther, KeyCode.None},
                {Player.comand.leftOther, KeyCode.None},
                {Player.comand.downOther, KeyCode.None},
                {Player.comand.rightOther, KeyCode.None},
                {Player.comand.speedUpOther, KeyCode.None},
                {Player.comand.speedDownOther, KeyCode.None}
            });
        tempStorage.Push(player);    
        playSolo = false;
        PlaySolo();
    }
    public void PlaySolo()
    {
        Player player = null;
        if (playSolo)        
            player = new Player("Player1", new Dictionary<Player.comand, KeyCode> {
                {Player.comand.turn, KeyCode.Space},
                {Player.comand.left, KeyCode.A},
                {Player.comand.down, KeyCode.S},
                {Player.comand.right, KeyCode.D},         
                {Player.comand.speedUp, KeyCode.Equals},
                {Player.comand.speedDown, KeyCode.Minus},
                {Player.comand.turnOther, KeyCode.KeypadEnter},
                {Player.comand.leftOther, KeyCode.LeftArrow},
                {Player.comand.downOther, KeyCode.DownArrow},
                {Player.comand.rightOther, KeyCode.RightArrow},
                {Player.comand.speedUpOther, KeyCode.KeypadPlus},
                {Player.comand.speedDownOther, KeyCode.KeypadMinus}
            });     
        else        
            player = new Player("Player1", new Dictionary<Player.comand, KeyCode> {
                {Player.comand.turn, KeyCode.Space},
                {Player.comand.left, KeyCode.A},
                {Player.comand.down, KeyCode.S},
                {Player.comand.right, KeyCode.D},
                {Player.comand.speedUp, KeyCode.Equals},
                {Player.comand.speedDown, KeyCode.Minus},

                {Player.comand.turnOther, KeyCode.None},
                {Player.comand.leftOther, KeyCode.None},
                {Player.comand.downOther, KeyCode.None},
                {Player.comand.rightOther, KeyCode.None},
                {Player.comand.speedUpOther, KeyCode.None},
                {Player.comand.speedDownOther, KeyCode.None}
            });
        
        tempStorage.Push(player);
        GoPlay();
    }

    public void Playx3()
    {
        Player player = new Player("Player3 ", new Dictionary<Player.comand, KeyCode> {
                {Player.comand.turn, KeyCode.RightControl},
                {Player.comand.left, KeyCode.L},
                {Player.comand.down, KeyCode.Semicolon},
                {Player.comand.right, KeyCode.Quote},
                {Player.comand.speedUp, KeyCode.Equals},
                {Player.comand.speedDown, KeyCode.Minus},

                {Player.comand.turnOther, KeyCode.None},
                {Player.comand.leftOther, KeyCode.None},
                {Player.comand.downOther, KeyCode.None},
                {Player.comand.rightOther, KeyCode.None},
                {Player.comand.speedUpOther, KeyCode.None},
                {Player.comand.speedDownOther, KeyCode.None}
            });
        tempStorage.Push(player);
        PlayTogether();
    }

    void GoPlay()
    {
        while(tempStorage.Count>0)
        {
            Player player = tempStorage.Pop();
            GameMetaData.GetInstance().SetPlayer(player);
        }
        SceneManager.LoadScene("Level");
    }
}
