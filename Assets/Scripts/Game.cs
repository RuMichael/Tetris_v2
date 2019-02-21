using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int GridHeight = 20;
    public static int GridWeight = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIsInsideGrid(Vector2 position)
    {
        return ((int)position.x > 0 && (int)position.y > 0 && (int)position.x <= GridWeight);
    }

    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
