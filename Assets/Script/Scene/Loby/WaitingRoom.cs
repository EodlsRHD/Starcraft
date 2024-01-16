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

    private Action<ePlayMode, Action> _onCloseCallback = null;
    private Action<MapData> _onGameStartonCallback = null;

    private MapData _mapData = null;

    private List<HomeAndAwayTamplate> _tamplates = new List<HomeAndAwayTamplate>();

    private ePlayMode _playMode = ePlayMode.Non;

    private bool _isHost = false;

    public void Initialize(Action<ePlayMode, Action> onCloseCallback, Action<MapData> onGameStartonCallback)
    {
        _tamplate.Initialize();

        if (onCloseCallback != null)
        {
            _onCloseCallback = onCloseCallback;
        }

        if(onGameStartonCallback != null)
        {
            _onGameStartonCallback = onGameStartonCallback;
        }

        _buttonStartGame.onClick.AddListener(OnGameStart);
        _buttonCancel.onClick.AddListener(() => { OnClose(); });

        _rT = this.GetComponent<RectTransform>();

        this.gameObject.SetActive(false);
    }

    public void Open(ePlayMode playMode, MapData data)
    {
        this.gameObject.SetActive(true);
        _templateParent.gameObject.SetActive(false);

        _mapData = data;
        _playMode = playMode;

        if (_mapData.memberIDs == null)
        {
            _mapData.memberIDs = new string[_mapData.maxPlayer];
            _mapData.memberIDs[0] = GameManager.instance.currentPlayerInfo._id;
            _mapData.roomHostUuid = GameManager.instance.currentPlayerInfo._id;

            _isHost = true;
        }

        GameManager.instance.toolManager.MoveX(_rT, -510f, 1f, false, (Action)(() =>
        {
            switch(_mapData.classification)
            {
                case eClassification.HomeAndAway:
                    {
                        int playerCount = 0;

                        for (int i = 0; i < _mapData.teamCount; i++)
                        {
                            HomeAndAwayTamplate tamplate = Instantiate(_tamplate, _templateParent);

                            if(i == 0)
                            {
                                tamplate.SetHomeAndAway("Home");
                            }
                            else if(i == 1)
                            {
                                tamplate.SetHomeAndAway("Away");
                            }

                            for (int p = 0; p < (_mapData.maxPlayer / _mapData.teamCount); p++)
                            {
                                HomeAndAwayTamplate playerTamplate = Instantiate(_tamplate, _templateParent);

                                if (_mapData.memberIDs[i] != null)
                                {
                                    ServerManager.instance.GetPlayerInfo(_mapData.memberIDs[i], (result) =>
                                    {
                                        tamplate.SetPlayer(result.userID, result.nickName, true);
                                    });
                                }
                                else
                                {
                                    tamplate.SetPlayer(string.Empty, "Empty", false);
                                }

                                _tamplates.Add(playerTamplate);

                                playerCount++;
                            }
                        }
                    }
                    break;

                case eClassification.Solo:
                    {
                        for (int i = 0; i < _mapData.maxPlayer; i++)
                        {
                            HomeAndAwayTamplate tamplate = Instantiate(_tamplate, _templateParent);

                            if(_mapData.memberIDs[i] != null)
                            {
                                ServerManager.instance.GetPlayerInfo(_mapData.memberIDs[i], (result) =>
                                {
                                    tamplate.SetPlayer(result.userID, result.nickName, true);
                                });
                            }
                            else
                            {
                                tamplate.SetPlayer(string.Empty, "Empty", false);
                            }

                            _tamplates.Add(tamplate);
                        }
                    }
                    break;
            }
        }));

        _templateParent.gameObject.SetActive(true);
        GameManager.instance.currentMapdata = _mapData;
    }

    private void OnGameStart()
    {
        for (int i = 0; i < GameManager.instance.currentMapdata.memberIDs.Length; i++)
        {
            if (GameManager.instance.currentMapdata.memberIDs[i] == null)
            {
                continue;
            }

            ServerManager.instance.GetPlayerInfo(_mapData.memberIDs[i], (result) =>
            {
                foreach (var tam in _tamplates)
                {
                    if (tam.isPlayer == false)
                    {
                        continue;
                    }

                    eRace b = tam.GetRaceValue(out string id);

                    if ((int)b == 4) // Non
                    {
                        InterfaceManager.instance.OpenOneButton(_mapData.name, "All users must choose the race.", () =>
                        {
                            InterfaceManager.instance.ClosePopUp();
                        }, "Ok");

                        return;
                    }

                    if (id.Equals(result.userID, StringComparison.Ordinal))
                    {
                        result.brood = (byte)b;

                        if (id.Equals(GameManager.instance.currentPlayerInfo.userID))
                        {
                            GameManager.instance.currentPlayerInfo.brood = (byte)b;
                        }

                        break;
                    }
                }
            });
        }

        _onGameStartonCallback?.Invoke(_mapData);
    }

    public void Close(Action onResult = null)
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
        _onCloseCallback?.Invoke(_playMode, onResult);
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

        _tamplates.Clear();

        _templateParent.transform.DetachChildren();
    }
}
