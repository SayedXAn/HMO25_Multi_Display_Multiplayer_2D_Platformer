using System.Collections;
using System.Linq;
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
    private bool gameOn = false;
    public AudioSource AS;
    public AudioClip[] sfx; //0-point 1-death 2-win
    public TMP_InputField[] rfid_inputFields;
    private string[] rfids = { "", "", "", ""};
    public GameObject rfidPanel;
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
        rfidPanel.SetActive(true);
        rfid_inputFields[0].ActivateInputField();
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
        AS.Play();
    }

    public void StartGameButton()
    {
        int playerCount = 0;
        for(int i = 0; i < rfid_inputFields.Length; i++)
        {
            if (rfid_inputFields[i].text.Length == 10)
            {
                rfids[i] = rfid_inputFields[i].text;
                playerCount++;
            }
        }
        if(playerCount > 0)
        {
            gameOn = true;
            rfidPanel.SetActive(false);
        }
        else
        {
            //no player
        }
    }

    public void CheckRFID(int id)
    {
        if (rfid_inputFields[id].text.Length == 10 && id != 3)
        {
            //rfid_inputFields[id+1].ActivateInputField();
            StartCoroutine(ActivateIF(id + 1));
        }
    }
    IEnumerator ActivateIF(int id)
    {
        yield return new WaitForSeconds(0.1f);
        rfid_inputFields[id].ActivateInputField();
    }
    public bool IsGameOn()
    {
        return gameOn;
    }
}
