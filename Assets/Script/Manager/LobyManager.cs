using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ePlayMode
{
    Non = -1,
    Single,
    Online
}

public class LobyManager : MonoBehaviour
{
    [SerializeField]
    private Loby _loby = null;

    [SerializeField]
    private SingleGame _singleGame = null;

    [SerializeField]
    private OnlineGame _onlineGame = null;

    [SerializeField]
    private PlayMode _play = null;

    void Start()
    {
        _loby.Initialize(SingleGameOpen, OnlineGameOpen);
        _singleGame.Initialize(SingleGameClose, PlayOpen);
        _onlineGame.Initialize(OnlineGameClose, PlayOpen);
        _play.Initialize(PlayClose);

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

    private void PlayOpen(ePlayMode playMode, Action onResult)
    {
        _play.Open(playMode, onResult);
    }

    private void PlayClose()
    {
        _play.Close(() => 
        { 
            _singleGame.ButtonsOn();
            _onlineGame.ButtonsOn();
        });
    }
}
