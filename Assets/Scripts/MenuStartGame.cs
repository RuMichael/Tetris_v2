﻿using System.Collections;
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
        Dictionary<byte,KeyCode> control = new Dictionary<byte,KeyCode>(4);
        control.Add(1, KeyCode.UpArrow);
        control.Add(2, KeyCode.LeftArrow);
        control.Add(3, KeyCode.DownArrow);
        control.Add(4, KeyCode.RightArrow);
        new Player("Vasya", control);
        PlaySolo();
    }
    public void PlaySolo()
    {
        Dictionary<byte,KeyCode> control = new Dictionary<byte,KeyCode>(4);
        control.Add(1, KeyCode.W);
        control.Add(2, KeyCode.A);
        control.Add(3, KeyCode.S);
        control.Add(4, KeyCode.D);
        new Player("Misha", control);
        SceneManager.LoadScene("Level");
    }
}
