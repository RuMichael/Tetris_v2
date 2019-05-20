using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    public Transform stratPositionInfo;

    
    void Start()
    {
        GameObject next;
        Vector3 startPosition = new Vector3(-390, 0, 0);
        Player winner = null;
        GameObject winnerr=null;
        foreach (Player player in GameMetaData.GetInstance().GetPlayers)
        {
            next = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerInfo", typeof(GameObject)), stratPositionInfo);
            next.transform.localPosition = startPosition;
            next.GetComponent<PlayerInfo>().ShowInfoPlayer(player.GetName);
            startPosition += new Vector3(780,0,0);

            if (winner == null )
            {
                winner = player;
                winnerr = next;
            }
            else if (winner.Score < player.Score)
            {
                winner = player;
                winnerr = next;
            }
            else if (winner.Timer<player.Timer)
            {
                winner = player;
                winnerr = next;
            }
        }
        winnerr.GetComponent<PlayerInfo>().ShowWin();

    }
    public void PlayAgain()
    {
        GameMetaData.GetInstance().Clear();
        SceneManager.LoadScene("StartGame");
    }    
    
}
