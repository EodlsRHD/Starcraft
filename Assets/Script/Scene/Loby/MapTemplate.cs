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

    private string _dataPath = string.Empty;

    private Action<MapData> _onMapInfoCallback = null;

    public void Initialize(string name, string path, Action<MapData> onMapInfoCallback)
    {
        if (onMapInfoCallback != null)
        {
            _onMapInfoCallback = onMapInfoCallback;
        }

        _button.onClick.AddListener(OnOpenMapData);

        _dataPath = path;
        _textName.text = name;

        this.gameObject.SetActive(true);
    }

    private void OnOpenMapData()
    {
        UnZipMapFile file = GameManager.instance.toolManager.UnZipFile(_dataPath);

        _onMapInfoCallback?.Invoke(file.mapData);
    }
}
