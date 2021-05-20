using System.Diagnostics;
using UnityEngine;
using TMPro;

public class MathOperations : MonoBehaviour
{
    [SerializeField] private GameObject greenOutline;
    [SerializeField] private GameObject redOutline;

    [SerializeField] private GameObject gameCanvasObj;
    [SerializeField] private GameObject resultCanvasObj;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [SerializeField] private TextMeshProUGUI roundCountText;
    [SerializeField] private TextMeshProUGUI generatedCalcField;
    [SerializeField] private TMP_InputField inputField;

    private int generatedSymbolIdx;
    private int generatedN1;
    private int generatedN2;
    private int result;

    private float answerFeedbackDuration = 0.5f;
    private byte round = 1;
    private byte totalRounds = 5;

    private byte accuracy = 0;
    private float speed = 0;
    private float worstSpeed = 20000f;

    private static string[] symbols = new string[] {
        "+", "-", "*"
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

            if (inputField.text.Equals(result.ToString())) {
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

        generatedSymbolIdx = UnityEngine.Random.Range(0, symbols.Length);

        generatedN1 = UnityEngine.Random.Range(1, 99);
        generatedN2 = generatedSymbolIdx == 1 ? UnityEngine.Random.Range(1, generatedN1) : UnityEngine.Random.Range(1, 99);

        generatedCalcField.text = $"{generatedN1.ToString()} {symbols[generatedSymbolIdx]} {generatedN2.ToString()} =";

        switch (generatedSymbolIdx) {
            case 0:
                result = generatedN1 + generatedN2;
                break;
            case 1:
                result = generatedN1 - generatedN2;
                break;
            case 2:
                result = generatedN1 * generatedN2;
                break;
        }
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
