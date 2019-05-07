using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void PlayAgain()
    {
        Player.ClearList();
        SceneManager.LoadScene("StartGame");
    }    
    
}
