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

    private static List<Node> _resources = new List<Node>();
    private static List<Node> _startPositions = new List<Node>();

    public static void SetMapObject(GameObject obj, List<Node> resourcesList, List<Node> startPositions)
    {
        _objMap = obj;
        _resources = resourcesList;
        _startPositions = startPositions;
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
        Map();
        Resources();
        Player();
    }

    private void Map()
    {
        _objMap.transform.SetParent(_mapParant);
        _objMap.transform.position = new Vector3(GameManager.instance.currentMapdata.mapSizeX * 0.5f, 0f, GameManager.instance.currentMapdata.mapSizeY * 0.5f);
    }

    private void Resources()
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            Node node = _resources[i];

            if (node.startPosition.playerColor != ePlayerColor.Non)
            {

            }

            if (node.resource.type != eResourceType.Non)
            {
                switch (node.resource.type)
                {
                    case eResourceType.Mineral:
                        {
                            if (GameManager.instance.poolMemory.Mineral == 0)
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, "Mineral", (request) =>
                                {
                                    GameManager.instance.poolMemory.Mineral = request.key;

                                    GameObject obj = request.GetObject();
                                    obj.transform.position = new Vector3(node.x, node.topographic.height, node.y);
                                });
                            }
                            else
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Mineral, (request) =>
                                {
                                    GameObject obj = request.GetObject();
                                    obj.transform.position = new Vector3(node.x, node.topographic.height, node.y);
                                });
                            }
                        }
                        break;

                    case eResourceType.Gas:
                        {
                            if (GameManager.instance.poolMemory.Mineral == 0)
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, "Gas", (request) =>
                                {
                                    GameManager.instance.poolMemory.Gas = request.key;

                                    GameObject obj = request.GetObject();
                                    obj.transform.position = new Vector3(node.x, node.topographic.height, node.y);
                                });
                            }
                            else
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Gas, (request) =>
                                {
                                    GameObject obj = request.GetObject();
                                    obj.transform.position = new Vector3(node.x, node.topographic.height, node.y);
                                });
                            }
                        }
                        break;
                }
            }
        }
    }

    private void Player()
    {
        MapData mapData = GameManager.instance.currentMapdata;

        for (int i = 0; i < _startPositions.Count; i++)
        {
            Node node = _startPositions[i];
        }
    }

    private void OnDestroy()
    {
        Destroy(_objMap);
    }
}
