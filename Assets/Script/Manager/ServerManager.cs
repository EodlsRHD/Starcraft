using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using UnityEngine.Networking;
using System;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

#region Data

public enum eObject
{
    Non = -1,
    Building,
    Unit,
    Resources,
    Research,
    Tool
}

public enum eRace
{
    Non = -1,
    Terran,
    Protoss,
    Zerg
}

public enum eUnitType
{
    Non = -1,
    Biological,
    Mechanical
}

public enum eUnitSizeType
{
    Non = -1,
    Large,
    Middle,
    Small
}

public enum eUnitAttackType
{
    Non = -1,
    Common,
    Concussive,
    Explosive
}

public enum eFarAndNeer
{
    Non = -1,
    Far,
    Neer
}

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
public class MapDataInfo
{
    public string _id = string.Empty;

    public string name = string.Empty;
    public string description = string.Empty;
    public string version = string.Empty;
    public string maker = string.Empty;
    public int maxPlayer = 0;

    public eClassification classification = eClassification.Non;
    public int teamCount = 0;

    public string roomHostUuid = string.Empty;
    public string[] members;

    public string thumbnailPath = string.Empty;

    public int mapSizeX = 0;
    public int mapSizeY = 0;

    public string fileDownloadUrl = string.Empty;
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

    public string roomHostUuid = string.Empty;
    public string[] memberIDs;

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

[System.Serializable]
public class ObjectData
{
    public string _id = string.Empty;
    public int key = 0;

    [Header("Type")]
    [Space(10)]

    [Tooltip("0 : Building, 1 : Unit, 2 : Resources, 3 : Research, 4 : Tool")]
    public byte objType = 0;

    [Tooltip("0 : Terran, 1 : Protoss, 2 : Zerg")]
    public byte raceType = 0;

    [Tooltip("0 : Biological, 1 : Mechanical")]
    public byte unitType = 0;

    [Tooltip("0 : Large, 1 : Middle, 2 : Small")]
    public byte unitSizeType = 0;

    [Tooltip("0 : Common, 1 : Concussive, 2 : Explosive")]
    public byte unitAttackType = 0;

    [Tooltip("0 : Common, 1 : Concussive, 2 : Explosive")]
    public byte unitAirAttackType = 0;

    [Tooltip("0 : Far, 1 : Neer")]
    public byte farAndNeer = 0;

    [Space(10)]

    public string name = string.Empty;
    public int productionCode = 0;
    public int productionTime = 0;

    [Space(10)]

    public int mineral = 0;
    public int gas = 0;

    [Tooltip("building is offer")]
    public int population = 0;

    [Space(10)]

    public bool isAir = false;
    public int maxHp = 0;

    [Space(10)]

    public bool hasShild = false;
    public int maxShild = 0;

    [Space(10)]

    public bool hasEnergy = false;

    [Tooltip("When this object has no energy, use it as a common value.")]
    public int maxEnergy = 0;

    [Space(10)]

    public bool hasAttack = false;
    public int attack = 0;
    public float attackRate = 0f;
    public float attackRange = 0f;

    [Space(10)]

    public bool hasAirAttack = false;
    public int airAttack = 0;
    public float airAttackRate = 0f;
    public float airAttackRange = 0f;

    [Space(10)]

    public bool isConcealment = false;
    public int sight = 0;
    public bool hasDetector = false;
    public int defence = 0;
    public float moveSpeed = 0f;

    [Space(10)]

    public ObjectCustom custom = null;
    public ObjectMetadata metaData = null;
}

[System.Serializable]
public class ObjectCustom
{
    public bool useMove = false;
    public int move_key = 0;
    public string move_id = string.Empty;

    [Space(10)]

    public bool useStop = false;
    public int stop_key = 0;
    public string stop_id = string.Empty;

    [Space(10)]

    public bool useAttack = false;
    public int attack_key = 0;
    public string attack_id = string.Empty;

    [Space(10)]

    public bool useHold = false;
    public int hold_key = 0;
    public string hold_id = string.Empty;

    [Space(10)]

    public bool usePatrol = false;
    public int patrol_key = 0;
    public string patrol_id = string.Empty;

    [Space(10)]

    public bool hasCustom_1 = false;
    public int custom_1_key = 0;
    public string custom_1_id = string.Empty;

    [Space(10)]

    public bool hasCustom_2 = false;
    public int custom_2_key = 0;
    public string custom_2_id = string.Empty;

    [Space(10)]

    public bool hasCustom_3 = false;
    public int custom_3_key = 0;
    public string custom_3_id = string.Empty;

    [Space(10)]

    public bool hasCustom_4 = false;
    public int custom_4_key = 0;
    public string custom_4_id = string.Empty;
}

[System.Serializable]
public class ObjectMetadata
{
    public int killCount = 0;

