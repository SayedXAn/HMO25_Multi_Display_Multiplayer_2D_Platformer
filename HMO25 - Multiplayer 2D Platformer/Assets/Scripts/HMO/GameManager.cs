using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string url = "https://rfid-scan.mern.singularitybd.net/users/set-point";
    private const string token = "9b1de5f407f1463e7b2a921bbce364";
    public TMP_Text statusText;
    private int gameID = 4;

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
        AS.clip = sfx[2];
        AS.Play();
        gameOn = false;
        scores[id - 1] = scores[id - 1] + 100;
        for(int i = 0; i < gameWinPanel.Length; i++)
        {
            gameWinPanel[i].gameObject.SetActive(true);
            gameWinText[i].text = "Player " + id + " wins";
        }
        SendScore();
    }

    public void SendScore()
    {
        for(int i = 0; i < rfids.Length; i++)
        {
            if(rfids[i].Length == 10)
            {
                StartCoroutine(PostScore((rfids[i]), gameID, scores[i]));
            }
        }
    }

    IEnumerator PostScore(string rfid, int gID, int score)
    {
        // Build JSON manually
        string jsonBody = "{";
        jsonBody += "\"RFID\":\"" + rfid + "\",";

        if (gID == 0) jsonBody += "\"game1\":" + score;
        if (gID == 1) jsonBody += "\"game2\":" + score;
        if (gID == 2) jsonBody += "\"game3\":" + score;
        if (gID == 3) jsonBody += "\"game4\":" + score;
        if (gID == 4) jsonBody += "\"game5\":" + score;
        if (gID == 5) jsonBody += "\"game6\":" + score;

        jsonBody += "}";

        Debug.Log("Sending: " + jsonBody);
        statusText.text = "Posting score...";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-token", token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST Success: " + request.downloadHandler.text);
            statusText.text = "Score updated successfully!";
            statusText.color = Color.green;
        }
        else
        {
            Debug.LogError("POST Failed: " + request.error + "\nResponse: " + request.downloadHandler.text);

            statusText.text = "Failed to update score!";
            statusText.color = Color.red;
        }

        // Optional: fade out after 3 seconds
        yield return new WaitForSeconds(3);
        statusText.text = "";
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
