using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Generator
{
    //[System.Serializable]
    //public class MapData
    //{
    //    public string name = string.Empty;
    //    public string description = string.Empty;
    //    public string version = string.Empty;
    //    public string maker = string.Empty;
    //    public int maxPlayer = 0;

    //    public string thumbnailPath = string.Empty;

    //    public int mapSizeX = 0;
    //    public int mpaSizeY = 0;

    //    public Node[,] mapData = null;
    //}

    //[System.Serializable]
    //public class Node
    //{
    //    public float x = 0;
    //    public float y = 0;

    //    public Topographic topographic = null;
    //    public StartPosition startPosition = null;
    //    public Resource resource = null;
    //}

    //[System.Serializable]
    //public class Topographic
    //{
    //    public bool walkable = false;
    //    public bool isHill = false;
    //    public byte height = 0; // 0 ~ 1
    //}

    //[System.Serializable]
    //public class StartPosition
    //{
    //    public int team = 0;
    //    public ePlayerColor playerColor = ePlayerColor.Non;
    //}

    //[System.Serializable]
    //public class Resource
    //{
    //    public eResourceType type = eResourceType.Non;
    //    public int quantity = 0;
    //}

    public class MapGenerator : MonoBehaviour
    {
        [Header("UI")]

        [SerializeField]
        private Button _buttonMakeing = null;

        [SerializeField]
        private Scrollbar _scrollBarMapSize = null;

        [Space(10)]

        [SerializeField]
        private Terrain _terrainFild = null;

        [SerializeField]
        private Camera _cameraPpreview = null;

        [SerializeField]
        private GameObject _objects = null;

        [Header("Parant")]

        [SerializeField]
        private Transform _trHillParant = null;

        [SerializeField]
        private Transform _trMinaralParant = null;

        [SerializeField]
        private Transform _trGanParant = null;

        private MapData _mapData = null;

        private Coroutine _coGrid = null;

        [Space(10)]

        public readonly string _resources = "Resources";

        public readonly string _startPoint = "StartPoint";

        private void Start()
        {
            _scrollBarMapSize.value = _terrainFild.terrainData.size.x;

            _scrollBarMapSize.onValueChanged.AddListener(ResizeingMap);
            _buttonMakeing.onClick.AddListener(Generate);
        }

        private void ResizeingMap(float value)
        {
            DestryObjs(_trHillParant);
            DestryObjs(_trMinaralParant);
            DestryObjs(_trGanParant);

            float convert = value * 256;

            if(value == 0)
            {
                convert = 64;
            }

            _terrainFild.terrainData.size = new Vector3(convert, _terrainFild.terrainData.size.y, convert);

            _cameraPpreview.orthographicSize = convert * 0.5f;
            _cameraPpreview.transform.position = new Vector3(convert * 0.5f, _cameraPpreview.transform.position.y, convert * 0.5f);

            _objects.transform.localScale = new Vector3(convert / 256f, 1, convert / 256f);

            if(_coGrid != null)
            {
                StopCoroutine(_coGrid);
            }

            _coGrid = StartCoroutine(Co_Grid((int)convert, () => 
            { 
            
            }));
        }

        private void DestryObjs(Transform parant)
        {
            var child = parant.transform.GetComponentsInChildren<Transform>();

            foreach (var iter in child)
            {
                if (iter != parant.transform)
                {
                    Destroy(iter.gameObject);
                }
            }
            parant.transform.DetachChildren();
        }

        IEnumerator Co_Grid(int lenght, System.Action onResult)
        {
            _mapData.nodes = new Node[lenght, lenght];

            for (int y = 0; y < lenght; y++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    _mapData.nodes[x, y].x = x;
                    _mapData.nodes[x, y].x = y;
                    CheckNode(ref _mapData.nodes[x, y]);

                    yield return null;
                }
            }

            yield return null;

            GameObject[] objResources = GameObject.FindGameObjectsWithTag(_resources);
            GameObject[] objStartColor = GameObject.FindGameObjectsWithTag(_startPoint);

            for (int i = 0; i < objResources.Length; i++)
            {
                int x = Mathf.FloorToInt(objResources[i].transform.position.x);
                int y = Mathf.FloorToInt(objResources[i].transform.position.y);

                _mapData.nodes[x, y].resource.type = (eResourceType)int.Parse(objResources[i].name.Split("_")[0]);
                _mapData.nodes[x, y].resource.quantity = int.Parse(objResources[i].name.Split("_")[1]);

            }

            for (int i = 0; i < objStartColor.Length; i++)
            {
                int x = Mathf.FloorToInt(objResources[i].transform.position.x);
                int y = Mathf.FloorToInt(objResources[i].transform.position.y);

                _mapData.nodes[x, y].startPosition.playerColor = (ePlayerColor)int.Parse(objResources[i].name.Split("_")[0]);
                _mapData.nodes[x, y].startPosition.team = int.Parse(objResources[i].name.Split("_")[1]);
            }

            yield return null;

            _coGrid = null;
            onResult?.Invoke();
        }

        private void CheckNode(ref Node node)
        {
            bool detected = false;

            node.resource = new Resource();
            node.startPosition = new StartPosition();
            node.topographic = new Topographic();

            Vector3 pos = new Vector3(node.x, 100f, node.y);

            if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag.Equals(_resources))
                {
                    detected = true;
                }

                if (hit.collider.gameObject.tag.Equals(_startPoint))
                {
                    detected = true;
                }

                if(detected == true)
                {
                    node.topographic.isWalkable = false;
                    node.topographic.height = hit.point.y;
                    return;
                }

                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 0.5f, NavMesh.AllAreas))
                {
                    node.topographic.isWalkable = true;
                    node.topographic.height = navHit.position.y;
                }
            }
        }

        private void Generate()
        {
            _mapData = new MapData();
        }
    }
}
