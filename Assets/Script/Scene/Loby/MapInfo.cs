using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MapInfo : MonoBehaviour
{
    [SerializeField]
    private Image _imageThumbnail = null;

    [SerializeField]
    private TMP_Text _textName = null;

    [SerializeField]
    private TMP_Text _textDescription = null;

    [SerializeField]
    private TMP_Text _textMaxPlayer = null;

    [SerializeField]
    private TMP_Text _textVersion = null;

    [SerializeField]
    private TMP_Text _textMaker = null;

    [SerializeField]
    private Button _buttonSelectMap = null;

    [SerializeField]
    private Button _buttonChangeOnline = null;

    private RectTransform _rT = null;

    private Action<MapData> _onOpenWaitingRoomCallback = null;
    private Action<Action> _onCloseMapListCallback = null;

    private MapData _mapData = null;

    public void Initialize(Action<MapData> onOpenWaitingRoomCallback, Action<Action> onCloseMapListCallback)
    {
        if(onOpenWaitingRoomCallback != null)
        {
            _onOpenWaitingRoomCallback = onOpenWaitingRoomCallback;
        }

        if(onCloseMapListCallback != null)
        {
            _onCloseMapListCallback = onCloseMapListCallback;
        }

        _buttonChangeOnline.onClick.AddListener(OnChangeOnline);
        _buttonSelectMap.onClick.AddListener(OnSelectMap);

        _buttonChangeOnline.gameObject.SetActive(false);
        _buttonSelectMap.gameObject.SetActive(true);

        _rT = this.GetComponent<RectTransform>();

        this.gameObject.SetActive(false);
    }

    public void Open(MapData data)
    {
        if(_mapData != null)
        {
            if (data.id == _mapData.id)
            {
                return;
            }
        }

        if(this.gameObject.activeSelf == true)
        {
            Close(() => 
            {
                OpenData(data);
            });
        }
        else
        {
            OpenData(data);
        }
    }

    private void OpenData(MapData data)
    {
        ServerManager.instance.ImageDownload(data.thumbnailPath, (texture, result) =>
        {
            _mapData = data;

            if (result == true)
            {
                Sprite s = GameManager.instance.toolManager.ConvertTextureToSprite(texture);

                if (s != null)
                {
                    _imageThumbnail.sprite = s;
                }
            }

            _textName.text = data.name;
            _textDescription.text = data.description;
            _textMaxPlayer.text = data.maxPlayer.ToString();
            _textVersion.text = data.version;
            _textMaker.text = data.maker;

            this.gameObject.SetActive(true);

            GameManager.instance.toolManager.MoveX(_rT, 510f, 1f, false, () =>
            {

            });
        });
    }

    public void Close(Action onResult = null)
    {
        if(this.gameObject.activeSelf == false)
        {
            return;
        }

        GameManager.instance.toolManager.MoveX(_rT, 1310f, 0.5f, false, () =>
        {
            _mapData = null;

            _imageThumbnail.sprite = null;

            _textName.text = string.Empty;
            _textDescription.text = string.Empty;
            _textVersion.text = string.Empty;
            _textMaker.text = string.Empty;

            _buttonChangeOnline.gameObject.SetActive(false);
            _buttonSelectMap.gameObject.SetActive(true);

            this.gameObject.SetActive(false);

            onResult?.Invoke();
        });
    }

    public void ActiveSelectButton(bool isOn)
    {
        _buttonChangeOnline.gameObject.SetActive(isOn);
        _buttonSelectMap.gameObject.SetActive(!isOn);
    }

    private void OnChangeOnline()
    {
        ServerManager.instance.JoinOrCreateRoom(_mapData, () => 
        {
            _buttonChangeOnline.gameObject.SetActive(false);
        });
    }

    private void OnSelectMap()
    {
        _onCloseMapListCallback(() => { _onOpenWaitingRoomCallback?.Invoke(_mapData); });
    }
}
