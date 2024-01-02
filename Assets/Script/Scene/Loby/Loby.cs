using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Loby : MonoBehaviour
{
    [SerializeField]
    private Button _buttonSingle = null;

    [SerializeField]
    private Button _buttonOnline = null;

    [SerializeField]
    private Button _buttonSettings = null;

    [SerializeField]
    private Button _buttonQuit = null;

    private Action _onOpenSingleGmaeCallback = null;

    private Action _onOpenOnlineGameCallback = null;

    public void Initialize(Action onOpenSingleGmaeCallback, Action onOpenOnlineGameCallback)
    {
        if(onOpenSingleGmaeCallback != null)
        {
            _onOpenSingleGmaeCallback = onOpenSingleGmaeCallback;
        }

        if (onOpenOnlineGameCallback != null)
        {
            _onOpenOnlineGameCallback = onOpenOnlineGameCallback;
        }

        _buttonSingle.onClick.AddListener(OnSingle);
        _buttonOnline.onClick.AddListener(OnOnline);
        _buttonSettings.onClick.AddListener(OnSettings);
        _buttonQuit.onClick.AddListener(OnQuit);

        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void OnSingle()
    {
        _onOpenSingleGmaeCallback?.Invoke();
    }

    private void OnOnline()
    {
        _onOpenOnlineGameCallback?.Invoke();
    }

    private void OnSettings()
    {
        InterfaceManager.instance.OpenSettings();
    }

    private void OnQuit()
    {
        InterfaceManager.instance.OpenYesOrNoPopup("Loby", "OnQuit?", () =>
        {
            InterfaceManager.instance.ClosePopUp();
        }, () =>
        {
            InterfaceManager.instance.ClosePopUp();

            GameManager.instance.GameQuit();
        });
    }
}
