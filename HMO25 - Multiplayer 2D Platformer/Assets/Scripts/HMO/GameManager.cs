using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] canvas;
    public TMP_Text[] gameWinText;
    public bool gameOn = true;
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("HMO");
        }
    }

    public void GameWin(uint id)
    {
        gameOn = false;
        for(int i = 0; i < canvas.Length; i++)
        {
            canvas[i].gameObject.SetActive(true);
            gameWinText[i].text = "Player " + id + " wins";
        }
        
    }
}
