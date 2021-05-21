using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI total;
    public Button playAgainBtn;

    void Awake()
    {
        playAgainBtn.onClick.AddListener(PreviousScene);
    }

    void Start()
    {
        accuracy.text = Common.common.correctGuessesScore.ToString();
        speed.text = Common.common.speedScore.ToString();
        total.text = (Common.common.correctGuessesScore + Common.common.speedScore).ToString();
    }

    void PreviousScene()
    {
        SceneManager.LoadScene(Common.common.prevScene);
    }
}
