using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// This class can be accessed from anywhere and will always have one instance
// It is mainly used for methods present common to multiple scripts
public class Common : MonoBehaviour
{
    public static Common common;

    [HideInInspector] public string email;

    [HideInInspector] public List<GameObject> framesHistory = new List<GameObject>();
    [HideInInspector] public List<byte> gameScenesIndexes = new List<byte>();
    [HideInInspector] public List<byte> gameScenesIndexesCopy = new List<byte>();

    [HideInInspector] public bool playedJgCores;
    [HideInInspector] public int correctJgCores;
    [HideInInspector] public int incorrectJgCores;
    [HideInInspector] public double timeJgCores;

    [HideInInspector] public bool playedJgMat;
    [HideInInspector] public int correctJgMat;
    [HideInInspector] public int incorrectJgMat;
    [HideInInspector] public double timeJgMat;

    [HideInInspector] public bool playedJgSilh;
    [HideInInspector] public int correctJgSilh;
    [HideInInspector] public int incorrectJgSilh;
    [HideInInspector] public double timeJgSilh;

    public float roundWaitTime = 1f;
    [HideInInspector] public int roundsPerGame = 5;

    private Color32 correctColor = new Color32(4, 196, 0, 255);
    private Color32 incorrectColor = new Color32(249, 74, 74, 255);

    void Awake()
    {
        // Ensure it has only one instance
        if (common == null) {
            DontDestroyOnLoad(gameObject);
            common = this;
        } else if (common != this) {
            Destroy(gameObject);
        }

        // Fix for a Unity bug
        #if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            Screen.SetResolution(375, 812, false);
        #endif

        SetStartingGames();
    }

    // Starts the application with all 3 games selected
    private void SetStartingGames()
    {
        gameScenesIndexes.Add(2);
        gameScenesIndexes.Add(3);
        gameScenesIndexes.Add(4);
    }

    // Visual feedback for when the player makes a guess
    public void OutcomeFeedback(bool correct, Button btn, TextMeshProUGUI feedbackTxt)
    {
        Image img = btn.GetComponent<Image>();

        if (correct) {
            img.color = correctColor;
            feedbackTxt.text = "Boa, acertou";
        } else {
            img.color = incorrectColor;
            feedbackTxt.text = "N??o foi dessa vez";
        }

        TextMeshProUGUI txt = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txt.color = Color.white;

        StartCoroutine(ResetFeedback(img, txt, feedbackTxt));
    }

    // Restore normal state after giving the feedback
    private IEnumerator ResetFeedback(Image img, TextMeshProUGUI txt, TextMeshProUGUI feedbackTxt)
    {
        yield return new WaitForSeconds(roundWaitTime);

        if (img == null) yield break;
        img.color = Color.white;
        feedbackTxt.text = "";

        if(SceneManager.GetActiveScene().name != "JogoDasCores")
            txt.color = new Color32(50, 50, 50, 255);
    }

    public void NextGame()
    {
        // If there are no more games to be played, write player score in the screen
        // and save it to the database
        if (gameScenesIndexesCopy.Count == 0) {
            if (SceneManager.GetActiveScene().name != "MainMenu") {
                SceneManager.LoadScene("MainMenu");
                StartCoroutine(ManageEndGameScreen());
                StartCoroutine(SavePlayerData(1, correctJgCores, incorrectJgCores, timeJgCores));
                StartCoroutine(SavePlayerData(2, correctJgMat, incorrectJgMat, timeJgMat));
                StartCoroutine(SavePlayerData(3, correctJgSilh, incorrectJgSilh, timeJgSilh));
            }
            return;
        }

        // In case there are more games to be played, load the next one
        int randGame = Random.Range(0, gameScenesIndexesCopy.Count);
        SceneManager.LoadScene(gameScenesIndexesCopy[randGame]);
        gameScenesIndexesCopy.RemoveAt(randGame);
    }

    // Load post game screen and get stats parent object
    private IEnumerator ManageEndGameScreen()
    {
        while (SceneManager.GetActiveScene().name != "MainMenu")
            yield return null;

        Transform frameFim = GameObject.Find("Canvas").transform.Find("Frame Fim");
        frameFim.gameObject.SetActive(true);
        Transform fields = frameFim.Find("Fields");

        WriteGameStats(fields, 0, correctJgCores, incorrectJgCores);
        WriteGameStats(fields, 1, correctJgMat, incorrectJgMat);
        WriteGameStats(fields, 2, correctJgSilh, incorrectJgSilh);
    }

    // Write a game stats to the player screen
    private void WriteGameStats(Transform fields, int n, int correct, int incorrect)
    {
        TextMeshProUGUI scoreTxt = fields.GetChild(n).Find("Score").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI percentTxt = fields.GetChild(n).Find("Percent").GetComponent<TextMeshProUGUI>();
        RectTransform barFill = fields.GetChild(n).Find("BarFill").GetComponent<RectTransform>();
        float roundsPlayed = correct + incorrect;
        float barFillAmount = correct / roundsPlayed;

        scoreTxt.text = $"{correct} / {roundsPlayed}";
        percentTxt.text = $"{barFillAmount:P0}";
        barFill.sizeDelta = new Vector2(858 * barFillAmount, barFill.sizeDelta.y);

        if (barFillAmount < 0.57) 
            percentTxt.color = Color.black;
        else 
            percentTxt.color = Color.white;
    }

    // Send game stats to savedata.php to store and update player stats
    private IEnumerator SavePlayerData(int id, int correct, int incorrect, double time)
    {
        double x = System.Math.Truncate(time * 100) / 100;
        string stime = string.Format("{0:F2}", x).Replace(',', '.');

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("j_id", id);
        form.AddField("vezes_jogadas", correct + incorrect);
        form.AddField("acertos", correct);
        form.AddField("erros", incorrect);
        form.AddField("tempo", stime);

        WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        yield return www;
        
        if (www.text == "0")
            Debug.Log("Data saved successfully.");
        else
            Debug.Log("Data save failed. Error " + www.text);
    }
}