    [Space(10)]

    public string hpName = string.Empty;
    public string attackName = string.Empty;
    public string airAttackName = string.Empty;
    public string defenceName = string.Empty;
    public string shildName = string.Empty;

    [Space(10)]

    public int HpKey = 0;
    public int attackKey = 0;
    public int airAttackKey = 0;
    public int defenceKey = 0;
    public int shildKey = 0;

    [Space(10)]

    public int currentHp = 0;
    public int currentShild = 0;
    public int currentEnergy = 0;

    [Space(10)]

    public int upgradeAttack = 0;
    public int upgradeAirAttack = 0;
    public int upgradedDefence = 0;
    public int upgradeShild = 0;

    [Space(10)]

    public bool isProduction = false;
    public List<string> productionUnitIDs = null;
}

[System.Serializable]
public class PlayerInfo
{
    public string _id = string.Empty;

    public string userID = string.Empty;
    public string userPW = string.Empty;

    public string nickName = string.Empty;
    public byte brood = 0;

    public int team = -1;
    public byte color = 0;

    public float x = 0;
    public float z = 0;

    public int win = 0;
    public int lose = 0;
}

[System.Serializable]
public class ObjectDataInfo
{
    public string _id = string.Empty;
    public int objectDataID = 0; // ObjectData ID

    public string name = string.Empty;
    public string description = string.Empty;
    public string useCondition = string.Empty;

    public int mineral = 0;
    public int gas = 0;
    public int productionTime = 0;
    public int energy = 0;
    public int population = 0;
}

public partial class MyRoomState : Colyseus.Schema.Schema
{
    [Colyseus.Schema.Type(0, "number")]
    public float mapID = default(float);

    [Colyseus.Schema.Type(1, "string")]
    public string ownerID = default(string);
}
#endregion

public class ServerManager : ColyseusManager<ServerManager>
{
    [System.Serializable]
    private class OnJoinResult
    {
        public Colyseus.ColyseusMatchMakeResponse seatReservation = null;
    }

    private static ServerManager _instance;

    private string token = string.Empty;
    private string refreshToken = string.Empty;

    private readonly string _mapsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/Unity_StarCraft2/maps";

    public static ServerManager instance
    {
        get 
        { 
            if(_instance == null)
            {
                _instance = new ServerManager();
            }

            return _instance;
        }
    }

    protected override void Start()
    {
        base.Start();

        _instance = this;

        Initialize();
    }

    public void Initialize()
    {
        base.InitializeClient();
        this._colyseusSettings.useSecureProtocol = false;
    }

    #region PlayerInfo

