using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Button _ButtonClose = null;

    [Header("Tab")]

    [SerializeField]
    private Toggle _toggleSocial = null;

    [SerializeField]
    private Settings_Social _social = null;

    [Space(5)]

    [SerializeField]
    private Toggle _toggleSound = null;

    [SerializeField]
    private Settings_Sound _sound = null;

    [Space(5)]

    [SerializeField]
    private Toggle _toggleGraphic = null;

    [SerializeField]
    private Settings_Graphic _graphic = null;

    [Space(5)]

    [SerializeField]
    private Toggle _toggleGamePlay= null;

    [SerializeField]
    private Settings_GamePlay _gamePlay = null;

    private Action _onCloseCallback = null;

    public void Initialize(Action onCloseCallback)
    {
        _social.Initialize();
        _sound.Initialize();
        _graphic.Initialize();
        _gamePlay.Initialize();

        if (onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        _ButtonClose.onClick.AddListener(OnClose);

        _toggleSocial.onValueChanged.AddListener((isOn) =>
        {
            _social.gameObject.SetActive(isOn);

            if (isOn == false)
            {
                return;
            }

            _social.Open();
        });

        _toggleSound.onValueChanged.AddListener((isOn) =>
        {
            _sound.gameObject.SetActive(isOn);

            if (isOn == false)
            {
                return;
            }

            _sound.Open();
        });

        _toggleGraphic.onValueChanged.AddListener((isOn) =>
        {
            _graphic.gameObject.SetActive(isOn);

            if (isOn == false)
            {
                return;
            }

            _graphic.Open();
        });

        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        _toggleSocial.isOn = true;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void OnClose()
    {
        _onCloseCallback?.Invoke();
    }

    public void Save()
    {
        _social.Save();
        _sound.Save();
        _graphic.Save();
        _gamePlay.Save();
    }
}
