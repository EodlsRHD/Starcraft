using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomPlay : MonoBehaviour
{
    [SerializeField]
    private SelectMap _selectMap = null;

    [SerializeField]
    private Button _buttonStart = null;

    [SerializeField]
    private Button _buttonClose = null;

    private Action _onCloseCallback = null;

    private Action _onResult = null;

    public void Initialize(Action onCloseCallback)
    {
        _selectMap.Initialize(() => { _buttonStart.gameObject.SetActive(true); });

        if (onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        _buttonStart.onClick.AddListener(GameStart);
        _buttonClose.onClick.AddListener(OnClose);

        _buttonStart.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Open(Action onResult)
    {
        if(onResult != null)
        {
            _onResult = onResult;
        }

        _selectMap.Open();

        this.gameObject.SetActive(true);
    }

    public void Close(Action onResult)
    {
        _selectMap.Close(() => 
        {
            this.gameObject.SetActive(false);

            _onResult = null;

            onResult?.Invoke();
        });
    }

    private void OnClose()
    {
        _onCloseCallback?.Invoke();
        _onResult?.Invoke();
    }

    private void GameStart()
    {
        if (GameManager.instance.currentMapdata == null)
        {
            return;
        }

        GameManager.instance.toolManager.ChangeScene("Game", GameManager.instance.currentMapdata.name, eScene.Game);
    }
}