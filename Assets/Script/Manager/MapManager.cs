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

    public static void SetMapObject(GameObject obj)
    {
        _objMap = obj;
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
        _objMap.transform.SetParent(_mapParant);
        _objMap.transform.position = new Vector3(GameManager.instance.currentMapdata.mapSizeX * 0.5f, 0f, GameManager.instance.currentMapdata.mapSizeY * 0.5f);
    }

    private void OnDestroy()
    {
        Destroy(_objMap);
    }
}
