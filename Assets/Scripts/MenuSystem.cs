using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    public Transform stratPositionInfo, winPositionPl1, winPositionPl2;
    void Start()
    {
        GameObject next;
        Vector3 startPosition = new Vector3(-390, -50, 0);
        Player winner = null;
        foreach (Player player in GameMetaData.GetInstance().GetPlayers)
        {
            next = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerInfo", typeof(GameObject)), stratPositionInfo);
            next.transform.localPosition = startPosition;
            next.GetComponent<PlayerInfo>().ShowInfoPlayer(player.GetName);
            startPosition += new Vector3(780,0,0);

            if (winner == null)
                winner = player;
            else if (winner.Score < player.Score)
                winner = player;
        }
        GameObject winTablo;
        if (winner.GetName == "Player1"){
            winTablo = (GameObject)Instantiate(Resources.Load("Prefabs/Win", typeof(GameObject)), winPositionPl1);
            winTablo.transform.localPosition += new Vector3(3,0,0);
        }
        else{
            winTablo = (GameObject)Instantiate(Resources.Load("Prefabs/Win", typeof(GameObject)), winPositionPl2);
            winTablo.transform.localPosition -= new Vector3(3,0,0);
        }
    }
    public void PlayAgain()
    {
        GameMetaData.GetInstance().Clear();
        SceneManager.LoadScene("StartGame");
    }    
    
}
