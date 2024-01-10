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

    public bool hasCustom_2 { get; set; }
    public int custom_2_key { get; set; }

    public bool hasCustom_3 { get; set; }
    public int custom_3_key { get; set; }

    public bool hasCustom_4 { get; set; }
    public int custom_4_key { get; set; }
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
        Uni_CreatePlayerInfo(ID, PW, nickName, onResult).Forget();
    }

    public void GetPlayerInfo(string ID, string PW, Action<PlayerInfo> onResult)
    {
        Uni_GetPlayerInfo(ID, PW, onResult).Forget();
    }

    public void GetPlayerInfo(ObjectId _id, Action<PlayerInfo> onResult)
    {
        Uni_GetPlayerInfo(_id, onResult).Forget();
    }

    public void UpdatePlayerInfo(ObjectId _id, PlayerInfo newInfo, Action<PlayerInfo> onResult)
    {
        Uni_UpdatePlayerInfo(_id, newInfo, onResult).Forget();
    }

    public void GetObjectDatas(Action<List<ObjectData>> onResult)
    {
         Uni_GetObjectDatas(onResult).Forget();
    }

    private async UniTaskVoid Uni_CreatePlayerInfo(string ID, string PW, string nickName, Action<PlayerInfo> onResult)
    {
        PlayerInfo result = new PlayerInfo();
        result.ID = ID;
        result.PW = ID;
        result.nickName = nickName;

        try
        {
            var collection = _mongoDB.GetCollection<PlayerInfo>(_userCollection);
            await collection.InsertOneAsync(result);

            onResult?.Invoke(result);
        }
        catch
        {
            onResult?.Invoke(null);
        }
    }

    private async UniTaskVoid Uni_GetPlayerInfo(string ID, string PW, Action<PlayerInfo> onResult)
    {
        try
        {
            var collection = _mongoDB.GetCollection<PlayerInfo>(_userCollection);
            var document = await collection.Find(x => x.ID.Equals(ID) && x.PW.Equals(PW)).ToListAsync();

            onResult?.Invoke(document[0]);
        }
        catch
        {
            onResult?.Invoke(null);
        }
    }

    private async UniTaskVoid Uni_GetPlayerInfo(ObjectId _id, Action<PlayerInfo> onResult)
    {
        var collection = _mongoDB.GetCollection<PlayerInfo>(_userCollection);
        var document = await collection.Find(x => x._id.Equals(_id)).ToListAsync();

        onResult?.Invoke(document[0]);
    }

    private async UniTaskVoid Uni_UpdatePlayerInfo(ObjectId _id, PlayerInfo newInfo, Action<PlayerInfo> onResult)
    {
        var collection = _mongoDB.GetCollection<PlayerInfo>(_userCollection);
        var filter = Builders<PlayerInfo>.Filter.Eq(x => x._id, _id);
        var document = await collection.ReplaceOneAsync(filter, newInfo);

        onResult?.Invoke(newInfo);
    }

    private async UniTaskVoid Uni_GetObjectDatas(Action<List<ObjectData>> onResult)
    {
        var collection = _mongoDB.GetCollection<ObjectData>("ObjectData");
        var document = await collection.Find(_ => true).ToListAsync();

        onResult?.Invoke(document);
    }

    private void Server_SendPostRequestAsync(string endpoint, object body, Action<string, long> onResult)
    {
        SendPostRequestAsync(endpoint, body, onResult).Forget();
    }

    private void Server_SendPostRequestFormData(string endpoint, string filename, object body, byte[] bytes, Action<string, long> onResult)
    {
        SendPostRequestFormData(endpoint, filename, body, bytes, onResult).Forget();
    }

    private async UniTaskVoid SendPostRequestAsync(string endpoint, object body, Action<string, long> onResult)
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

    private async UniTaskVoid SendPostRequestFormData(string endpoint, string filename, object body, byte[] bytes, Action<string, long> onResult)
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
