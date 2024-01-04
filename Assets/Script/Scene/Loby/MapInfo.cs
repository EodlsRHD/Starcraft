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

    private RectTransform _rT = null;

    public void Initialize()
    {
        _rT = this.GetComponent<RectTransform>();

        this.gameObject.SetActive(false);
    }

    public void Open(MapData data)
    {
        GameManager.instance.toolManager.ImageDownload(data.thumbnailPath, (texture, result) => 
        {
            if(result == true)
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

    public void Close()
    {
        GameManager.instance.toolManager.MoveX(_rT, 1310f, 1f, false, () =>
        {
            _imageThumbnail.sprite = null;

            _textName.text = string.Empty;
            _textDescription.text = string.Empty;
            _textVersion.text = string.Empty;
            _textMaker.text = string.Empty;

            this.gameObject.SetActive(false);
        });
    }
}
