using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpwanScript : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;
    private void Start()
    {
        if (Display.displays.Length > 1)
        {
            // Activate all secondary displays
            for (int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }
        }
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = SpawnPoints[m_playerCount].transform.position;
        if(m_playerCount == 0)
        {
            playerInput.GetComponent<PlayerController>().SwitchOnCamera(); 
        }
        m_playerCount++;
    }
}
