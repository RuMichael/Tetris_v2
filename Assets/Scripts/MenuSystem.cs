using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Level");
        Player player1 = new Player{};
        Player player2 = new Player{};
        DontDestroyOnLoad(player1);
        DontDestroyOnLoad(player2);
    }    
}