    public void CreatePlayerInfo(string ID, string PW, string nickName, Action<PlayerInfo> onResult)
    {
        var req = new
        {
            userID = ID,
            userPW = PW,
            nickName = nickName
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            playerInfo = new PlayerInfo()
        };

        SendPostRequestAsync("user/signUp", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == 0)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.playerInfo);
        });
    }

    public void SignInPlayerInfo(string ID, string PW, Action<bool> onResult)
    {
        var req = new
        {
            userID = ID,
            userPW = PW
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            isFound = false
        };

        SendPostRequestAsync("user/signIn", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(false);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(false);
                return;
            }

            onResult?.Invoke(result.isFound);
        });
    }

    public void GetPlayerInfo(string ID, string PW, Action<PlayerInfo> onResult)
    {
        var req = new
        {
            userID = ID,
            userPW = PW
        };

        var response = new
        {
            resultCode = 0,
            message = string.Empty,
            playerInfo = new PlayerInfo()
        };

        SendPostRequestAsync("user/getPlayerInfo", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, response);

            if (result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.playerInfo);
        });
    }

    public void GetPlayerInfo(string _id, Action<PlayerInfo> onResult)
    {
        var req = new
        {
            _id = _id
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            playerInfo = new PlayerInfo()
        };

        SendPostRequestAsync("user/getPlayerInfo_ObjectID", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.playerInfo);
        });
    }

    public void UpdatePlayerInfo(string _id, PlayerInfo newInfo, Action<PlayerInfo> onResult)
    {
        var req = new
        {
            _id = _id,
            newInfo = newInfo
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            playerInfo = new PlayerInfo()
        };

        SendPostRequestAsync("user/updatePlayerInfo", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if(result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.playerInfo);
        });
    }

    #endregion

    #region Data

    public void GetObjectDatas(Action<List<ObjectData>> onResult)
    {
        var req = new
        {

        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            objectDatas = new List<ObjectData>()
        };

        SendPostRequestAsync("objectData/getObjectDatas", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.objectDatas);
        });
    }

    public void SetObjectDatas(List<ObjectData> objectDatas, Action<bool> onResult)
    {
        var req = new
        {
            objectDatas = objectDatas
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty
        };

        SendPostRequestAsync("objectData/setObjectDatas", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(false);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(false);
                return;
            } 

            onResult?.Invoke(true);
        });
    }

    public void GetObjectDataInfos(Action<List<ObjectDataInfo>> onResult)
    {
        var req = new
        {

        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            custom = new List<ObjectDataInfo>()
        };

        SendPostRequestAsync("objectData/getObjectDataInfos", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.custom);
        });
    }

    public void SetObjectDataInfos(List<ObjectDataInfo> customDatas, Action<bool> onResult)
    {
        var req = new
        {
            customDatas = customDatas
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty
        };

        SendPostRequestAsync("objectData/setObjectDataInfos", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(false);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(false);
                return;
            }

            onResult?.Invoke(true);
        });
    }

    #endregion

    #region Room

    public void JoinOrCreateRoom(MapData data, Action onResult = null)
    {
        GetPlayerInfo(data.roomHostUuid, (result) => 
        {
            RequestJoinRoom((int)data.id, result.nickName, false, data.roomHostUuid, () => 
            {
                onResult?.Invoke();
            });
        });
    }

    private void RequestJoinRoom(int mapID, string nickName, bool isPrivate = false, string roomOwner = "", Action onRoomJoinDone = null)
    {
        Debug.Log("Join Room mapID : " + mapID);

        SendPostRequestAsync("room/join", new Dictionary<string, object>() {
            { "mapID", mapID },
            { "isPrivate", isPrivate },
            { "name", name },
            { "roomOwner", roomOwner },
        }, async (resultJson, resultCode) =>
        {
            Debug.Log(resultJson);
            OnJoinResult res = JsonUtility.FromJson<OnJoinResult>(resultJson);

            Debug.Log(res.seatReservation.room.roomId);
            var roomReserv = await client.ConsumeSeatReservation<MyRoomState>(res.seatReservation, _colyseusSettings.HeadersDictionary);
            RoomServerManager.Instance.Initialize(roomReserv);

            onRoomJoinDone();
        });
    }

    public void GetMapDataInfos(Action<List<MapDataInfo>> onResult)
    {
        var req = new
        {
            
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            mapDatsInfoa = new List<MapDataInfo>()
        };

        SendPostRequestAsync("room/getMapDataInfos", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(null);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(result.mapDatsInfoa);
        });
    }

    public void EnterMapDataInfo(string mapID, Action<bool> onResult)
    {
        var req = new
        {
            mapID = mapID
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty
        };

        SendPostRequestAsync("room/enterMapDataInfo", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(false);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(false);
                return;
            }

            onResult?.Invoke(true);
        });
    }

    public void AddMapDataInfo(MapDataInfo info, Action<bool> onResult)
    {
        var req = new
        {
            info = info
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            mapData = new MapData()
        };

        SendPostRequestAsync("room/addMapDataInfo", req, (resultJson, resultCode) =>
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(resultJson, res);

            if (result == null)
            {
                onResult?.Invoke(false);
                return;
            }

            if (result.resultCode == -1)
            {
                onResult?.Invoke(false);
                return;
            }

            onResult?.Invoke(true);
        });
    }

    #endregion

    private async void SendPostRequestAsync(string endpoint, object body, Action<string, long> onResult)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
        Debug.Log(json);
        //byte[] bytes = Encoding.UTF8.GetBytes(json);
        using (UnityWebRequest request = UnityWebRequest.Post(_colyseusSettings.WebRequestEndpoint + endpoint, json))
        {
            request.disposeUploadHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeCertificateHandlerOnDispose = true;

            request.SetRequestHeader("Content-Type", "application/json");
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            request.uploadHandler.Dispose();
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.SetRequestHeader("token", this.token);
            request.SetRequestHeader("refreshtoken", this.refreshToken);

            float time = Time.realtimeSinceStartup;
            Debug.Log("send WebRequest To " + _colyseusSettings.WebRequestEndpoint + endpoint);

            ShowLoading();
            await request.SendWebRequest();
            HideLoading();
            Debug.Log("Recive webrequest from " + _colyseusSettings.WebRequestEndpoint + endpoint + " Time used : " + (Time.realtimeSinceStartup - time));
            string result = request.downloadHandler.text;
            //Debug.Log(result);

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("connection Error! : " + endpoint + "        result : " + request.result);
                onResult("", request.responseCode);

                //request.Dispose();
                return;
            }

            string newrefreshToken = request.GetResponseHeader("new-refresh-token");
            string newToken = request.GetResponseHeader("new-token");

            if (string.IsNullOrEmpty(newrefreshToken) == false)
            {
                Debug.Log("newRefreshToken : " + newrefreshToken);
                refreshToken = newrefreshToken;
            }

            if (string.IsNullOrEmpty(newToken) == false)
            {
                Debug.Log("newToken : " + newToken);
                token = newToken;
            }

            onResult(result, request.responseCode);
            request.Dispose();
            result = null;
        }
    }

    private async void SendPostRequestFormData(string endpoint, string filename, object body, byte[] bytes, Action<string, long> onResult)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (bytes != null)
        {
            formData.Add(new MultipartFormFileSection("file", bytes, filename, "image"));
        }

        string json = String.Empty;
        if (body != null)
        {
            json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            formData.Add(new MultipartFormDataSection("bodyJson", json, "application/json"));
        }

        using (UnityWebRequest request = UnityWebRequest.Post(_colyseusSettings.WebRequestEndpoint + endpoint, json))
        {
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeCertificateHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;

            byte[] boundary = UnityWebRequest.GenerateBoundary();
            byte[] formSection = UnityWebRequest.SerializeFormSections(formData, boundary);
            //byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

            //byte[] bodyResult = new byte[formSection.Length + jsonToSend.Length];
            //Buffer.BlockCopy(formSection, 0, bodyResult, 0, formSection.Length);
            //Buffer.BlockCopy(jsonToSend, 0, bodyResult, formSection.Length, jsonToSend.Length);

            request.uploadHandler.Dispose();

            UploadHandler uploader = new UploadHandlerRaw(formSection);
            uploader.contentType = String.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary));

            request.uploadHandler = uploader;
            request.SetRequestHeader("token", this.token);
            request.SetRequestHeader("refreshtoken", this.refreshToken);

            float time = Time.realtimeSinceStartup;
            Debug.Log("send WebRequest To " + _colyseusSettings.WebRequestEndpoint + endpoint);

            ShowLoading();
            await request.SendWebRequest();
            HideLoading();

            boundary = UnityWebRequest.GenerateBoundary();
            formSection = UnityWebRequest.SerializeFormSections(formData, boundary);

            Debug.Log("Recive webrequest from " + _colyseusSettings.WebRequestEndpoint + endpoint + " Time used : " + (Time.realtimeSinceStartup - time));
            string result = request.downloadHandler.text;
            //Debug.Log(result);

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("connection Error! : " + endpoint + "        result : " + request.result);
                onResult("", request.responseCode);

                //request.Dispose();
                return;
            }

            string newrefreshToken = request.GetResponseHeader("new-refresh-token");
            string newToken = request.GetResponseHeader("new-token");

            if (string.IsNullOrEmpty(newrefreshToken) == false)
            {
                Debug.Log("newRefreshToken : " + newrefreshToken);
                refreshToken = newrefreshToken;
            }

            if (string.IsNullOrEmpty(newToken) == false)
            {
                Debug.Log("newToken : " + newToken);
                token = newToken;
            }

            onResult(result, request.responseCode);
            request.Dispose();
        }

    }

    public async void ImageDownload(string url, Action<Texture, bool> onResult)
    {
        if (url.Length == 0)
        {
            onResult?.Invoke(null, false);
            return;
        }

        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            float time = Time.realtimeSinceStartup;
            await req.SendWebRequest();
            Debug.Log("Time used : " + (Time.realtimeSinceStartup - time));

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("ImageDownload ConnectionError");
                onResult?.Invoke(null, false);
                return;
            }

            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("ImageDownload ProtocolError");
                onResult?.Invoke(null, false);
                return;
            }

            onResult?.Invoke(((DownloadHandlerTexture)req.downloadHandler).texture, true);
        }
    }

    public async void FildDownload(string url, Action<string, bool> onResult)
    {
        if (url.Length == 0)
        {
            onResult?.Invoke(string.Empty, false);
            return;
        }

        string downloadPath = string.Empty;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            float time = Time.realtimeSinceStartup;
            await req.SendWebRequest();
            Debug.Log("Time used : " + (Time.realtimeSinceStartup - time));

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("FildDownload ConnectionError");
                onResult?.Invoke(string.Empty, false);
                return;
            }

            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("FildDownload ProtocolError");
                onResult?.Invoke(string.Empty, false);
                return;
            }

            File.WriteAllBytes(_mapsPath, req.downloadHandler.data);
        }
    }

    private void ShowLoading()
    {

    }

    private void HideLoading()
    {

    }
}

public class GenericAwaiter<T> : INotifyCompletion where T : AsyncOperation
{
    private T asyncOperation;
    private Action continuation;

    public GenericAwaiter(T asyncOp)
    {
        this.asyncOperation = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted { get { return asyncOperation.isDone; } }

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    private void OnRequestCompleted(AsyncOperation obj)
    {
        continuation();
        asyncOperation = null;
    }
}

public static class ExtensionMethods
{
    public static GenericAwaiter<UnityWebRequestAsyncOperation> GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new GenericAwaiter<UnityWebRequestAsyncOperation>(asyncOp);
    }
}
