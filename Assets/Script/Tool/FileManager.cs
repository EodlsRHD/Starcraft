using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;

public class FileManager : MonoBehaviour
{
    public void Initialize()
    {
        
    }

    public bool FileCheck(string path)
    {
        return File.Exists(path);
    }

    public string ReadFile(string Path)
    {
        return File.ReadAllText(Path);
    }

    public string JsonSerialize(object body)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(body);
    }

    public T JsonDeserialize<T>(T t, string json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, t);
    }

    public async void ImageDownload(string url, Action<Texture, bool> onResult)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            float time = Time.realtimeSinceStartup;
            await req.SendWebRequest();
            Debug.Log("Time used : " + (Time.realtimeSinceStartup - time));

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("ConnectionError");
                onResult?.Invoke(null, false);
                return;
            }

            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("ProtocolError");
                onResult?.Invoke(null, false);
                return;
            }

            onResult?.Invoke(((DownloadHandlerTexture)req.downloadHandler).texture, true);
        }
    }

    public Sprite ConvertTextureToSprite(Texture texture)
    {
        Texture2D texture2D = (Texture2D)texture;
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(texture2D.width * 0.5f, texture2D.height * 0.5f));
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
