﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    string name;
    static bool soloGame;
    public static bool SoloGame
    {
        get{return soloGame;}
        set{soloGame = value;}
    }
}