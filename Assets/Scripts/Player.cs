using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum comand
    {
        left,right,turn,down,speedUp,speedDown
    }
    //public static List<Player> Players = new List<Player>();
    public static Stack<Player> Playerss = new Stack<Player>();
    string name;
    Dictionary<comand,KeyCode> control = new Dictionary<comand,KeyCode>();
    public static int GetCount
    {
        get{
            return (Playerss != null) ? Playerss.Count : 0;
        }
    }
    /* public static Player GetPlayer()
    {        
        if (Players == null || GetCount == 0)
            return null;
        Player tmp = Players[GetCount - 1];
        List<Player> tmpPlayers = new List<Player>();
        for (int i=0;i<GetCount-1;i++)
            tmpPlayers.Add(Players[i]);
        if (GetCount == 1)
            Players=null;
        else
        {
            Players.Clear();
            Players = tmpPlayers;
        }
        return tmp;
    }*/
    public static Player GetPlayerr()
    {
        return Playerss.Pop();
    }

    public static void ClearList()
    {
        if(Playerss == null)
            Playerss = new Stack<Player>();
        else
            Playerss.Clear();
    }
    public void AddPlayer(string name, Dictionary<comand,KeyCode> control)
    {
        this.name = name;
        this.control = control;
        //Players.Add(this);
        Playerss.Push(new Player{name = name, control = control});
    }
    public string GetName
    {
        get{
            return name;
        }
    }
    public Dictionary<comand,KeyCode> GetControl
    {
        get{
            return control;
        }
    }
}
