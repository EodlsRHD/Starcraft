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

    private Action<IObserver> _onUpdateInformationCallback = null;

    public static void SetMapObject(GameObject obj, List<Node> resourcesList, List<Node> startPositions)
    {
        _objMap = obj;
        _resources = resourcesList;
        _startPositions = startPositions;
        DontDestroyOnLoad(_objMap);
    }

    public void Initialize(Action onIsReadyCallback, Action<IObserver> onUpdateInformationCallback)
    {
        if(onIsReadyCallback != null)
        {
            _onIsReadyCallback = onIsReadyCallback;
        }

        if(onUpdateInformationCallback != null)
        {
            _onUpdateInformationCallback = onUpdateInformationCallback;
        }
    }

    public void SetUp()
    {
        Map();
        Resources();
        Player();
        SetFirstObject();

        _onIsReadyCallback?.Invoke();
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

                                    InstantiateResources(ref request, ref node);
                                });
                            }
                            else
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Mineral, (request) =>
                                {
                                    InstantiateResources(ref request, ref node);
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

                                    InstantiateResources(ref request, ref node);
                                });
                            }
                            else
                            {
                                GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Gas, (request) =>
                                {
                                    InstantiateResources(ref request, ref node);
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
        PlayerInfo[] players = GameManager.instance.currentMapdata.members;

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

            players[i].x = node.x;
            players[i].z = node.y;
            players[i].color = (byte)node.startPosition.playerColor;
            players[i].team = node.startPosition.team;

            if(players[i].ID.Equals(GameManager.instance.playerInfo.ID, StringComparison.Ordinal))
            {
                InGameManager.instance.cameraArm = new Vector2(players[i].x, players[i].z);
                GameManager.instance.UpdatePlayerInfo(players[i]);
            }
        }
    }

    private void SetFirstObject()
    {
        PlayerInfo[] players = GameManager.instance.currentMapdata.members;

        for (int i = 0; i < players.Length; i++)
        {
            if (_startPositions.Count <= i)
            {
                break;
            }

            if (players[i] == null)
            {
                continue;
            }

            Node node = _startPositions[i];

            switch((eRace)players[i].brood)
            {
                case eRace.Terran:
                    {
                        if (GameManager.instance.poolMemory.CommandCenter == 0)
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, players[i], "CommandCenter", (request) => 
                            {
                                GameManager.instance.poolMemory.CommandCenter = request.key;

                                InstantiateObject(ref request, ref node, ref GameManager.instance.poolMemory.CommandCenter, GameManager.instance.tagMemory.building);
                            });
                        }
                        else
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.CommandCenter, (request) =>
                            {
                                InstantiateObject(ref request, ref node, ref GameManager.instance.poolMemory.CommandCenter, GameManager.instance.tagMemory.building);
                            });
                        }
                    }
                    break;

                case eRace.Protoss:
                    {
                        if (GameManager.instance.poolMemory.Nexus == 0)
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, "Nexus", (request) =>
                            {
                                GameManager.instance.poolMemory.Nexus = request.key;

                                InstantiateObject(ref request, ref node, ref GameManager.instance.poolMemory.Nexus, GameManager.instance.tagMemory.building);
                            });
                        }
                        else
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Nexus, (request) =>
                            {
                                InstantiateObject(ref request, ref node, ref GameManager.instance.poolMemory.Nexus, GameManager.instance.tagMemory.building);
                            });
                        }
                    }
                    break;

                case eRace.Zerg:
                    {
                        if (GameManager.instance.poolMemory.Hatchery == 0)
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, "Hatchery", (request) =>
                            {
                                GameManager.instance.poolMemory.Hatchery = request.key;

                                InstantiateObject(ref request, ref node, ref GameManager.instance.poolMemory.Hatchery, GameManager.instance.tagMemory.building);
                            });
                        }
                        else
                        {
                            GameManager.instance.toolManager.RequestPool(ePoolType.Prefab, GameManager.instance.poolMemory.Hatchery, (request) =>
                            {
                                InstantiateObject(ref request, ref node, ref GameManager.instance.poolMemory.Hatchery, GameManager.instance.tagMemory.building);
                            });
                        }
                    }
                    break;
            }
        }
    }

    private void InstantiateResources(ref RequestPool request, ref Node node)
    {
        GameObject obj = request.GetObject();
        obj.transform.position = new Vector3(node.x, node.topographic.height + (obj.transform.localScale.y * 0.5f), node.y);
        obj.tag = GameManager.instance.tagMemory.resources;

        if (obj.TryGetComponent(out Game_Resources component) == false)
        {
            component = obj.AddComponent<Game_Resources>();
        }

        component.Initialize(node.resource);
    }

    private void InstantiateObject(ref RequestPool request, ref Node node, ref int key, string tag)
    {
        GameObject obj = request.GetObject();
        obj.transform.position = new Vector3(node.x, node.topographic.height + (obj.transform.localScale.y * 0.5f), node.y);
        obj.tag = tag;

        var tamplate = obj.GetComponent<ObjectTamplate>();

        GameManager.instance.toolManager.RequestData(key, (requst) =>
        {
            tamplate.Initialize(requst.GetData(), _onUpdateInformationCallback);
        });

        InGameManager.instance.Add(eObject.Building, tamplate);
    }

    private void OnDestroy()
    {
        Destroy(_objMap);
    }
}