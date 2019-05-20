using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public enum comand
    {
        left, right, turn, down, speedUp, speedDown, leftOther, rightOther, turnOther, downOther,  speedUpOther, speedDownOther
    }

    public Player(string name, Dictionary<comand,KeyCode> control)
    {
        this.Name = name;
        this.control = control;
    }

    string Name;
    Dictionary<comand,KeyCode> control;
    int score;
    int rows;
    int difficult;
        
    public Dictionary<comand,KeyCode> GetControl
    {
        get{
            return control;
        }
    }    
    public string GetName
    {
        get{
            return Name;
        }
    }

    public int Score
    {
        get{return score;}
        set{score = value;}
    }
    public int Rows
    {
        get{return rows;}
        set{rows = value;}
    }
    public int Difficulty
    {
        get{return difficult;}
        set{difficult = value;}
    }

}
