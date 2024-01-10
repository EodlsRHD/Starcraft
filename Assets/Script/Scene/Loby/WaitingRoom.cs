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
    private Action<MapData> _onGameStartonCallback = null;

    private MapData _mapData = null;

    private List<HomeAndAwayTamplate> _tamplates = new List<HomeAndAwayTamplate>();

    public void Initialize(Action<Action> onCloseCallback, Action<MapData> onGameStartonCallback)
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

    public void Open(MapData data)
    {
        this.gameObject.SetActive(true);

        _mapData = data;

        if(_mapData.members == null)
        {
            _mapData.members = new PlayerInfo[_mapData.maxPlayer];
            _mapData.members[0] = GameManager.instance.playerInfo;
            _mapData.roomHostID = GameManager.instance.playerInfo.id;
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

                                if (_mapData.roomHostID.Equals((string)GameManager.instance.playerInfo.id, StringComparison.Ordinal))
                                {
                                    if (playerCount + p == 0)
                                    {
                                        playerTamplate.SetPlayer((string)GameManager.instance.playerInfo.id, GameManager.instance.playerInfo.nickName, true);
                                        _mapData.members[playerCount + p] = GameManager.instance.playerInfo;
                                    }
                                    else
                                    {
                                        playerTamplate.SetPlayer(string.Empty,  "Empty", false);
                                    }
                                }
                                else
                                {

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

                            if (_mapData.roomHostID.Equals((string)GameManager.instance.playerInfo.id, StringComparison.Ordinal))
                            {
                                if (i == 0)
                                {
                                    tamplate.SetPlayer((string)GameManager.instance.playerInfo.id, GameManager.instance.playerInfo.nickName, true);
                                    _mapData.members[i] = GameManager.instance.playerInfo;
                                }
                                else
                                {
                                    tamplate.SetPlayer(string.Empty, "Empty", false);
                                }
                            }
                            else
                            {

                            }

                            _tamplates.Add(tamplate);
                        }
                    }
                    break;
            }
        }));

        GameManager.instance.currentMapdata = _mapData;
    }

    private void OnGameStart()
    {
        for (int i = 0; i < GameManager.instance.currentMapdata.members.Length; i++)
        {
            if (GameManager.instance.currentMapdata.members[i] == null)
            {
                continue;
            }

            foreach (var tam in _tamplates)
            {
                if(tam.isPlayer == false)
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

                if (id.Equals(GameManager.instance.currentMapdata.members[i].id, StringComparison.Ordinal))
                {
                    GameManager.instance.currentMapdata.members[i].brood = (byte)b;

                    if(id.Equals(GameManager.instance.playerInfo.id))
                    {
                        GameManager.instance.playerInfo.brood = (byte)b;
                    }

                    break;
                }
            }
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

        _tamplates.Clear();

        _templateParent.transform.DetachChildren();
    }
}
