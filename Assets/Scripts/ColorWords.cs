using System;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class ColorWords : MonoBehaviour
{
    [SerializeField] private GameObject greenOutline;
    [SerializeField] private GameObject redOutline;

    [SerializeField] private GameObject gameCanvasObj;
    [SerializeField] private GameObject resultCanvasObj;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    
    [SerializeField] private TextMeshProUGUI roundCountText;
    [SerializeField] private TextMeshProUGUI generatedWordField;
    [SerializeField] private TMP_InputField inputField;
    
    private int generatedWord;
    private int generatedColor;
    private int prevGeneratedWord = -1;
    private int prevGeneratedColor = -1;

    private float answerFeedbackDuration = 0.5f;
    private byte round = 1;
    private byte totalRounds = 5;

    private byte accuracy = 0;
    private float speed = 0;
    private float worstSpeed = 20000f;

    public static string[] words = new string[] {
        "Vermelho", "Laranja", "Amarelo", "Verde",
        "Azul", "Rosa", "Roxo", "Marrom"
        };

    public static Color32[] colors = new Color32[] {
        new Color32(255,0,0,255), new Color32(255,136,0,255), new Color32(255,216,0,255),
        new Color32(0,255,33,255), new Color32(0,107,255,255), new Color32(255,0,220,255),
        new Color32(87,0,127,255), new Color32(100,70,0,255)
        };

    Stopwatch stopwatch = new Stopwatch();

    void Start()
    {
        stopwatch.Start();
        NewRound();
    }

    void Update()
    {
        if (round == totalRounds + 1) return;

        KeepInputOpen();

        if (Input.GetKeyDown(KeyCode.Return) && inputField.text != "") {
            round++;

            if (inputField.text.Equals(words[generatedColor], StringComparison.CurrentCultureIgnoreCase)) {
                greenOutline.SetActive(true);
                accuracy++;
            } else {
                redOutline.SetActive(true);
            }
            
            Invoke("DisableOutline", answerFeedbackDuration);
            
            if (round == totalRounds + 1) {
                Invoke("EndGame", answerFeedbackDuration);
                stopwatch.Stop();
                CalculateSpeed();
            } else {
                NewRound();
            }
        }
    }

    void NewRound()
    {
        roundCountText.text = "round " + round.ToString() + "/" + totalRounds.ToString();
        
        inputField.text = "";

        do { generatedWord = UnityEngine.Random.Range(0, words.Length); }
        while (generatedWord == prevGeneratedWord);

        do { generatedColor = UnityEngine.Random.Range(0, colors.Length); } 
        while (generatedColor == generatedWord || generatedColor == prevGeneratedColor);

        generatedWordField.text = words[generatedWord];
        generatedWordField.color = colors[generatedColor];

        prevGeneratedWord = generatedWord;
        prevGeneratedColor = generatedColor;
    }

    void KeepInputOpen()
    {
        inputField.ActivateInputField();
        inputField.Select();
    }

    void DisableOutline()
    {
        greenOutline.SetActive(false);
        redOutline.SetActive(false);
    }

    void CalculateSpeed()
    {
        float t = Mathf.Clamp01(stopwatch.ElapsedMilliseconds / (worstSpeed * totalRounds));
        speed = Mathf.Lerp(5f, 0f, t);
    }

    void EndGame()
    {
        gameCanvasObj.SetActive(false);
        resultCanvasObj.SetActive(true);

        accuracyText.text = accuracy.ToString();
        speedText.text = speed.ToString("F2");
        totalScoreText.text = (accuracy + speed).ToString("F2");
    }
}
