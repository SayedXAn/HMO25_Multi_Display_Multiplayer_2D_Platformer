using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string url = "https://rfid-scan.wskoly.xyz/api/game/score";
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
    public int timerCount = 120;
    public TMP_Text[] timerTexts;
    //public TMP_Text amiDebugText;
    void Start()
    {
        //if (Display.displays.Length > 1)
        //{
        //    // Activate all secondary displays
        //    for (int i = 1; i < Display.displays.Length; i++)
        //    {
        //        Display.displays[i].Activate();
        //    }
        //}
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
        int w = int.Parse(id.ToString());
        Debug.Log("wwww: " + w);
        for(int i = 0; i < scores.Length; i++)
        {
            if(i == w)
            {
                scores[i] = 15;
                scoreTexts[i].text = "Score: 150";
            }
            else
            {
                scores[i] = scores[i]/10;
                if(scores[i] < 2)
                {
                    scores[i] = 2;
                }
            }
            gameWinPanel[i].gameObject.SetActive(true);
            
            gameWinText[i].text = "Player " + (w+1) + " wins";
        }

        //for(int i = 0; i < gameWinPanel.Length; i++)
        //{
        //    gameWinPanel[i].gameObject.SetActive(true);
        //    gameWinText[i].text = "Player " + id + " wins";
        //}

        SendScore();
    }

    public void CheckWhoIsWinnerWhenTimeOver()
    {
        gameOn = false;
        //Debug.Log("Time Seshhhhhhhhhhhhhhhhhhhhhhhh");

        for (int i = 0; i < scores.Length; i++)
        {
            gameWinPanel[i].gameObject.SetActive(true);
            gameWinText[i].text = "Time's up!\nYour score: " + scores[i];
            scores[i] = scores[i]/10;
            if (scores[i] < 2)
            {
                scores[i] = 2;
            }
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
        var gameID = gID + 1;
        // Build JSON manually

        string jsonBody = $"{{\"rfid\": \"{rfid}\", \"scores\": {{\"{gameID}\": {score}}}}}";

        Debug.Log("Sending: " + jsonBody);
        statusText.text = "Posting score...";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("x-token", token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST Success: " + request.downloadHandler.text);
            statusText.text = "Score updated successfully!";
            statusText.color = Color.green;
            // Parse JSON response
            UpdateScoreResponse response = JsonUtility.FromJson<UpdateScoreResponse>(request.downloadHandler.text);
            //scoreText.text = $"Your total score: {response.total_points}";
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
        //Debug.Log("Ami player "+ id + " ami paisi 10");
        //amiDebugText.text = "Ami player " + id + " ami paisi 10";
        scores[id - 1] += 10;
        scoreTexts[id - 1].text = "Score: " + scores[id-1].ToString();
        AS.clip = sfx[0];
        AS.Play();
    }

    public void StartGameButton()
    {
        //int playerCount = 0;
        //for(int i = 0; i < rfid_inputFields.Length; i++)
        //{
        //    if (rfid_inputFields[i].text.Length == 10)
        //    {
        //        rfids[i] = rfid_inputFields[i].text;
        //        playerCount++;
        //    }
        //}
        if(/*playerCount > 0*/ true)
        {
            gameOn = true;
            rfidPanel.SetActive(false);
            StartCountDown();
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
    public void RestartScene()
    {
        SceneManager.LoadScene("HMO_Updated");
    }
    public void StartCountDown()
    {
        StartCoroutine(CountDownTimer());
    }
    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(1f);
        timerCount--;
        foreach(TMP_Text text in timerTexts)
        {
            text.text = timerCount.ToString() + "s";
        }
        if(!gameOn)
        {
            StopCoroutine(CountDownTimer());
            foreach (TMP_Text text in timerTexts)
            {
                text.text = "00s";
            }
        }
        else if(timerCount > 0)
        {
            StartCoroutine(CountDownTimer());
        }
        else if(timerCount == 0)
        {
            CheckWhoIsWinnerWhenTimeOver();
        }
    }
}

[System.Serializable]
public class ScoreResponse
{
    public string rfid;
    public int game_id;
    public int score;
    public string user_name;
    public int total_points;
}

[System.Serializable]
public class UpdateScoreResponse
{
    public string rfid;
    public string user_name;
    public string updated_games;
    public int total_points;
    public string message;
}
