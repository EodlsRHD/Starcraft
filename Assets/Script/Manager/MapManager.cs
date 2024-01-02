using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Transform MapParant = null;

    private Action _onIsReadyCallback = null;

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

        GenerateMap();
    }

    private void GenerateMap()
    {

    }
}
