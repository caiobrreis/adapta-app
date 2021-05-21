using System.Diagnostics;
using UnityEngine;
using TMPro;

public class MathOperations : MonoBehaviour
{
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private TextMeshProUGUI roundCountField;
    [SerializeField] private TextMeshProUGUI fullOperationField;
    [SerializeField] private TMP_InputField inputField;

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

    void Update()
    {
        if (round == totalRounds + 1) return;

        Common.common.KeepInputOpen(inputField);

        if (Input.GetKeyDown(KeyCode.Return) && inputField.text != "") {
            if (inputField.text.Equals(result.ToString())) {
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

        symbolIdx = Random.Range(0, symbols.Length);
        n1 = Random.Range(1, 99);
        n2 = symbolIdx == 1 ? Random.Range(1, n1) : Random.Range(1, 99);

        fullOperationField.text = $"{n1.ToString()} {symbols[symbolIdx]} {n2.ToString()} =";

        result = GetResult(symbolIdx);
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
}