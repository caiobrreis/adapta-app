using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Common : MonoBehaviour
{
    public static Common common;

    [HideInInspector] public List<GameObject> prevFrames = new List<GameObject>();
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

    [HideInInspector] public float roundWaitTime = 0.5f;
    [HideInInspector] public int roundsPerGame = 5;

    private Color32 correctColor = new Color32(100, 255, 100, 255);
    private Color32 incorrectColor = new Color32(255, 100, 100, 255);

    void Awake()
    {
        if (common == null) {
            DontDestroyOnLoad(gameObject);
            common = this;
        } else if (common != this) {
            Destroy(gameObject);
        }

        Screen.SetResolution(375, 812, false);
        SetStartingGames();
    }


    private void SetStartingGames()
    {
        gameScenesIndexes.Add(2);
        gameScenesIndexes.Add(3);
        gameScenesIndexes.Add(4);
    }

    public void ChangeButtonColor(bool correct, Button btn)
    {
        Image img = btn.GetComponent<Image>();
        img.color = correct ? correctColor : incorrectColor;

        TextMeshProUGUI txt = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txt.color = Color.white;

        StartCoroutine(RestoreColor(img, txt));
    }

    private IEnumerator RestoreColor(Image img, TextMeshProUGUI txt)
    {
        yield return new WaitForSeconds(roundWaitTime);

        if (img == null) yield break;
        img.color = Color.white;

        if(SceneManager.GetActiveScene().name != "JogoDasCores")
            txt.color = new Color32(50, 50, 50, 255);
    }

    public void NextGame()
    {
        if (gameScenesIndexesCopy.Count == 0) {
            if (SceneManager.GetActiveScene().name != "MainMenu") {
                SceneManager.LoadScene(1);
                StartCoroutine("WriteEndGameStats");
            }
            return;
        }

        int randGame = Random.Range(0, gameScenesIndexesCopy.Count);
        SceneManager.LoadScene(gameScenesIndexesCopy[randGame]);
        gameScenesIndexesCopy.RemoveAt(randGame);
    }

    private IEnumerator WriteEndGameStats()
    {
        while (SceneManager.GetActiveScene().buildIndex != 1)
            yield return null;

        Transform frameFim = GameObject.Find("Canvas").transform.Find("Frame Fim");
        frameFim.gameObject.SetActive(true);
        Transform fields = frameFim.Find("Fields");

        TextMeshProUGUI scoreTxt = fields.GetChild(0).Find("Score").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI percentTxt = fields.GetChild(0).Find("Percent").GetComponent<TextMeshProUGUI>();
        RectTransform barFill = fields.GetChild(0).Find("BarFill").GetComponent<RectTransform>();
        float roundsPlayed = correctJgCores + incorrectJgCores;
        float barFillAmount = correctJgCores / roundsPlayed;

        scoreTxt.text = $"{correctJgCores} / {roundsPlayed}";
        percentTxt.text = $"{barFillAmount:P0}";
        barFill.sizeDelta = new Vector2(858 * barFillAmount, barFill.sizeDelta.y);
        if (barFillAmount < 0.57) percentTxt.color = Color.black;
        else percentTxt.color = Color.white;

        scoreTxt = fields.GetChild(1).Find("Score").GetComponent<TextMeshProUGUI>();
        percentTxt = fields.GetChild(1).Find("Percent").GetComponent<TextMeshProUGUI>();
        barFill = fields.GetChild(1).Find("BarFill").GetComponent<RectTransform>();
        roundsPlayed = correctJgMat + incorrectJgMat;
        barFillAmount = correctJgMat / roundsPlayed;

        scoreTxt.text = $"{correctJgMat} / {roundsPlayed}";
        percentTxt.text = $"{barFillAmount:P0}";
        barFill.sizeDelta = new Vector2(858 * barFillAmount, barFill.sizeDelta.y);
        if (barFillAmount < 0.57) percentTxt.color = Color.black;
        else percentTxt.color = Color.white;

        scoreTxt = fields.GetChild(2).Find("Score").GetComponent<TextMeshProUGUI>();
        percentTxt = fields.GetChild(2).Find("Percent").GetComponent<TextMeshProUGUI>();
        barFill = fields.GetChild(2).Find("BarFill").GetComponent<RectTransform>();
        roundsPlayed = correctJgSilh + incorrectJgSilh;
        barFillAmount = correctJgSilh / roundsPlayed;

        scoreTxt.text = $"{correctJgSilh} / {roundsPlayed}";
        percentTxt.text = $"{barFillAmount:P0}";
        barFill.sizeDelta = new Vector2(858 * barFillAmount, barFill.sizeDelta.y);
        if (barFillAmount < 0.57) percentTxt.color = Color.black;
        else percentTxt.color = Color.white;
    }
}
