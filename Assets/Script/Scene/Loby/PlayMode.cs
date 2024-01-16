using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayMode : MonoBehaviour
{
    [SerializeField]
    private MapList _mapList = null;

    [SerializeField]
    private MapInfo _mapInfo = null;

    [SerializeField]
    private WaitingRoom _waitingRoom = null;

    private Action _onCloseCallback = null;
    private Action _onResult = null;

    private ePlayMode _playMode = ePlayMode.Non;

    public void Initialize(Action onCloseCallback)
    {
        _mapList.Initialize(OpenMapInfo, OnClose);
        _mapInfo.Initialize(OpenWaitingRoom, CloseMapList);
        _waitingRoom.Initialize(WaitingRoomClose, GameStart);

        if (onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        this.gameObject.SetActive(false);
    }

    public void Open(ePlayMode playMode, Action onResult)
    {
        if(onResult != null)
        {
            _onResult = onResult;
        }

        _playMode = playMode;

        _mapList.Open(_playMode);

        this.gameObject.SetActive(true);
    }

    public void Close(Action onResult)
    {
        _mapList.Close(() => 
        {
            this.gameObject.SetActive(false);

            _onResult = null;

            onResult?.Invoke();
        });
    }

    private void OnClose()
    {
        _mapInfo.Close();
        _onCloseCallback?.Invoke();
        _onResult?.Invoke();
    }

    private void GameStart(MapData mapData)
    {
        _waitingRoom.Close();
        _mapInfo.Close(() => 
        {
            GameManager.instance.currentMapdata = mapData;
            GameManager.instance.toolManager.ChangeScene("Game", GameManager.instance.currentMapdata.name, eScene.Game);
        });
    }

    private void OpenMapInfo(MapData mapData)
    {
        _mapInfo.Open(mapData);
    }

    private void OpenWaitingRoom(MapData mapData)
    {
        _waitingRoom.Open(_playMode, mapData);
        _mapInfo.ActiveSelectButton(true);
    }

    private void CloseMapList(Action onResult)
    {
        _mapList.Close(onResult);
    }

    private void WaitingRoomClose(ePlayMode playerMode, Action onResult)
    {
        _mapInfo.ActiveSelectButton(false);
        _mapInfo.Close();
        _waitingRoom.Close(() => 
        {
            _mapList.Open(playerMode);
        });
    }
}