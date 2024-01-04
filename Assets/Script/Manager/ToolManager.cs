using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ToolManager : MonoBehaviour
{
    [SerializeField]
    private SceneChanger _sceneChanger = null;

    [SerializeField]
    private CanvasGroupScreenFade _canvasGroupScreen = null;

    [SerializeField]
    private UiAnimation _uiAnimation = null;

    [SerializeField]
    private FileManager _filemanager = null;

    [SerializeField]
    private GameObject _objObjectPool = null;

    private ObjectPool _objectPool = null;

    private Action<eScene> _onChangeSceneCallback = null;

    public void Initialize(Action<eScene> onChangeSceneCallback)
    {
        _sceneChanger.Initialize();
        _canvasGroupScreen.Initialize();
        _uiAnimation.Initialize();
        _filemanager.Initialize();

        if (onChangeSceneCallback != null)
        {
            _onChangeSceneCallback = onChangeSceneCallback;
        }
    }

    public void InstantiateObjectPool()
    {
        GameObject obj = Instantiate(_objObjectPool, InGameManager.instance.transform);

        if (obj.TryGetComponent(out ObjectPool pool))
        {
            _objectPool = pool;
            _objectPool.Initialize();
        }
    }

    public string RandomString(int _nLength = 12)
    {
        System.Random _random = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값

        const string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxynz";  //문자 생성 풀
        char[] chRandom = new char[_nLength];

        for (int i = 0; i < _nLength; i++)
        {
            chRandom[i] = strPool[_random.Next(strPool.Length)];
        }
        string strRet = chRandom.ToString();   // char to string
        return strRet;
    }

    public void ChangeScene(string title, string description, eScene eLoadSceneIndex, Action onLoadDoneCallback = null, Image backgroundImage = null)
    {
        _sceneChanger.SceneLoad(title, description, eLoadSceneIndex, onLoadDoneCallback, backgroundImage);

        _onChangeSceneCallback?.Invoke(eLoadSceneIndex);
    }

    public void ShowCnavasGroup(CanvasGroup cg, float delayTime, float durationTime, Action onResultCallback = null)
    {
        _canvasGroupScreen.ShowCnavasGroup(cg, delayTime, durationTime, onResultCallback);
    }

    public void HideCnavasGroup(CanvasGroup cg, float delayTime, float durationTime, Action onResultCallback = null)
    {
        _canvasGroupScreen.HideCnavasGroup(cg, delayTime, durationTime, onResultCallback);
    }

    public void MoveUI(RectTransform rTr, Vector2 targetPosition, float fadeTime, bool snapping, Action onResult, Ease easeMode = Ease.Unset)
    {
        _uiAnimation.Move(rTr, targetPosition, fadeTime, snapping, onResult, easeMode);
    }

    public void MoveX(RectTransform rTr, float endValue, float duration, bool snapping, Action onResult)
    {
        _uiAnimation.MoveX(rTr, endValue, duration, snapping, onResult);
    }

    public bool FileCheck(string path)
    {
        return _filemanager.FileCheck(path);
    }

    public string ReadFile(string path)
    {
        return _filemanager.ReadFile(path);
    }

    public string JsonSerialize(object body)
    {
        return _filemanager.JsonSerialize(body);
    }

    public T JsonDeserialize<T>(T t, string json)
    {
        return _filemanager.JsonDeserialize(t, json);
    }

    public T ConvertToData<T>(T t, string path)
    {
        if(_filemanager.FileCheck(path) == false)
        {
            return default(T);
        }

        string json = _filemanager.ReadFile(path);

        return _filemanager.JsonDeserialize(t, json);
    }

    public UnZipMapFile UnZipFile(string path)
    {
        return _filemanager.UnZipFile(path);
    }

    public void ImageDownload(string url, Action<Texture, bool> onResult)
    {
        _filemanager.ImageDownload(url, onResult);
    }

    public Sprite ConvertTextureToSprite(Texture texture)
    {
        return _filemanager.ConvertTextureToSprite(texture);
    }

    public void RequestPool(ePoolType type, int key, Action<RequestPool> onResult)
    {
        if(_objectPool == null)
        {
            return;
        }

        _objectPool.Request(type, key, onResult);
    }

    public void RequestPool(string name, ePoolType type, Action<RequestPool> onResult)
    {
        if (_objectPool == null)
        {
            return;
        }

        _objectPool.Request(name, type, onResult);
    }

    public void PutBackPool(PutBackPool value)
    {
        _objectPool.PutBack(value);
    }
}
