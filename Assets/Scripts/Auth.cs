using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Auth : MonoBehaviour
{
    public TMP_InputField reg_emailField;
    public TMP_InputField reg_phoneField;
    public TMP_InputField reg_passwordField;
    public TMP_InputField reg_repeatPasswordField;
    public Button reg_submitButton;

    public TMP_InputField login_emailPhoneField;
    public TMP_InputField login_passwordField;
    public Button login_submitButton;

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", reg_emailField.text);
        form.AddField("phone", reg_phoneField.text);
        form.AddField("password", reg_passwordField.text);

        WWW www = new WWW("http://localhost/sqlconnect/register.php", form);
        yield return www;
        
        if (www.text == "0") {
            Debug.Log("User created successfully.");
            Common.common.email = reg_emailField.text;
            SceneManager.LoadScene("MainMenu");
        } else {
            Debug.Log("User creation failed. Error " + www.text);
        }
    }

    public void VerifyRegisterInputs()
    {
        reg_submitButton.interactable = (reg_phoneField.text.Length > 0 && reg_emailField.text.Length > 0 && reg_passwordField.text.Length >= 8) && 
                                        (reg_passwordField.text == reg_repeatPasswordField.text);
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", login_emailPhoneField.text);
        form.AddField("phone", login_emailPhoneField.text);
        form.AddField("password", login_passwordField.text);

        WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
        yield return www;
        
        if (www.text == "0") {
            Debug.Log("User logged in successfully.");
            Common.common.email = login_emailPhoneField.text;
            SceneManager.LoadScene("MainMenu");
        } else {
            Debug.Log("User login failed. Error " + www.text);
        }
    }

    public void VerifyLoginInputs()
    {
        login_submitButton.interactable = login_emailPhoneField.text.Length > 0 && login_passwordField.text.Length >= 8;
    }
}
