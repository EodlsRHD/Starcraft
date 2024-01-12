using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

public class Login : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputFieldId = null;

    [SerializeField]
    private TMP_InputField _inputFieldPassword = null;

    [SerializeField]
    private Button _buttonSignIn = null;

    [SerializeField]
    private Button _buttonSignUp = null;

    [SerializeField]
    private Button _buttonForgetPassword = null;

    [SerializeField]
    private GameObject _objPlayerInfoSettings = null;

    [SerializeField]
    private TMP_InputField _inputFieldNickName = null;

    [SerializeField]
    private Button _buttonSignUpAccept = null;

    public void Initilaize()
    {
        _buttonSignIn.onClick.AddListener(OnSignIn);
        _buttonSignUp.onClick.AddListener(OnSignUp);
        _buttonForgetPassword.onClick.AddListener(OnForgetPassword);
        _buttonSignUpAccept.onClick.AddListener(OnSignUpAccept);

        _objPlayerInfoSettings.gameObject.SetActive(false);
    }

    public void Open()
    {
        _inputFieldId.text = string.Empty;
        _inputFieldPassword.text = string.Empty;

        this.gameObject.SetActive(true);
    }

    private void OnSignIn()
    {
        if(_inputFieldId.text.Length == 0 || _inputFieldPassword.text.Length == 0)
        {
            InterfaceManager.instance.OpenOneButton("Sign In", "ID or password cannot be empty.", () =>
            {
                InterfaceManager.instance.ClosePopUp();
            }, "Okay");

            return;
        }

        ServerManager.instance.SignInPlayerInfo(_inputFieldId.text, _inputFieldPassword.text, (result) =>
        {
            if(result == false)
            {
                InterfaceManager.instance.OpenOneButton("Sign In", "Member information not found.", () =>
                {
                    InterfaceManager.instance.ClosePopUp();
                }, "Okay");
                return;
            }

            ServerManager.instance.GetPlayerInfo(_inputFieldId.text, _inputFieldPassword.text, (result) =>
            {
                if (result == null)
                {
                    InterfaceManager.instance.OpenOneButton("Sign In", "I don't have any member information.", () =>
                    {
                        InterfaceManager.instance.ClosePopUp();
                    });

                    return;
                }

                GameManager.instance.playerInfo = result;

                UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Loby);
                UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Interface, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            });
        });
    }

    private void OnSignUp()
    {
        if (_inputFieldId.text.Length == 0 || _inputFieldPassword.text.Length == 0)
        {
            InterfaceManager.instance.OpenOneButton("Sign Up", "ID or password cannot be empty.", () =>
            {
                InterfaceManager.instance.ClosePopUp();
            }, "Okay");

            return;
        }

        _objPlayerInfoSettings.gameObject.SetActive(true);
    }

    private void OnSignUpAccept()
    {
        if (_inputFieldId.text.Length == 0 || _inputFieldPassword.text.Length == 0)
        {
            InterfaceManager.instance.OpenOneButton("Sign Up", "ID or password cannot be empty.", () =>
            {
                InterfaceManager.instance.ClosePopUp();
            }, "Okay");

            return;
        }

        ServerManager.instance.CreatePlayerInfo(_inputFieldId.text, _inputFieldPassword.text, _inputFieldNickName.text, (result) =>
        {
            if(result != null)
            {
                InterfaceManager.instance.OpenOneButton("Sign Up", "Members that exist.", () => 
                {
                    InterfaceManager.instance.ClosePopUp();
                });

                return;
            }

            GameManager.instance.playerInfo = result;

            InterfaceManager.instance.OpenOneButton("Sign Up", "Success.", () =>
            {
                InterfaceManager.instance.ClosePopUp();

                _objPlayerInfoSettings.gameObject.SetActive(false);

                UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Loby);
                UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Interface, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }, "Okay");
        });
    }

    private void OnForgetPassword()
    {

    }
}
