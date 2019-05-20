using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Text hubName, hubScore, hubRows, hubDiff;

    void Start()
    {
        hubName = null;
        hubScore = null;
        hubRows = null;
        hubDiff = null;    
    }
    public void ShowInfoPlayer(string name)
    {
        Player player = GameMetaData.GetInstance().GetPlayer(name);
        hubName.text = player.GetName;
        hubScore.text = "Score: " + player.Score.ToString();
        hubRows.text = "Rows: " + player.Rows.ToString();
        hubDiff.text = "Difficulty: " + player.Difficulty.ToString();
    }
    
}
