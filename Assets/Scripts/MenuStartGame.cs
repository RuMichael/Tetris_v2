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
        Player pl = new Player();
        pl.SoloGame = false;
        SceneManager.LoadScene("Level");
    }
    public void PlaySolo()
    {
        Player pl = new Player();
        pl.SoloGame = true;
        SceneManager.LoadScene("Level");
    }
}
