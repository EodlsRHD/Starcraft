using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SingleGame : MonoBehaviour
{
    [SerializeField]
    private Button _buttonCustomPlay = null;

    [SerializeField]
    private Button _buttonClose = null;

    private Action _onCloseCallback = null;

    private Action<Action> _onCustomPlayCallback = null;

    public void Initialize(Action onCloseCallback, Action<Action> onCustomPlayCallback)
    {
        if(onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        if(onCustomPlayCallback != null)
        {
            _onCustomPlayCallback = onCustomPlayCallback;
        }

        _buttonCustomPlay.onClick.AddListener(OnCustomPlay);
        _buttonClose.onClick.AddListener(OnClose);

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

    private void OnClose()
    {
        _onCloseCallback?.Invoke();
    }

    private void OnCustomPlay()
    {
        _buttonCustomPlay.gameObject.SetActive(false);
        _buttonClose.gameObject.SetActive(false);

        _onCustomPlayCallback?.Invoke(() => { Open(); });
    }

    public void ButtonsOn()
    {
        _buttonCustomPlay.gameObject.SetActive(true);
        _buttonClose.gameObject.SetActive(true);
    }
}
