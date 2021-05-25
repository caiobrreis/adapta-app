using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TextMeshProUGUI errorText;
    public Button confirmNameBtn;
    public GameObject nameScreen;
    
    void Awake()
    {
        if (Common.common.username == "")
            nameScreen.SetActive(true);
    }

    public void NameCheck()
    {
        if (nameInput.text == "") {
            errorText.text = "O nome não pode ser vazio.";
        } else {
            nameScreen.SetActive(false);
            Common.common.username = nameInput.text;
        }
    }
}
