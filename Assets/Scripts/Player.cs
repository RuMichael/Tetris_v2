using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum comand
    {
        left,right,turn,down,speedUp,speedDown
    }
    public static Stack<Player> Playerss = new Stack<Player>();
    string name;
    Dictionary<comand,KeyCode> control = new Dictionary<comand,KeyCode>();
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
        if(Playerss == null)
            Playerss = new Stack<Player>();
        else
            Playerss.Clear();
    }
    public void AddPlayer(string name, Dictionary<comand,KeyCode> control)
    {
        this.name = name;
        this.control = control;
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
