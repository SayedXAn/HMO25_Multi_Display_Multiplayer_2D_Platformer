using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] gameWinPanel;
    public TMP_Text[] gameWinText;
    public int[] scores = { 0, 0, 0, 0};
    public TMP_Text[] scoreTexts;
    public bool gameOn = true;
    public AudioSource AS;
    public AudioClip[] sfx; //0-point 1-death 2-win
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
            SceneManager.LoadScene("HMO_Updated");
        }
    }

    public void GameWin(uint id)
    {
        gameOn = false;
        for(int i = 0; i < gameWinPanel.Length; i++)
        {
            gameWinPanel[i].gameObject.SetActive(true);
            gameWinText[i].text = "Player " + id + " wins";
        }
        
    }

    public void OrbHitScoreCount(uint id)
    {
        scores[id - 1] += 10;
        scoreTexts[id - 1].text = "Score: " + scores[id-1].ToString();
        AS.clip = sfx[0];
    }
}
