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
    private Button correctBtn;
    
    private int generatedWordIdx;
    private int generatedColorIdx;
    private int prevGeneratedWordIdx = -1;
    private int prevGeneratedColorIdx = -1;

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

    void Start()
    {
        common = Common.common;
        common.correctJgCores = 0;
        common.incorrectJgCores = 0;
        common.playedJgCores = true;
        NewRound();
    }

    void NewRound()
    {
        waiting = false;
        stopwatch.Start();
        
        do { generatedWordIdx = Random.Range(0, words.Length); }
        while (generatedWordIdx == prevGeneratedWordIdx);

        do { generatedColorIdx = Random.Range(0, colors.Length); } 
        while (generatedColorIdx == generatedWordIdx || generatedColorIdx == prevGeneratedColorIdx);

        prevGeneratedWordIdx = generatedWordIdx;
        prevGeneratedColorIdx = generatedColorIdx;

        ManageChoices();
    }

    void Guessed(Button btn)
    {
        if (waiting) return;
        waiting = true;
        stopwatch.Stop();
        
        if (btn == correctBtn)
            common.correctJgCores++;
        else
            common.incorrectJgCores++;
        
        common.ChangeButtonColor(btn == correctBtn, btn);

        if (++round <= common.roundsPerGame) {
            Invoke("NewRound", common.roundWaitTime);
        } else {
            common.Invoke("NextGame", common.roundWaitTime);
            common.timeJgCores = stopwatch.ElapsedMilliseconds / 1000.0;
        }
    }

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

    void ButtonOperations(Button btn, TextMeshProUGUI txt, int pos, bool correct)
    {
        int cPos = correct ? pos : pos - 1;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Guessed(btn));

        txt.text = wordsCopy[pos];
        txt.color = colorsCopy[cPos];

        wordsCopy.RemoveAt(pos);
        colorsCopy.RemoveAt(cPos);
    }

    void ManageChoices()
    {
        wordsCopy = words.ToList();
        colorsCopy = colors.ToList();

        int correctChoice = Random.Range(0, buttonsParent.childCount);
        correctBtn = buttonsParent.GetChild(correctChoice).GetComponent<Button>();
        TextMeshProUGUI correctTxt = correctBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ButtonOperations(correctBtn, correctTxt, generatedColorIdx, true);

        ShuffleListsEqually();

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
