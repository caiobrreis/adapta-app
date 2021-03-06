using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathOperations : MonoBehaviour
{
    private Common common;
    [SerializeField] private TextMeshProUGUI questionField;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private TextMeshProUGUI feedbackText;
    private Button correctBtn;

    private int symbolIdx;
    private int n1;
    private int n2;
    private int result;

    private byte round = 1;
    private bool waiting = false;

    private static string[] symbols = new string[] { "+", "-", "*" };

    Stopwatch stopwatch = new Stopwatch();

    // Invoked when script gets enabled. Reset stats and starts a new round
    void Start()
    {
        common = Common.common;
        common.correctJgMat = 0;
        common.incorrectJgMat = 0;
        common.playedJgMat = true;
        NewRound();
    }

    // Creates a random math operation
    void NewRound()
    {
        waiting = false;
        stopwatch.Start();
        
        symbolIdx = Random.Range(0, symbols.Length);
        switch (symbolIdx) {
            case 0:
                n1 = Random.Range(1, 51);
                n2 = Random.Range(1, 51);
                result = n1 + n2;
                break;
            case 1:
                n1 = Random.Range(1, 100);
                n2 = Random.Range(1, n1);
                result = n1 - n2;
                break;
            case 2:
                n1 = Random.Range(1, 21);
                n2 = Random.Range(1, 21);
                result = n1 * n2;
                break;
            default:
                n1 = 0;
                n2 = 0;
                result = 0;
                break;
        }

        questionField.text = $"{n1.ToString()} {symbols[symbolIdx]} {n2.ToString()} = ?";

        ManageChoices();
    }

    // Gets called on button click
    void Guessed(Button btn)
    {
        if (waiting) return;
        waiting = true;
        stopwatch.Stop();
        
        if (btn == correctBtn)
            common.correctJgMat++;
        else
            common.incorrectJgMat++;
        
        common.OutcomeFeedback(btn == correctBtn, btn, feedbackText);

        // Loads next round or next game if this game is over
        if (++round <= common.roundsPerGame) {
            Invoke("NewRound", common.roundWaitTime);
        } else {
            common.Invoke("NextGame", common.roundWaitTime);
            common.timeJgMat = stopwatch.ElapsedMilliseconds / 1000.0;
        }
    }

    // Sets button text and adds Guessed as a on button click listener
    void ButtonOperations(Button btn, TextMeshProUGUI txt, List<int> choices, int val)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Guessed(btn));

        txt.text = val.ToString();
        choices.Add(val);
    }

    void ManageChoices()
    {
        List<int> choices = new List<int>(6);

        // Randomly chooses a button to be the correct one
        int correctChoice = Random.Range(0, buttonsParent.childCount);
        correctBtn = buttonsParent.GetChild(correctChoice).GetComponent<Button>();
        TextMeshProUGUI correctTxt = correctBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ButtonOperations(correctBtn, correctTxt, choices, result);

        // Chooses a random value and do operations on all incorrect buttons
        foreach (Transform child in buttonsParent) {
            if (child != correctBtn.transform) {
                Button btn = child.GetComponent<Button>();
                TextMeshProUGUI txt = child.GetChild(0).GetComponent<TextMeshProUGUI>();

                int value;
                do {
                    switch (symbolIdx) {
                        case 0:
                            value = Random.Range(1, 100);
                            break;
                        case 1:
                            value = Random.Range(1, 100);
                            break;
                        case 2:
                            value = Random.Range(1, 400);
                            break;
                        default:
                            value = Random.Range(1, 400);
                            break;
                    }
                }
                while (choices.Contains(value));

                ButtonOperations(btn, txt, choices, value);
            }
        }
    }
}
