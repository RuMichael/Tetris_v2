using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static List<Player> Players = new List<Player>();
    string name;
    Dictionary<byte,KeyCode> control = new Dictionary<byte,KeyCode>(4);
    public static int GetCount
    {
        get{
            return Players.Count;
        }
    }
    public static void DeletePlayer(string name)
    {
        if (Player.GetCount == 0)
            return;
        Player tmp = null;
        foreach(var item in Players)
            if(item.GetName == name)
                tmp = item;
        if(tmp != null)
            Players.Remove(tmp);
    }
    public static Player GetPlayer()
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
    }

    public static void ClearList()
    {
        if(Players == null)
            Players = new List<Player>();
        else
            Players.Clear();
    }
    public Player(string name, Dictionary<byte,KeyCode> control)
    {
        this.name = name;
        this.control = control;
        Players.Add(this);
    }
    public string GetName
    {
        get{
            return name;
        }
    }
    public Dictionary<byte,KeyCode> GetControl
    {
        get{
            return control;
        }
    }
}
