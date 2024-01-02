using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputFieldId = null;

    [SerializeField]
    private TMP_InputField _inputFieldPassword = null;

    [SerializeField]
    private Button _buttonLogin = null;

    public void Initilaize()
    {
        _buttonLogin.onClick.AddListener(OnLogin);
    }

    public void Open()
    {
        _inputFieldId.text = string.Empty;
        _inputFieldPassword.text = string.Empty;

        this.gameObject.SetActive(true);
    }

    private void OnLogin()
    {
        // social login

        Debug.Log("TEST Login");
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Loby);
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Interface, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        //GameManager.instance.toolManager.ChangeScene("Loby", "...", eScene.Loby);
    }
}
