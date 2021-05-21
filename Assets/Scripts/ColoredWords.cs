using System;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class ColoredWords : MonoBehaviour
{
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private TextMeshProUGUI roundCountField;
    [SerializeField] private TextMeshProUGUI generatedWordField;
    [SerializeField] private TMP_InputField inputField;
    
    private int generatedWordIdx;
    private int generatedColorIdx;
    private int prevGeneratedWordIdx = -1;
    private int prevGeneratedColorIdx = -1;

    private byte round = 1;
    private byte totalRounds = 5;

    private int correctGuesses = 0;
    private float worstSpeedPerRound = 20000f;

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

        Common.common.KeepInputOpen(inputField);

        if (Input.GetKeyDown(KeyCode.Return) && inputField.text != "") {
            if (inputField.text.Equals(words[generatedColorIdx], StringComparison.CurrentCultureIgnoreCase)) {
                Common.common.ShowOutline(true, gameCanvas);
                correctGuesses++;
            } else {
                Common.common.ShowOutline(false, gameCanvas);
            }
            
            if (++round <= totalRounds) {
                NewRound();
            } else {
                stopwatch.Stop();
                Common.common.Invoke("ResultScene", Common.common.outlineDuration);
                Common.common.correctGuessesScore = correctGuesses * 100;
                Common.common.CalculateSpeed(stopwatch.ElapsedMilliseconds, worstSpeedPerRound * totalRounds);
                Common.common.WriteToTextFile(stopwatch.ElapsedMilliseconds / 1000);
            }
        }
    }

    void NewRound()
    {
        roundCountField.text = $"round {round.ToString()}/{totalRounds.ToString()}";
        inputField.text = "";

        do { generatedWordIdx = Random.Range(0, words.Length); }
        while (generatedWordIdx == prevGeneratedWordIdx);

        do { generatedColorIdx = Random.Range(0, colors.Length); } 
        while (generatedColorIdx == generatedWordIdx || generatedColorIdx == prevGeneratedColorIdx);

        generatedWordField.text = words[generatedWordIdx];
        generatedWordField.color = colors[generatedColorIdx];

        prevGeneratedWordIdx = generatedWordIdx;
        prevGeneratedColorIdx = generatedColorIdx;
    }
}
