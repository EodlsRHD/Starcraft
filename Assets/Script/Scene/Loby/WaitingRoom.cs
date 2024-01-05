using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WaitingRoom : MonoBehaviour
{
    [SerializeField]
    private Button _buttonStartGame = null;

    [SerializeField]
    private Button _buttonCancel = null;

    [SerializeField]
    private HomeAndAwayTamplate _tamplate = null;

    [SerializeField]
    private Transform _templateParent = null;

    private RectTransform _rT = null;

    private Action<Action> _onCloseCallback = null;

    private MapData _mapData = null;

    public void Initialize(Action<Action> onCloseCallback)
    {
        _tamplate.Initialize();

        if (onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        _buttonStartGame.onClick.AddListener(OnGameStart);
        _buttonCancel.onClick.AddListener(() => { OnClose(); });

        _rT = this.GetComponent<RectTransform>();

        this.gameObject.SetActive(false);
    }

    public void Open(MapData data)
    {
        this.gameObject.SetActive(true);

        _mapData = data;

        GameManager.instance.toolManager.MoveX(_rT, -510f, 1f, false, () =>
        {
            switch(_mapData.classification)
            {
                case eClassification.HomeAndAway:
                    {
                        for (int i = 0; i < _mapData.teamNum; i++)
                        {
                            HomeAndAwayTamplate tamplate = Instantiate(_tamplate, _templateParent);
                            tamplate.SetHomeAndAway("Home");
                        }
                    }
                    break;

                case eClassification.Solo:
                    {
                        for (int i = 0; i < _mapData.maxPlayer; i++)
                        {
                            HomeAndAwayTamplate tamplate = Instantiate(_tamplate, _templateParent);
                        }
                    }
                    break;
            }
        });
    }

    private void OnGameStart()
    {
        if (GameManager.instance.currentMapdata == null)
        {
            return;
        }

        OnClose(() => 
        {
            GameManager.instance.currentMapdata = _mapData;
            GameManager.instance.toolManager.ChangeScene("Game", GameManager.instance.currentMapdata.name, eScene.Game);
        });
    }

    public void Close(Action onResult)
    {
        GameManager.instance.toolManager.MoveX(_rT, -1310f, 0.5f, false, () =>
        {
            this.gameObject.SetActive(false);

            onResult?.Invoke();

            DeleteTamplate();
        });
    }

    private void OnClose(Action onResult = null)
    {
        _onCloseCallback?.Invoke(onResult);
    }

    private void DeleteTamplate()
    {
        var child = _templateParent.transform.GetComponentsInChildren<Transform>();

        foreach (var iter in child)
        {
            if (iter != _templateParent.transform)
            {
                Destroy(iter.gameObject);
            }
        }

        _templateParent.transform.DetachChildren();
    }
}
