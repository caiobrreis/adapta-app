using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Silhouette : MonoBehaviour
{
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private TextMeshProUGUI roundCountField;
    [SerializeField] private Image imageField;
    [SerializeField] private Transform buttonsParent;

    private int generatedImageIdx;

    private byte round = 1;
    private byte totalRounds = 5;

    private int correctGuesses = 0;
    private float worstSpeedPerRound = 20000f;

    //! NÃO MUDAR O NOME DESSA VARIÁVEL images
    public Sprite[] images = new Sprite[] { };
    private string[] words = new string[] {
        "garrafa", "abajur", "ventilador", "lápis", "chave", "bicicleta", "caneca", "garfo", "foguete", "martelo", "lua",
        "algema", "ampulheta", "âncora", "balde", "bandeira", "bigode", "bigorna", "botão", "cadeira", "castelo", "coração",
        "coroa", "corrente", "escada", "estrela", "extintor", "hidrante", "leque", "machado", "óculos", "sino", "sofá",
        "telefone", "vela", 
        "aranha", "elefante", "cachorro", "gato", "rato", "girafa", "peixe", "formiga", "borboleta", "golfinho", "pinguim",
        "camelo", "caranguejo", "cobra", "dinossauro", "gorila", "pássaro", "touro", "urso", "vaca",
        "maça", "pera", "banana", "morango", "abacaxi", "cenoura", "pimenta", "amendoim", "cereja", "milho", "sorvete", "uva"
    };

    Stopwatch stopwatch = new Stopwatch();

    void Start()
    {
        stopwatch.Start();
        NewRound();
    }

    void NewRound()
    {
        roundCountField.text = $"round {round.ToString()}/{totalRounds.ToString()}";

        generatedImageIdx = Random.Range(0, images.Length);
        imageField.sprite = images[generatedImageIdx];

        ManageChoices();
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
        correctBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = images[generatedImageIdx].name;

        List<string> wordsCopy = words.ToList();
        wordsCopy.Remove(images[generatedImageIdx].name);

        foreach (Transform child in buttonsParent) {
            if (child != correctBtn.transform) {
                Button btn = child.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => Guessed(false));

                int random = Random.Range(0, wordsCopy.Count);
                child.GetChild(0).GetComponent<TextMeshProUGUI>().text = wordsCopy[random];
                wordsCopy.RemoveAt(random);
            }
        }
    }
}
