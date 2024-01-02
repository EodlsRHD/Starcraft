using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ePoolType
{
    Non = -1,
    Image,
    Prefab
}

public struct RequestPool
{
    public ePoolType type;
    public int key;
    public Action<RequestPool> onResult;

    private Sprite _s;
    private GameObject _o;

    public void SetPool(ePoolType type, int key, Action<RequestPool> onResult)
    {
        this.type = type;
        this.key = key;
        this.onResult = onResult;
    }

    public void SetRequest(GameObject o)
    {
        _o = o;
    }

    public void SetRequest(Sprite s)
    {
        _s = s;
    }

    public Sprite GetSprite()
    {
        return _s;
    }

    public GameObject GetObject()
    {
        return _o;
    }
}

public struct PutBackPool
{
    public ePoolType type;
    public int key;

    public Sprite s;
    public GameObject o;

    public void SetData(ePoolType type, int key, Sprite s)
    {
        this.type = type;
        this.key = key;
        this.s = s;
    }

    public void SetData(ePoolType type, int key, GameObject o)
    {
        this.type = type;
        this.key = key;
        this.o = o;
    }
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _originalImageList = new List<Sprite>();

    [SerializeField]
    private List<GameObject> _originalObjList = new List<GameObject>();

    private const int INITIAL_COUNT = 10;
    private const int MAX_COUNT = 50;

    private Dictionary<int, Stack<Sprite>> _dictionaryImagePool = null;
    private Dictionary<int, Stack<GameObject>> _dictionaryObjectPool = null;

    private Stack<Sprite> _stackImage = null;
    private Stack<GameObject> _stackObject = null;

    private Queue<RequestPool> requestQueue = new Queue<RequestPool>();

    private Queue<PutBackPool> _destroyQueue = new Queue<PutBackPool>();
    private Coroutine _coDestroy = null;
    private bool _isDestroy = false;

    public void Initialize()
    {
        _dictionaryImagePool = new Dictionary<int, Stack<Sprite>>();
        _dictionaryObjectPool = new Dictionary<int, Stack<GameObject>>();

        if(GameManager.instance.TEST_MODE == false)
        {
            for (int i = 0; i < _originalImageList.Count; i++)
            {
                _stackImage = new Stack<Sprite>();

                for (int j = 0; j < INITIAL_COUNT; j++)
                {
                    var s = Instantiate(_originalImageList[i]);
                    _stackImage.Push(s);
                }

                _dictionaryImagePool.Add(_originalImageList[i].GetHashCode(), _stackImage);
            }

            for (int i = 0; i < _originalObjList.Count; i++)
            {
                _stackObject = new Stack<GameObject>();

                for (int j = 0; j < INITIAL_COUNT; j++)
                {
                    var s = Instantiate(_originalObjList[i]);
                    _stackObject.Push(s);
                }

                _dictionaryObjectPool.Add(_originalObjList[i].GetHashCode(), _stackObject);
            }
        }

        this.gameObject.SetActive(true);
    }

    public void Request(ePoolType type, int key, Action<RequestPool> onResult)
    {
        RequestPool p = new RequestPool();
        p.SetPool(type, key, onResult);

        requestQueue.Enqueue(p);
    }

    public void Request(string name, ePoolType type, Action<RequestPool> onResult)
    {
        if(name.Contains("Hp_")) // test code
        {
            return;
        }

        string convertName = ConvertName(name);
        RequestPool p = new RequestPool();

        switch (type)
        {
            case ePoolType.Image:
                {
                    for (int i = 0; i < _originalImageList.Count; i++)
                    {
                        if(_originalImageList[i].name.Contains(convertName))
                        {
                            p.SetPool(type, _originalImageList[i].GetHashCode(), onResult);

                            break;
                        }
                    }
                }
                break;

            case ePoolType.Prefab:
                {
                    for (int i = 0; i < _originalObjList.Count; i++)
                    {
                        if (_originalObjList[i].name.Contains(convertName))
                        {
                            p.SetPool(type, _originalObjList[i].GetHashCode(), onResult);

                            break;
                        }
                    }
                }
                break;
        }

        requestQueue.Enqueue(p);
    }

