using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SingleGame : MonoBehaviour
{
    [SerializeField]
    private Button _buttonPlay = null;

    [SerializeField]
    private Button _buttonClose = null;

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

        _buttonPlay.onClick.AddListener(OnPlay);
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

    private void OnPlay()
    {
        _buttonPlay.gameObject.SetActive(false);
        _buttonClose.gameObject.SetActive(false);

        _onPlayCallback?.Invoke(ePlayMode.Single, () => { Open(); });
    }

    public void ButtonsOn()
    {
        _buttonPlay.gameObject.SetActive(true);
        _buttonClose.gameObject.SetActive(true);
    }
}
