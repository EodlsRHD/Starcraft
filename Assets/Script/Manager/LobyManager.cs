using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LobyManager : MonoBehaviour
{
    [SerializeField]
    private Loby _loby = null;

    [SerializeField]
    private SingleGame _singleGame = null;

    [SerializeField]
    private OnlineGame _onlineGame = null;

    [SerializeField]
    private CustomPlay _customPlay = null;

    void Start()
    {
        _loby.Initialize(SingleGameOpen, OnlineGameOpen);
        _singleGame.Initialize(SingleGameClose, CustomPlayOpen);
        _onlineGame.Initialize(OnlineGameClose);
        _customPlay.Initialize(CustomPlayClose);

        _loby.Open();
    }

    private void SingleGameOpen()
    {
        _loby.Close();
        _singleGame.Open();
    }

    private void SingleGameClose()
    {
        _singleGame.Close();
        _loby.Open();
    }

    private void OnlineGameOpen()
    {
        _loby.Close();
        _onlineGame.Open();
    }

    private void OnlineGameClose()
    {
        _onlineGame.Close();
        _loby.Open();
    }

    private void CustomPlayOpen(Action onResult)
    {
        _customPlay.Open(onResult);
    }

    private void CustomPlayClose()
    {
        _customPlay.Close(() => { _singleGame.ButtonsOn(); });
    }
}
