using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using TMPro;

public enum eResourceType
{
    Non = -1,
    Mineral,
    Gas
}

public enum ePlayerColor
{
    Non = -1,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Black
}

[System.Serializable]
public class MapData
{
    public string name = string.Empty;
    public string description = string.Empty;
    public string version = string.Empty;
    public string maker = string.Empty;
    public int maxPlayer = 0;

    public string thumbnailPath = string.Empty;

    public int mapSizeX = 0;
    public int mpaSizeY = 0;

    public Node[,] nodes = null;
}

[System.Serializable]
public class Node
{
    public float x = 0;
    public float y = 0;

    public Topographic topographic = null;
    public StartPosition startPosition = null;
    public Resource resource = null;
}

[System.Serializable]
public class Topographic
{
    public bool isWalkable = false;
    public bool isHill = false;
    public float height = 0; // 0 ~ 1
}

[System.Serializable]
public class StartPosition
{
    public int team = 0;
    public ePlayerColor playerColor = ePlayerColor.Non;
}

[System.Serializable]
public class Resource
{
    public eResourceType type = eResourceType.Non;
    public int quantity = 0;
}

public class SelectMap : MonoBehaviour
{
    [SerializeField]
    private MapInfo _mapInfo = null;

    [SerializeField]
    private MapTemplate _mapTamplate = null;

    [SerializeField]
    private Transform _templateParent = null;

    [SerializeField]
    private TMP_Text _textPath = null;

    private RectTransform _rT = null;

    private Action _onStartButtonActiveCallback = null;

    public void Initialize(Action onStartButtonActiveCallback)
    {
        _mapInfo.Initialize();

        if(onStartButtonActiveCallback != null)
        {
            _onStartButtonActiveCallback = onStartButtonActiveCallback;
        }

        _rT = this.GetComponent<RectTransform>();

        _mapTamplate.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string mapsPath = path + @"/Unity_StarCraft2/maps/";

        _textPath.text = mapsPath;

        DirectoryInfo dInfo = new DirectoryInfo(mapsPath);
        FileInfo[] infos = dInfo.GetFiles("*.USC2");

        this.gameObject.SetActive(true);

        GameManager.instance.toolManager.MoveX(_rT, -510f, 1f, false, () =>
        {
            for (int i = 0; i < infos.Length; i++)
            {
                MapTemplate tam = Instantiate(_mapTamplate, _templateParent);
                tam.Initialize(infos[i].Name, mapsPath + infos[i].Name, OpenMapInfo);
            }
        });
    }

    public void Close(Action onResult)
    {
        _mapInfo.Close();

        GameManager.instance.toolManager.MoveX(_rT, -1310f, 1f, false, () => 
        {
            this.gameObject.SetActive(false);

            onResult?.Invoke();

            DeleteTamplate();
        });
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

    private void OpenMapInfo(MapData data)
    {
        _mapInfo.Open(data);

        GameManager.instance.currentMapdata = data;
        _onStartButtonActiveCallback?.Invoke();
    }
}
