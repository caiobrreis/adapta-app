using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathOperations : MonoBehaviour
{
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private TextMeshProUGUI roundCountField;
    [SerializeField] private TextMeshProUGUI fullOperationField;
    [SerializeField] private Transform buttonsParent;

    private int symbolIdx;
    private int n1;
    private int n2;
    private int result;

    private byte round = 1;
    private byte totalRounds = 5;

    private int correctGuesses = 0;
    private float worstSpeedPerRound = 20000f;

    private static string[] symbols = new string[] { "+", "-", "*" };

    Stopwatch stopwatch = new Stopwatch();

    void Start()
    {
        stopwatch.Start();
        NewRound();
    }

    void NewRound()
    {
        roundCountField.text = $"round {round.ToString()}/{totalRounds.ToString()}";

        symbolIdx = Random.Range(0, symbols.Length);
        switch (symbolIdx) {
            case 0:
                n1 = Random.Range(1, 99);
                n2 = Random.Range(1, 99);
                break;
            case 1:
                n1 = Random.Range(1, 99);
                n2 = Random.Range(1, n1);
                break;
            case 2:
                n1 = Random.Range(1, 31);
                n2 = Random.Range(1, 31);
                break;
            default:
                n1 = Random.Range(1, 99);
                n2 = Random.Range(1, n1);
                break;
        }

        fullOperationField.text = $"{n1.ToString()} {symbols[symbolIdx]} {n2.ToString()} =";

        result = GetResult(symbolIdx);

        ManageChoices();
    }

    int GetResult(int symbolIdx)
    {
        switch (symbolIdx) {
            case 0:
                return n1 + n2;
            case 1:
                return n1 - n2;
            case 2:
                return n1 * n2;
            default:
                return 0;
        }
    }

    void Guessed(bool correct)
    {
        if (round >= totalRounds + 1) return;
        
        if (correct) {
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

    void ManageChoices()
    {
        int correctChoice = Random.Range(0, buttonsParent.childCount);
        
        Button correctBtn = buttonsParent.GetChild(correctChoice).GetComponent<Button>();
        correctBtn.onClick.RemoveAllListeners();
        correctBtn.onClick.AddListener(() => Guessed(true));
        correctBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = result.ToString();

        List<int> choices = new List<int>(6);
        choices.Add(result);

        foreach (Transform child in buttonsParent) {
            if (child != correctBtn.transform) {
                Button btn = child.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => Guessed(false));

                int value;
                do {
                    switch (symbolIdx) {
                        case 0:
                            value = Random.Range(1, 199);
                            break;
                        case 1:
                            value = Random.Range(1, 100);
                            break;
                        case 2:
                            value = Random.Range(1, 900);
                            break;
                        default:
                            value = Random.Range(1, 900);
                            break;
                    }
                }
                while ( choices.Contains(value) );

                choices.Add(value);
                child.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
            }
        }
    }
}
