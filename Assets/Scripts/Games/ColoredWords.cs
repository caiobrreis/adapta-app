using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColoredWords : MonoBehaviour
{
    private Common common;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private TextMeshProUGUI feedbackText;
    private Button correctBtn;
    
    private int generatedIdx;
    private int prevGeneratedIdx = -1;

    private byte round = 1;
    private bool waiting = false;

    public string[] words = new string[] {
        "Vermelho", "Laranja", "Preto", "Verde",
        "Azul", "Rosa", "Roxo", "Marrom"
    };

    public Color32[] colors = new Color32[] {
        new Color32(255,0,0,255), new Color32(255,136,0,255), new Color32(0,0,0,255),
        new Color32(0,255,33,255), new Color32(0,107,255,255), new Color32(255,0,220,255),
        new Color32(87,0,127,255), new Color32(100,70,0,255)
    };
    private List<string> wordsCopy;
    private List<Color32> colorsCopy;

    Stopwatch stopwatch = new Stopwatch();

    // Invoked when script gets enabled. Reset stats and starts a new round
    void Start()
    {
        common = Common.common;
        common.correctJgCores = 0;
        common.incorrectJgCores = 0;
        common.playedJgCores = true;
        NewRound();
    }

    // Randomly chooses a index different than the previous one to be the correct word/color
    void NewRound()
    {
        waiting = false;
        stopwatch.Start();

        do { generatedIdx = Random.Range(0, colors.Length); } 
        while (generatedIdx == prevGeneratedIdx);

        prevGeneratedIdx = generatedIdx;

        ManageChoices();
    }

    // Gets called on button click
    void Guessed(Button btn)
    {
        if (waiting) return;
        waiting = true;
        stopwatch.Stop();
        
        if (btn == correctBtn)
            common.correctJgCores++;
        else
            common.incorrectJgCores++;
        
        common.OutcomeFeedback(btn == correctBtn, btn, feedbackText);

        // Loads next round or next game if this game is over
        if (++round <= common.roundsPerGame) {
            Invoke("NewRound", common.roundWaitTime);
        } else {
            common.Invoke("NextGame", common.roundWaitTime);
            common.timeJgCores = stopwatch.ElapsedMilliseconds / 1000.0;
        }
    }

    // Shuffle copies of words and colors list equally
    void ShuffleListsEqually()
    {
        for (int i = 0; i < wordsCopy.Count; i++) {
            int randomIndex = Random.Range(i, wordsCopy.Count);
            
            string temp = wordsCopy[i];
            wordsCopy[i] = wordsCopy[randomIndex];
            wordsCopy[randomIndex] = temp;

            Color32 tempC = colorsCopy[i];
            colorsCopy[i] = colorsCopy[randomIndex];
            colorsCopy[randomIndex] = tempC;
        }
    }

    // Sets buttons text and color and adds Guessed as a on button click listener
    void ButtonOperations(Button btn, TextMeshProUGUI txt, int idx, bool correct)
    {
        // if this is the correct btn, word and color index will be the same, therefore the will match
        int colorIdx = correct ? idx : idx - 1;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Guessed(btn));

        txt.text = wordsCopy[idx];
        txt.color = colorsCopy[colorIdx];

        wordsCopy.RemoveAt(idx);
        colorsCopy.RemoveAt(colorIdx);
    }

    void ManageChoices()
    {
        wordsCopy = words.ToList();
        colorsCopy = colors.ToList();

        // Randomly chooses a button to be the correct one
        int correctChoice = Random.Range(0, buttonsParent.childCount);
        correctBtn = buttonsParent.GetChild(correctChoice).GetComponent<Button>();
        TextMeshProUGUI correctTxt = correctBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ButtonOperations(correctBtn, correctTxt, generatedIdx, true);

        ShuffleListsEqually();

        // Do operations on all incorrect buttons
        int count = wordsCopy.Count - 1;
        foreach (Transform child in buttonsParent) {
            if (child != correctBtn.transform) {
                Button btn = child.GetComponent<Button>();
                TextMeshProUGUI txt = child.GetChild(0).GetComponent<TextMeshProUGUI>();

                ButtonOperations(btn, txt, count, false);

                count--;
            }
        }
    }
}
