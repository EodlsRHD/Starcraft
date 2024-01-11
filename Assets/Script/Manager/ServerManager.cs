using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using UnityEngine.Networking;
using System;
using System.Text;
using Cysharp.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

#region data

[System.Serializable]
public class ObjectData
{
    public ObjectId _id { get; set; }
    public int key { get; set; }

    public byte objType { get; set; }
    public byte raceType { get; set; }
    public byte unitType { get; set; }
    public byte unitSizeType { get; set; }
    public byte unitAttackType { get; set; }
    public byte farAndNeer { get; set; }

    public string name { get; set; }
    public int productionCode { get; set; }

    public bool isAir { get; set; }
    public int maxHp { get; set; }

    public bool hasShild { get; set; }
    public int maxShild { get; set; }

    public bool hasEnergy { get; set; }
    public int maxEnergy { get; set; }

    public bool hasAttack { get; set; }
    public bool hasAirAttack { get; set; }
    public int attack { get; set; }
    public float attackRate { get; set; }
    public float attackRange { get; set; }

    public int defence { get; set; }
    public float moveSpeed { get; set; }

    public ObjectCustom custom { get; set; }
    public ObjectMatadata metaData { get; set; }
}

[System.Serializable]
public class ObjectCustom
{
    public bool hasCustom_1 { get; set; }
    public int custom_1_key { get; set; }
    public string custom_1_id { get; set; }

    public bool hasCustom_2 { get; set; }
    public int custom_2_key { get; set; }
    public string custom_2_id { get; set; }

    public bool hasCustom_3 { get; set; }
    public int custom_3_key { get; set; }
    public string custom_3_id { get; set; }

    public bool hasCustom_4 { get; set; }
    public int custom_4_key { get; set; }
    public string custom_4_id { get; set; }
}

[System.Serializable]
public class ObjectMatadata
{
    public int killCount { get; set; }

    public int HpKey { get; set; }
    public int attackKey { get; set; }
    public int defenceKey { get; set; }
    public int shildKey { get; set; }

    public int currentHp { get; set; }
    public int currentShild { get; set; }
    public int currentEnergy { get; set; }

    public int upgradeAttack { get; set; }
    public int upgradedDefence { get; set; }
    public int upgradeShild { get; set; }

    public bool isProduction { get; set; }
    public List<string> productionUnitIDs { get; set; }
}

[System.Serializable]
public class PlayerInfo
{
    public ObjectId _id { get; set; }

    public string ID { get; set; }
    public string PW { get; set; }

    public string nickName { get; set; }
    public byte brood { get; set; }

    public int team { get; set; }
    public byte color { get; set; }

    public float x { get; set; }
    public float z { get; set; }

    public int win { get; set; }
    public int lose { get; set; }
}

#endregion

public class ServerManager : Colyseus.ColyseusManager<ServerManager>
{
    private static ServerManager _instance;

    const string _connectionUri = "mongodb+srv://eodls0810:shjin5405@starcraft.lxxbebz.mongodb.net/?retryWrites=true&w=majority";
    const string _userCollection = "User";

    private string token = string.Empty;
    private string refreshToken = string.Empty;

    private MongoClient _client = null;

    private IMongoDatabase _mongoDB = null;

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
    }

    public void Initialize()
    {
        var settings = MongoClientSettings.FromConnectionString(_connectionUri);
        _client = new MongoClient(settings);
        _mongoDB = _client.GetDatabase("Starcraft");
    }

    public void CreatePlayerInfo(string ID, string PW, string nickName, Action<PlayerInfo> onResult)
    {
        var req = new
        {
            ID = ID,
            PW = PW,
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

            onResult?.Invoke(res.playerInfo);
        });
    }

    public void GetPlayerInfo(string ID, string PW, Action<PlayerInfo> onResult)
    {
        var req = new
        {
            ID = ID,
            PW = PW
        };

        var res = new
        {
            resultCode = 0,
            message = string.Empty,
            playerInfo = new PlayerInfo()
        };

        SendPostRequestAsync("user/getPlayerInfo", req, (resultJson, resultCode) =>
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

            onResult?.Invoke(res.playerInfo);
        });
    }

    public void GetPlayerInfo(ObjectId _id, Action<PlayerInfo> onResult)
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

            if (result.resultCode == 0)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(res.playerInfo);
        });
    }

    public void UpdatePlayerInfo(ObjectId _id, PlayerInfo newInfo, Action<PlayerInfo> onResult)
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

            if (result.resultCode == 0)
            {
                onResult?.Invoke(null);
                return;
            }

            onResult?.Invoke(res.playerInfo);
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

        SendPostRequestAsync("user/getObjectDatas", req, (resultJson, resultCode) =>
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

            onResult?.Invoke(result.objectDatas);
        });
    }

    private void Server_SendPostRequestAsync(string endpoint, object body, Action<string, long> onResult)
    {
        SendPostRequestAsync(endpoint, body, onResult);
    }

    private void Server_SendPostRequestFormData(string endpoint, string filename, object body, byte[] bytes, Action<string, long> onResult)
    {
        SendPostRequestFormData(endpoint, filename, body, bytes, onResult);
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
