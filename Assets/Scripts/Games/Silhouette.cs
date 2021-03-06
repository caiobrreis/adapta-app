using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Silhouette : MonoBehaviour
{
    private Common common;
    [SerializeField] private Image imageField;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private TextMeshProUGUI feedbackText;
    private Button correctBtn;

    private int generatedImageIdx;

    private byte round = 1;
    private bool waiting = false;

    public Sprite[] images = new Sprite[] { };
    private string[] words = new string[] {
        "garrafa", "abajur", "ventilador", "lápis", "chave", "bicicleta", "caneca", "garfo", "foguete", "martelo", "lua",
        "algema", "ampulheta", "âncora", "balde", "bandeira", "bigode", "bigorna", "botão", "cadeira", "castelo", "coração",
        "coroa", "corrente", "escada", "estrela", "extintor", "hidrante", "leque", "machado", "óculos", "sino", "sofá",
        "telefone", "vela", "aranha", "elefante", "cachorro", "gato", "rato", "girafa", "peixe", "formiga", "borboleta",
        "golfinho", "pinguim", "camelo", "caranguejo", "cobra", "dinossauro", "gorila", "pássaro", "touro", "urso", "vaca",
        "maça", "pera", "banana", "morango", "abacaxi", "cenoura", "pimenta", "amendoim", "cereja", "milho", "sorvete", "uva"
    };
    private List<string> wordsCopy;
    private List<Sprite> imagesCopy;

    Stopwatch stopwatch = new Stopwatch();

    // Invoked when script gets enabled. Reset stats and starts a new round
    void Start()
    {
        common = Common.common;
        common.correctJgSilh = 0;
        common.incorrectJgSilh = 0;
        common.playedJgSilh = true;
        imagesCopy = images.ToList();
        NewRound();
    }

    // Get a new random image
    void NewRound()
    {
        waiting = false;
        stopwatch.Start();

        generatedImageIdx = Random.Range(0, imagesCopy.Count);
        imageField.sprite = imagesCopy[generatedImageIdx];

        ManageChoices();
    }

    // Gets called on button click
    void Guessed(Button btn)
    {
        if (waiting) return;
        waiting = true;
        stopwatch.Stop();
        
        if (btn == correctBtn)
            common.correctJgSilh++;
        else
            common.incorrectJgSilh++;

        common.OutcomeFeedback(btn == correctBtn, btn, feedbackText);

        // Loads next round or next game if this game is over
        if (++round <= common.roundsPerGame) {
            Invoke("NewRound", common.roundWaitTime);
        } else {
            common.Invoke("NextGame", common.roundWaitTime);
            common.timeJgSilh = stopwatch.ElapsedMilliseconds / 1000.0;
        }
    }

    // Adds Guessed as a on button click listener
    void ButtonOperations(Button btn)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Guessed(btn));
    }

    void ManageChoices()
    {
        wordsCopy = words.ToList();

        // Randomly chooses a button to be the correct one
        int correctChoice = Random.Range(0, buttonsParent.childCount);
        correctBtn = buttonsParent.GetChild(correctChoice).GetComponent<Button>();
        TextMeshProUGUI correctTxt = correctBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ButtonOperations(correctBtn);

        // Set a button with the correct answer text
        correctTxt.text = imagesCopy[generatedImageIdx].name;
        wordsCopy.Remove(imagesCopy[generatedImageIdx].name);
        imagesCopy.Remove(imagesCopy[generatedImageIdx]);

        // Chooses a random text to all incorrect buttons
        foreach (Transform child in buttonsParent) {
            if (child != correctBtn.transform) {
                Button btn = child.GetComponent<Button>();
                TextMeshProUGUI txt = child.GetChild(0).GetComponent<TextMeshProUGUI>();

                ButtonOperations(btn);

                int random = Random.Range(0, wordsCopy.Count);
                txt.text = wordsCopy[random];
                wordsCopy.RemoveAt(random);
            }
        }
    }
}
