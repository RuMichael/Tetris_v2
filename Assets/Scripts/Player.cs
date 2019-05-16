using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public enum comand
    {
        left, right, turn, down, speedUp, speedDown, leftOther, rightOther, turnOther, downOther,  speedUpOther, speedDownOther
    }

    #region #static

    
    public static Stack<Player> Playerss = new Stack<Player>();
    public static int GetCount
    {
        get{
            return (Playerss != null) ? Playerss.Count : 0;
        }
    }
    public static Player GetPlayerr()
    {
        return Playerss.Pop();
    }
    public static void ClearList()
    {
        if (Playerss == null)
            Playerss = new Stack<Player>();
        else
            Playerss.Clear();
    }
    public static void AddPlayer(string name, Dictionary<comand,KeyCode> control)
    {
        Playerss.Push(new Player{Name = name, control = control});
    }

    public static void AddPlayer(Player player)
    {
        bool check = true;
        foreach (Player item in Playerss)
        {
            if (item.GetName == player.GetName)
            {    
                check = false;
                item.Score = player.Score;
                item.Rows = player.Rows;
                item.Difficulty = player.Difficulty;
            }
        }
        if (check)
            Playerss.Push(player);
    }
    #endregion

    #region class player

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

    #endregion
}
