using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using UnityEngine.Networking;
using System;
using System.Text;

#region Data

public enum eObject
{
    Non = -1,
    Building,
    Unit,
    Resources
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
    public PlayerInfo[] members;

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

    public byte objType = 0;
    public byte raceType = 0;
    public byte unitType = 0;
    public byte unitSizeType = 0;
    public byte unitAttackType = 0;
    public byte farAndNeer = 0;

    public string name = string.Empty;
    public int productionCode = 0;

    public bool isAir = false;
    public int maxHp = 0;

    public bool hasShild = false;
    public int maxShild = 0;

    public bool hasEnergy = false;
    public int maxEnergy = 0;

    public bool hasAttack = false;
    public bool hasAirAttack = false;
    public int attack = 0;
    public float attackRate = 0f;
    public float attackRange = 0f;

    public int defence = 0;
    public float moveSpeed = 0f;

    public ObjectCustom custom = null;
    public ObjectMatadata metaData = null;
}

[System.Serializable]
public class ObjectCustom
{
    public bool hasCustom_1 = false;
    public int custom_1_key = 0;
    public string custom_1_id = string.Empty;

    public bool hasCustom_2 = false;
    public int custom_2_key = 0;
    public string custom_2_id = string.Empty;
    public bool hasCustom_3 = false;
    public int custom_3_key = 0;
    public string custom_3_id = string.Empty;

    public bool hasCustom_4 = false;
    public int custom_4_key = 0;
    public string custom_4_id = string.Empty;
}

[System.Serializable]
public class ObjectMatadata
{
    public int killCount = 0;

    public int HpKey = 0;
    public int attackKey = 0;
    public int defenceKey = 0;
    public int shildKey = 0;

    public int currentHp = 0;
    public int currentShild = 0;
    public int currentEnergy = 0;

    public int upgradeAttack = 0;
    public int upgradedDefence = 0;
    public int upgradeShild = 0;

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
public class CustomData
{
    public string _id = string.Empty;
    public int orderKey = 0; // Object has this key.

    public string name = string.Empty;
    public string description = string.Empty;
}

#endregion

public class ServerManager : ColyseusManager<ServerManager>
{
    private static ServerManager _instance;

    private string token = string.Empty;
    private string refreshToken = string.Empty;

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

    }

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

    public void GetCustomDatas(Action<List<CustomData>> onResult)
    {
        var req = new
        {

        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            custom = new List<CustomData>()
        };

        SendPostRequestAsync("objectData/getCustomDatas", req, (resultJson, resultCode) =>
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

    public void SetCustomDatas(List<CustomData> customDatas, Action<bool> onResult)
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

        SendPostRequestAsync("objectData/SetCustomDatas", req, (resultJson, resultCode) =>
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

    private void ShowLoading()
    {

    }

    private void HideLoading()
    {

    }
}
