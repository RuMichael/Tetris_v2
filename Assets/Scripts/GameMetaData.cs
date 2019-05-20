using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMetaData 
{

    private Dictionary<string, Player> m_Players;
    private Stack<Player> stackPlayers;
    private static GameMetaData m_Instance;
    public GameMetaData()
    {
        m_Players = new Dictionary<string, Player>();
    }
    public static GameMetaData GetInstance()
    {
        if(m_Instance == null)
            m_Instance = new GameMetaData ();
        return m_Instance;
    }
    public Player GetPlayer(string playerName)
    {
        if(!m_Players.ContainsKey(playerName)) 
            m_Players.Add(playerName, null);
        return m_Players[playerName];
    }
    public void SetPlayer(Player player)
    {        
        if(m_Players.ContainsKey(player.GetName))
            m_Players[player.GetName] = player;
        else
            m_Players.Add(player.GetName, player);        
    }
    public List<Player> GetPlayers
    {
        get{ 
            List<Player> players = new List<Player>();
            foreach(Player player in m_Players.Values)            
                players.Add(player);            
            return players;
        }
    }

}
