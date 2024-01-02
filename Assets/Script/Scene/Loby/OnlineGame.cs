using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OnlineGame : MonoBehaviour
{
    [SerializeField]
    private Button _buttonClose = null;

    private Action _onCloseCallback = null;

    public void Initialize(Action onCloseCallback)
    {
        if(onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        _buttonClose.onClick.AddListener(OnClose);

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
}
