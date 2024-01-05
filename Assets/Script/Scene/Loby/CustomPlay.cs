using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomPlay : MonoBehaviour
{
    [SerializeField]
    private MapList _mapList = null;

    [SerializeField]
    private MapInfo _mapInfo = null;

    [SerializeField]
    private WaitingRoom _waitingRoom = null;

    private Action _onCloseCallback = null;

    private Action _onResult = null;

    public void Initialize(Action onCloseCallback)
    {
        _mapList.Initialize(OpenMapInfo, OnClose);
        _mapInfo.Initialize(OpenWaitingRoom, CloseMapList);
        _waitingRoom.Initialize(WaitingRoomClose);

        if (onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        this.gameObject.SetActive(false);
    }

    public void Open(Action onResult)
    {
        if(onResult != null)
        {
            _onResult = onResult;
        }

        _mapList.Open();

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

    private void OpenMapInfo(MapData mapData)
    {
        GameManager.instance.currentMapdata = mapData;
        _mapInfo.Open(mapData);
    }

    private void OpenWaitingRoom(MapData mapData)
    {
        _waitingRoom.Open(mapData);
    }

    private void CloseMapList(Action onResult)
    {
        _mapList.Close(onResult);
    }

    private void WaitingRoomClose(Action onResult)
    {
        _mapInfo.Close();
        _waitingRoom.Close(() => 
        {
            _mapList.Open();
        });
    }
}