    private string ConvertName(string name)
    {
        string result = string.Empty;

        string Non = "Non";
        string[] split = name.Split("_");

        if(split[1].Equals("Protoss"))
        {
            split[2] = Non;
            split[4] = Non;
        }

        if(split[1].Equals("Terran"))
        {
            split[4] = Non;
        }

        if (split[1].Equals("Zerg"))
        {
            split[2] = Non;

            if(split[0].Equals("Defence"))
            {
                split[4] = Non;
            }
        }

        for (int i = 0; i < split.Length; i++)
        {
            result += split[i];

            if(i < split.Length - 1)
            {
                result += "_";
            }
        }

        return result;
    }

    public void PutBack(PutBackPool value)
    {
        switch(value.type)
        {
            case ePoolType.Image:
                {
                    if(_dictionaryImagePool.ContainsKey(value.key) == false)
                    {
                        return;
                    }

                    if(_dictionaryImagePool[value.key].Count >= MAX_COUNT)
                    {
                        _destroyQueue.Enqueue(value);
                        return;
                    }

                    _dictionaryImagePool[value.key].Push(value.s);
                }
                break;

            case ePoolType.Prefab:
                {
                    if (_dictionaryObjectPool.ContainsKey(value.key) == false)
                    {
                        return;
                    }

                    if (_dictionaryObjectPool[value.key].Count >= MAX_COUNT)
                    {
                        _destroyQueue.Enqueue(value);
                        return;
                    }

                    _dictionaryObjectPool[value.key].Push(value.o);
                }
                break;
        }
    }

    private void Update()
    {
        if(requestQueue.Count > 0)
        {
            for (int i = 0; i < requestQueue.Count; i++)
            {
                RequestPool p = requestQueue.Dequeue();

                switch (p.type)
                {
                    case ePoolType.Image:
                        {
                            if(_dictionaryImagePool.ContainsKey(p.key) == false)
                            {
                                var result = Find(p.key, _originalImageList);
                                _dictionaryImagePool.Add(p.key, new Stack<Sprite>());
                                _dictionaryImagePool[p.key].Push(Instantiate(result));
                            }

                            Sprite s = _dictionaryImagePool[p.key].Pop();
                            p.SetRequest(s);
                        }
                        break;

                    case ePoolType.Prefab:
                        {
                            if (_dictionaryObjectPool.ContainsKey(p.key) == false)
                            {
                                var result = Find(p.key, _originalObjList);
                                _dictionaryObjectPool.Add(p.key, new Stack<GameObject>());
                                _dictionaryObjectPool[p.key].Push(Instantiate(result));
                            }

                            GameObject o = _dictionaryObjectPool[p.key].Pop();
                            p.SetRequest(o);
                        }
                        break;
                }

                p.onResult?.Invoke(p);
            }
        }

        if(_isDestroy == false)
        {
            if (_destroyQueue.Count > 0)
            {
                _isDestroy = true;
                _coDestroy = StartCoroutine(CoDestroy());
            }
        }
    }

    private T Find<T>(int key, List<T> list)
    {
        T result = default(T);

        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].GetHashCode() == key)
            {
                result = list[i];

                break;
            }
        }

        return result;
    }

    IEnumerator CoDestroy()
    {
        while(_destroyQueue.Count > 0)
        {
            PutBackPool p = _destroyQueue.Dequeue();

            if(p.o != null)
            {
                Destroy(p.o);
            }

            if (p.s != null)
            {
                Destroy(p.s);
            }

            yield return 0.1f;
        }

        _isDestroy = false;
    }

    private void OnDestroy()
    {
        if(_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
    }
}
