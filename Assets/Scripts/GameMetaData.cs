using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMetaData 
{
    private static GameMetaData m_Instance;
    public static GameMetaData GetInstance()
    {
        if(m_Instance == null)
            m_Instance = new GameMetaData ();
        return m_Instance;
    }

    private List<Player> m_Playerss;
    public GameMetaData()
    {
        m_Playerss = new List<Player>();
    }
    public void SetPlayer(Player player)
    {        
        bool check = false;   
        for(int i = 0; i < m_Playerss.Count; i++)        
            if (m_Playerss[i].GetName == player.GetName)
            {
                m_Playerss[i] = player;
                check = true;
                break;
            }
        if (!check)
            m_Playerss.Add(player);
    }
    public List<Player> GetPlayers
    {
        get{       
            return m_Playerss;
        }
    }
    public void Clear()
    {
        m_Playerss.Clear();
    }
}
