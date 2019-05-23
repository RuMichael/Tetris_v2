using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    public RectTransform stratPositionInfo;

    
    void Start()
    {
        List<Player> tmpPlayers = GameMetaData.GetInstance().GetPlayers;
        GameObject next;
        Player winner = null;
        PlayerInfo winnerInfo = null;
        int count = 0 ;

        Vector3 addPosition = new Vector3(stratPositionInfo.rect.width / (tmpPlayers.Count + 1), 0, 0);
        Vector3 startPosition = new Vector3(addPosition.x - (stratPositionInfo.rect.width / 2), 0, 0);
        foreach (Player player in tmpPlayers)
        {            
            next = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerInfo", typeof(GameObject)), stratPositionInfo);
            next.transform.localPosition = startPosition;
            startPosition += addPosition;

            PlayerInfo tmpPlInf = next.GetComponent<PlayerInfo>();
            tmpPlInf.ShowInfoPlayer(player, count);

            if (winner == null || winner.Score < player.Score || winner.Timer < player.Timer)
            {
                winner = player;
                winnerInfo = tmpPlInf;
            }
            count++;
        }
        winnerInfo.ShowWin();

    }
    public void PlayAgain()
    {
        GameMetaData.GetInstance().Clear();
        SceneManager.LoadScene("StartGame");
    }    
    
}
