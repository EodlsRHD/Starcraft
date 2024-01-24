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

    [Header("Protoss")]

    [SerializeField]
    private List<ObjectData> _pBuilding = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _pUnits = new List<ObjectData>();

    [Header("Zerg")]

    [SerializeField]
    private List<ObjectData> _zBuilding = new List<ObjectData>();

    [SerializeField]
    private List<ObjectData> _zUnits = new List<ObjectData>();

    [Header("Custom")]

    [SerializeField]
    private List<CustomData> _customs = new List<CustomData>();

    [Header("UI")]

    [SerializeField]
    private Button _buttonUpload = null;

    private void Start()
    {
        _buttonUpload.onClick.AddListener(() =>
        {
            Debug.Log("Upload start");

            Debug.Log("CustomDatas...");

            ServerManager.instance.SetCustomDatas(_customs, (result) =>
            {
                if(result == false)
                {
                    Debug.Log("Upload false");
                    return;
                }

                Debug.Log("Done");

                Debug.Log("Terran Building...");

                ServerManager.instance.SetObjectDatas(_tBuilding, (result) =>
                {
                    if (result == false)
                    {
                        Debug.Log("Upload false");
                        return;
                    }

                    Debug.Log("Done");

                    Debug.Log("Terran Unit...");

                    ServerManager.instance.SetObjectDatas(_tUnits, (result) =>
                    {
                        if (result == false)
                        {
                            Debug.Log("Upload false");
                            return;
                        }

                        Debug.Log("Done");

                        Debug.Log("Zerg Building...");

                        ServerManager.instance.SetObjectDatas(_zBuilding, (result) =>
                        {
                            if (result == false)
                            {
                                Debug.Log("Upload false");
                                return;
                            }

                            Debug.Log("Done");

                            Debug.Log("Zerg Unit...");

                            ServerManager.instance.SetObjectDatas(_zUnits, (result) =>
                            {
                                if (result == false)
                                {
                                    Debug.Log("Upload false");
                                    return;
                                }

                                Debug.Log("Done");

                                Debug.Log("Protoss Building...");

                                ServerManager.instance.SetObjectDatas(_pBuilding, (result) =>
                                {
                                    if (result == false)
                                    {
                                        Debug.Log("Upload false");
                                        return;
                                    }

                                    Debug.Log("Done");

                                    Debug.Log("Protoss Unit...");

                                    ServerManager.instance.SetObjectDatas(_pUnits, (result) =>
                                    {
                                        if (result == false)
                                        {
                                            Debug.Log("Upload false");
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
