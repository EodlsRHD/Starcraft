using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private string _buildingPath = string.Empty;

    [SerializeField]
    private string _UnitPath = string.Empty;

    private List<ObjectData> _dataList = null;
    private Dictionary<int, ObjectData> _datas = new Dictionary<int, ObjectData>();

    private Queue<RequestPool> _requestQueue = new Queue<RequestPool>();

    public void Initialize()
    {

    }

    public void RequestData(int key, Action<RequestPool> onResult)
    {
        RequestPool p = new RequestPool();
        p.SetPool(ePoolType.Data, key, onResult);

        _requestQueue.Enqueue(p);
    }

    private void Update()
    {
        if(_requestQueue.Count > 0)
        {
            for (int i = 0; i < _requestQueue.Count; i++)
            {
                RequestPool p = _requestQueue.Dequeue();

                Debug.LogError(nameof(p.key));

                if (_datas[p.key] == null)
                {
                //    var fields = GameManager.instance.poolMemory.GetType().GetFields().ToList();
                //    var field = fields.Find(x => x.GetValue(x).GetType().GetField() == p.key);

                //    var d = _dataList.Find(x => x.name.Equals())
                }
                else
                {
                    p.SetRequset(_datas[p.key]);
                }

                p.onResult?.Invoke(p);
            }
        }
    }

    private void ReadData()
    {
        _dataList = new List<ObjectData>();
    }
}
