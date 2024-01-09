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
        _objMap.transform.position = new Vector3(GameManager.instance.currentMapdata.mapSizeX * 0.5f - 0.5f, 0f, GameManager.instance.currentMapdata.mapSizeY * 0.5f - 0.5f);
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

                                    if(obj.TryGetComponent(out Game_Resources component) == false)
                                    {
                                        component = obj.AddComponent<Game_Resources>();
                                    }

                                    component.Initialize(node.resource);
                                });
                            }
                            else
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Mineral, (request) =>
                                {
                                    GameObject obj = request.GetObject();
                                    obj.transform.position = new Vector3(node.x, node.topographic.height, node.y);

                                    if (obj.TryGetComponent(out Game_Resources component) == false)
                                    {
                                        component = obj.AddComponent<Game_Resources>();
                                    }

                                    component.Initialize(node.resource);
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

                                    if (obj.TryGetComponent(out Game_Resources component) == false)
                                    {
                                        component = obj.AddComponent<Game_Resources>();
                                    }

                                    component.Initialize(node.resource);
                                });
                            }
                            else
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Gas, (request) =>
                                {
                                    GameObject obj = request.GetObject();
                                    obj.transform.position = new Vector3(node.x, node.topographic.height, node.y);

                                    if (obj.TryGetComponent(out Game_Resources component) == false)
                                    {
                                        component = obj.AddComponent<Game_Resources>();
                                    }

                                    component.Initialize(node.resource);
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
        PlayerInfo[] players = mapData.members;

        for (int i = 0; i < players.Length; i++)
        {
            if(_startPositions.Count <= i)
            {
                break;
            }

            if(players[i] == null)
            {
                continue;
            }

            Node node = _startPositions[i];

            Debug.LogError(node.x + "  a  " + node.y);

            players[i].x = node.x;
            players[i].z = node.y;
            players[i].color = node.startPosition.playerColor;
            players[i].team = node.startPosition.team;

            if(players[i].id.Equals(mapData.roomHostID, StringComparison.Ordinal))
            {
                InGameManager.instance.cameraArm = new Vector2(players[i].x, players[i].z);
            }
        }


    }

    private void OnDestroy()
    {
        Destroy(_objMap);
    }
}
