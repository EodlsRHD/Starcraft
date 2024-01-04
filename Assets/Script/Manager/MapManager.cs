using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Transform _mapParant = null;

    private Action _onIsReadyCallback = null;

    private static GameObject _objMap = null;

    private static List<Game_Resources> _resourcesList = new List<Game_Resources>();

    public static void SetMapObject(GameObject obj, List<Game_Resources> resourcesList)
    {
        _objMap = obj;
        _resourcesList = resourcesList;
        DontDestroyOnLoad(_objMap);
    }

    public void Initialize(Action onIsReadyCallback)
    {
        if(onIsReadyCallback != null)
        {
            _onIsReadyCallback = onIsReadyCallback;
        }

        if(GameManager.instance.TEST_MODE == true)
        {
            _onIsReadyCallback?.Invoke();
        }
    }

    public void InstantiateMap()
    {
        Debug.Log(_resourcesList.Count);

        _objMap.transform.SetParent(_mapParant);
        _objMap.transform.position = new Vector3(GameManager.instance.currentMapdata.mapSizeX * 0.5f, 0f, GameManager.instance.currentMapdata.mapSizeY * 0.5f);
    }

    private void OnDestroy()
    {
        Destroy(_objMap);
    }
}
