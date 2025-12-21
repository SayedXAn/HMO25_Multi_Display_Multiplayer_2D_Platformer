using Platformer.Mechanics;
using UnityEngine;

public class CanvasAssigner : MonoBehaviour
{
    public Canvas[] canvas;
    public PlayerController[] playerCons;
    void Start()
    {
        for(int i = 0; i < canvas.Length; i++)
        {
            canvas[i].targetDisplay = (int)playerCons[i].id - 1;
        }
    }
}
