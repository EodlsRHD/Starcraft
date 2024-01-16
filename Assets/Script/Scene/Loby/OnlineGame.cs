using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OnlineGame : MonoBehaviour
{
    [SerializeField]
    private Button _buttonClose = null;

    [SerializeField]
    private Button _buttonEnterOnline = null;

    private Action _onCloseCallback = null;
    private Action<ePlayMode, Action> _onPlayCallback = null;

    public void Initialize(Action onCloseCallback, Action<ePlayMode, Action> onPlayCallback)
    {
        if(onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        if(onPlayCallback != null)
        {
            _onPlayCallback = onPlayCallback;
        }

        _buttonClose.onClick.AddListener(OnClose);
        _buttonEnterOnline.onClick.AddListener(OnEnterOnline);

        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        Debug.Log("Enter Online");
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

    private void OnEnterOnline()
    {
        _onPlayCallback?.Invoke(ePlayMode.Online, () => { Open(); });
    }

    public void ButtonsOn()
    {
        _buttonEnterOnline.gameObject.SetActive(true);
        _buttonClose.gameObject.SetActive(true);
    }
}
