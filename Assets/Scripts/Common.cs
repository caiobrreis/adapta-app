using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Common : MonoBehaviour
{
    public static Common common;

    [HideInInspector] public string username = "";
    [HideInInspector] public int prevScene;

    public GameObject redOutline;
    public GameObject greenOutline;
    public float outlineDuration = 0.5f;

    [HideInInspector] public int speedScore;
    [HideInInspector] public int correctGuessesScore;

    void Awake()
    {
        if (common == null) {
            DontDestroyOnLoad(gameObject);
            common = this;
        } else if (common != this) {
            Destroy(gameObject);
        }
    }

    public void KeepInputOpen(TMP_InputField inputField)
    {
        inputField.ActivateInputField();
        inputField.Select();
    }

    public void ShowOutline(bool correct, Transform parent)
    {
        if (correct) {
            GameObject obj = Instantiate(greenOutline, parent);
            Destroy(obj, outlineDuration);
        } else {
            GameObject obj = Instantiate(redOutline, parent);
            Destroy(obj, outlineDuration);
        }
    }

    public void CalculateSpeed(long time, float worstSpeed)
    {
        float t = Mathf.Clamp01(time / worstSpeed);
        speedScore = (int)Mathf.Lerp(500f, 0f, t);
    }

    public void WriteToTextFile(long time)
    {
        string path = "Assets/Data/Score.txt";

        StreamWriter writer = new StreamWriter(path); // (path, true) to append
        // falta nome
        writer.WriteLine($"{DateTime.Now}, {correctGuessesScore}, {speedScore}, {correctGuessesScore + speedScore}, {time}");
        writer.Close();
    }

    public void ResultScene()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Result");
    }
}
