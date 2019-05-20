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
        Vector3 startPosition = new Vector3(-400, -50, 0);
        foreach (Player player in GameMetaData.GetInstance().GetPlayers)
        {
            next = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerInfo", typeof(GameObject)), stratPositionInfo);
            next.transform.localPosition = startPosition;
            next.GetComponent<PlayerInfo>().ShowInfoPlayer(player.GetName);
            startPosition += new Vector3(800,0,0);
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("StartGame");
    }    
    
}
