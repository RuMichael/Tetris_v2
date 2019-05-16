using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartGame : MonoBehaviour
{
    public Text hud_Header;

    bool playSolo = true;
    void Start()
    {
        hud_Header.text =" TETRIS ";
    }
    public void PlayTogether()
    {
        Player.AddPlayer("Player1", new Dictionary<Player.comand, KeyCode> {
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
            {Player.comand.speedDownOther, KeyCode.None},
        });
        playSolo = false;
        PlaySolo();
    }
    public void PlaySolo()
    {
        if (playSolo)
            Player.AddPlayer("Player2", new Dictionary<Player.comand, KeyCode> {
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
                {Player.comand.speedDownOther, KeyCode.KeypadMinus},
            });
        else
            Player.AddPlayer("Player2", new Dictionary<Player.comand, KeyCode> {
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
                {Player.comand.speedDownOther, KeyCode.None},
            });
        
        SceneManager.LoadScene("Level");
    }
}
