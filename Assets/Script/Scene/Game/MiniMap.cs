using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    private Camera _miniMapCamera = null;

    [SerializeField]
    private Camera _fogCamera = null;

    public void Initialize()
    {
        float x = GameManager.instance.currentMapdata.mapSizeX * 0.5f;
        float y = GameManager.instance.currentMapdata.mapSizeY * 0.5f;

        Debug.LogErrorFormat("mapSize   : {0}, {0}", x, y);

        if(x >= y)
        {
            SetSize(y);
        }
        else if(x < y)
        {
            SetSize(x);
        }

        this.gameObject.SetActive(true);
    }

    private void SetSize(float size)
    {
        _miniMapCamera.orthographicSize = size - 0.5f;
        _fogCamera.orthographicSize = size - 0.5f;

        Vector3 pos = new Vector3(size - 0.5f, 100f, size - 0.5f);

        _miniMapCamera.transform.position = pos;
        _fogCamera.transform.position = pos;
    }
}
