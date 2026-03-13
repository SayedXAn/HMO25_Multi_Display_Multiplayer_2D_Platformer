using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAssigner : MonoBehaviour
{
    //public Canvas[] canvas;
    public PlayerController[] playerCons;
    public Image[] playerModels;
    public Color[] playerColors;
    void Start()
    {
        //for(int i = 0; i < 4; i++)
        //{
        //    canvas[i].targetDisplay = (int)playerCons[i].id - 1;
            
        //}
        for (int i = 0; i < 4; i++)
        {
            playerModels[i].color = playerColors[(int)playerCons[i].id - 1];
            Debug.Log("(int)playerCons[i].id - 1 " + (playerCons[i].id - 1) + " color is: " + playerModels[i].color.ToString());
        }
    }
   
}
