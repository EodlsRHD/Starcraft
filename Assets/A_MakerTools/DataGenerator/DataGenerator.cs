using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataGenerator : MonoBehaviour
{
    [Header("Terran")]

    [SerializeField]
    private List<ObjectData> _tBuilding = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _tUnits = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _tResearchAndTools = new List<ObjectData>();

    [Header("Protoss")]

    [SerializeField]
    private List<ObjectData> _pBuilding = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _pUnits = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _pResearchAndTools = new List<ObjectData>();

    [Header("Zerg")]

    [SerializeField]
    private List<ObjectData> _zBuilding = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _zUnits = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _zResearchAndTools = new List<ObjectData>();

    [Header("ObjectDataInfo")]

    [SerializeField]
    private List<ObjectDataInfo> _objectDataInfos = new List<ObjectDataInfo>();

    [SerializeField]
    private List<ObjectDataInfo> _researchAndToolInfos = new List<ObjectDataInfo>();

    [Header("UI")]

    [SerializeField]
    private Button _buttonObjectDataUpload = null;

    [SerializeField]
    private Button _buttonObjectDataInfoUpdate = null;

    private void Start()
    {
        _buttonObjectDataInfoUpdate.onClick.AddListener(() =>
        {
            Debug.Log("Upload start");

            Debug.Log("Research And Tool Info...");

            ServerManager.instance.SetObjectDataInfos(_researchAndToolInfos, (result) =>
            {
                if (result == false)
                {
                    Debug.Log("Research And Tool Info Upload false");
                    return;
                }

                Debug.Log("Terran Research and Tool...");

                Debug.Log("Done...");

                ServerManager.instance.SetObjectDatas(_tResearchAndTools, (result) =>
                {
                    if (result == false)
                    {
                        Debug.Log("Research And Tool Info Upload false");
                        return;
                    }

                    Debug.Log("Done...");

                    Debug.Log("Protoss Research and Tool...");

                    ServerManager.instance.SetObjectDatas(_pResearchAndTools, (result) =>
                    {
                        if (result == false)
                        {
                            Debug.Log("Research And Tool Info Upload false");
                            return;
                        }

                        Debug.Log("Done...");

                        Debug.Log("Zerg Research and Tool...");

                        ServerManager.instance.SetObjectDatas(_zResearchAndTools, (result) =>
                        {
                            if (result == false)
                            {
                                Debug.Log("Research And Tool Info Upload false");
                                return;
                            }

                            Debug.Log("Done...");

                            Debug.Log("All Done...");
                        });
                    });
                });
            });
        });

        _buttonObjectDataUpload.onClick.AddListener(() =>
        {
            Debug.Log("Upload start");

            Debug.Log("CustomDatas...");

            ServerManager.instance.SetObjectDataInfos(_objectDataInfos, (result) =>
            {
                if(result == false)
                {
                    Debug.Log("CustomDatas Upload false");
                    return;
                }

                Debug.Log("Done");

                Debug.Log("Terran Building...");

                ServerManager.instance.SetObjectDatas(_tBuilding, (result) =>
                {
                    if (result == false)
                    {
                        Debug.Log("Terran Building Upload false");
                        return;
                    }

                    Debug.Log("Done");

                    Debug.Log("Terran Unit...");

                    ServerManager.instance.SetObjectDatas(_tUnits, (result) =>
                    {
                        if (result == false)
                        {
                            Debug.Log("Terran Unit Upload false");
                            return;
                        }

                        Debug.Log("Done");

                        Debug.Log("Zerg Building...");

                        ServerManager.instance.SetObjectDatas(_zBuilding, (result) =>
                        {
                            if (result == false)
                            {
                                Debug.Log("Zerg Building Upload false");
                                return;
                            }

                            Debug.Log("Done");

                            Debug.Log("Zerg Unit...");

                            ServerManager.instance.SetObjectDatas(_zUnits, (result) =>
                            {
                                if (result == false)
                                {
                                    Debug.Log("Zerg Unit Upload false");
                                    return;
                                }

                                Debug.Log("Done");

                                Debug.Log("Protoss Building...");

                                ServerManager.instance.SetObjectDatas(_pBuilding, (result) =>
                                {
                                    if (result == false)
                                    {
                                        Debug.Log("Protoss BuildingUpload false");
                                        return;
                                    }

                                    Debug.Log("Done");

                                    Debug.Log("Protoss Unit...");

                                    ServerManager.instance.SetObjectDatas(_pUnits, (result) =>
                                    {
                                        if (result == false)
                                        {
                                            Debug.Log("Protoss Unit Upload false");
                                            return;
                                        }

                                        Debug.Log("Done...");

                                        Debug.Log("All Done...");
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }
}
