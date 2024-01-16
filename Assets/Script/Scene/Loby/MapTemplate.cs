using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MapTemplate : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textName = null;

    [SerializeField]
    private Button _button = null;

    private Action<MapData> _onMapInfoCallback = null;

    private ePlayMode _playMode = ePlayMode.Single;

    private string _path = string.Empty;

    private bool _isOpen = false;


    public void Initialize_Single(string name, string path, Action<MapData> onMapInfoCallback)
    {
        if (onMapInfoCallback != null)
        {
            _onMapInfoCallback = onMapInfoCallback;
        }

        _button.onClick.AddListener(OnOpenMapData);

        _playMode = ePlayMode.Single;
        _path = path;
        _textName.text = name;

        this.gameObject.SetActive(true);
    }

    public void Initialize_Online(string name, string path, Action<MapData> onMapInfoCallback)
    {
        if (onMapInfoCallback != null)
        {
            _onMapInfoCallback = onMapInfoCallback;
        }

        _button.onClick.AddListener(OnOpenMapData);

        _playMode = ePlayMode.Online;
        _path = path;
        _textName.text = name;

        this.gameObject.SetActive(true);
    }

    private void OnOpenMapData()
    {
        if(_isOpen == true)
        {
            return;
        }

        switch(_playMode)
        {
            case ePlayMode.Single:
                {
                    UnZipMapFile file = GameManager.instance.toolManager.UnZipFile(_path);

                    _onMapInfoCallback?.Invoke(file.mapData);
                }
                break;

            case ePlayMode.Online:
                {
                    ServerManager.instance.FildDownload(_path, (path, result) =>
                    {
                        if (result == false)
                        {
                            this.gameObject.SetActive(false);

                            return;
                        }

                        UnZipMapFile file = GameManager.instance.toolManager.UnZipFile(path);

                        ServerManager.instance.JoinOrCreateRoom(file.mapData, () =>
                        {
                            _onMapInfoCallback?.Invoke(file.mapData);
                        });
                    });
                }
                break;
        }

        _isOpen = true;
    }

    public void CloseInfo()
    {
        _isOpen = false;
    }
}
