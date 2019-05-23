using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Text hubName, hubScore, hubRows, hubDiff;

    public GameObject[] prefabs = new GameObject[3];

    public GameObject prefabWin;

    public Transform startPosPrefabControl, startPosPrefabWin;


    void Start()
    {
        hubName = null;
        hubScore = null;
        hubRows = null;
        hubDiff = null;    
    }
    public void ShowInfoPlayer(Player player, int count)
    {
        hubName.text = player.GetName;
        hubScore.text = "Score: " + player.Score.ToString();
        hubRows.text = "Rows: " + player.Rows.ToString();
        hubDiff.text = "Difficulty: " + player.Difficulty.ToString();

        GameObject tmp = Instantiate(prefabs[count], startPosPrefabControl);
        
    }

    public void ShowWin()
    {
        GameObject tmp = Instantiate(prefabWin, startPosPrefabWin);
        tmp.transform.localScale = new Vector3(15,15,0);
    }
    
}
