using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using TMPro;

#region Mapdata

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

public enum eClassification
{
    Non = -1,
    HomeAndAway,
    Solo
}

[System.Serializable]
public class MapData
{
    public long id = 0;

    public string name = string.Empty;
    public string description = string.Empty;
    public string version = string.Empty;
    public string maker = string.Empty;
    public int maxPlayer = 0;

    public eClassification classification = eClassification.Non;
    public int teamCount = 0;

    public string roomHostID = string.Empty;
    public PlayerInfo[] members;

    public string thumbnailPath = string.Empty;

    public int mapSizeX = 0;
    public int mapSizeY = 0;

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
    public int team = -1;
    public ePlayerColor playerColor = ePlayerColor.Non;
}

[System.Serializable]
public class Resource
{
    public eResourceType type = eResourceType.Non;
    public int quantity = 0;
}

#endregion

public class MapList : MonoBehaviour
{
    [SerializeField]
    private MapTemplate _mapTamplate = null;

    [SerializeField]
    private Transform _templateParent = null;

    [SerializeField]
    private TMP_Text _textPath = null;

    [SerializeField]
    private Button _buttonClose = null;

    private RectTransform _rT = null;

    private Action<MapData> _onOpenMapInfo = null;

    private List<MapTemplate> _tamplates = new List<MapTemplate>();

    public void Initialize(Action<MapData> onOpenMapInfo, Action onCloseCallback)
    {
        if(onOpenMapInfo != null)
        {
            _onOpenMapInfo = onOpenMapInfo;
        }

        _buttonClose.onClick.AddListener(() => { onCloseCallback?.Invoke(); });

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

                _tamplates.Add(tam);
            }
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
        CloseInfo();

        _onOpenMapInfo?.Invoke(data);
    }

    private void CloseInfo()
    {
        foreach (var tam in _tamplates)
        {
            tam.CloseInfo();
        }
    }
}
