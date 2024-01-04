using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;

namespace Generator
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("UI")]

        [Header("Select Object")]

        [SerializeField]
        private TMP_Text _textSelectObjInfoName = null;

        [SerializeField]
        private Button _buttonUnSelect = null;

        [SerializeField]
        private Button _buttonDestroy = null;

        [Header("Info")]

        [SerializeField]
        private TMP_InputField _inputName = null;

        [SerializeField]
        private TMP_InputField _inputDescription = null;

        [SerializeField]
        private TMP_InputField _inputMaker = null;

        [SerializeField]
        private TMP_InputField _inputVersion = null;

        [SerializeField]
        private TMP_InputField _inputMaxPlayer = null;

        [Header("Map Size")]

        [SerializeField]
        private Button _buttonGenerate = null;

        [SerializeField]
        private Scrollbar _scrollBarMapSize = null;

        [SerializeField]
        private Button _buttonChangeMapSize = null;

        [Header("Resources")]

        [SerializeField]
        private Button _buttonMineral = null;

        [SerializeField]
        private Button _buttonGas = null;

        [SerializeField]
        private TMP_InputField _inputQuantity = null;

        [Header("Start Point")]

        [SerializeField]
        private TMP_Dropdown _dropdownColor = null;

        [SerializeField]
        private Button _buttonMakePosition = null;

        [Header("Objects")]

        [SerializeField]
        private Terrain _terrainFild = null;

        [SerializeField]
        private Camera _cameraPpreview = null;

        [SerializeField]
        private GameObject _objects = null;

        [SerializeField]
        private GameObject _objLoading = null;

        [SerializeField]
        private RectTransform _rtrRawImage = null;

        [SerializeField]
        private GameObject _objSelectInfo = null;

        [Header("Parant")]

        [SerializeField]
        private Transform _trHillParant = null;

        [SerializeField]
        private Transform _trMinaralParant = null;

        [SerializeField]
        private Transform _trGanParant = null;

        [SerializeField]
        private Transform _trStartPositionParant = null;

        [Header("Prefab")]

        [SerializeField]
        private GameObject _objMineralPrefab = null;

        [SerializeField]
        private GameObject _objGasPrefab = null;

        [SerializeField]
        private GameObject _objStartPositionPrefab = null;

        private MapData _mapData = null;

        private Coroutine _coGrid = null;

        private GameObject _selectObject = null;

        private float _beforeValue = 0f;

        private float _mapSize = 0f;

        private float _mapSizeHalf = 0f;

        private readonly string _resources = "Resources";

        private readonly string _startPoint = "StartPoint";

        private readonly string _ground = "Ground";

        private LayerMask _layMask_ground;

        private bool _isRed = false;

        private bool _isOrange = false;

        private bool _isYellow = false;

        private bool _isGreen = false;

        private bool _isBlue = false;

        private bool _isPurple = false;

        private bool _isBlack = false;

        private int _colorCount = 0;

        private bool _isMakeGridDone = true;

        private void Start()
        {
            _beforeValue = _scrollBarMapSize.value;

            if(_scrollBarMapSize.value == 0)
            {
                _mapSize = 64;
            }
            else
            {
                _mapSize = _scrollBarMapSize.value * 256;
            }

            _mapSizeHalf = _mapSize * 0.5f;

            _layMask_ground = LayerMask.GetMask(_ground);

            _buttonUnSelect.onClick.AddListener(UnSelectObject);
            _buttonDestroy.onClick.AddListener(DestroyObject);

            _buttonMineral.onClick.AddListener(() => { InstantiateResources(eResourceType.Mineral); });
            _buttonGas.onClick.AddListener(() => { InstantiateResources(eResourceType.Gas); });
            _inputQuantity.text = "0";

            _buttonMakePosition.onClick.AddListener(() => { InstantiateStartPosition(_dropdownColor.value); });

            _buttonChangeMapSize.onClick.AddListener(() => { ResizeingMap(_scrollBarMapSize.value); });
            _buttonGenerate.onClick.AddListener(Generate);

            _objSelectInfo.gameObject.SetActive(false);
        }

        private void OnApplicationQuit()
        {
            _scrollBarMapSize.value = 0f;
            _cameraPpreview.orthographicSize = 32f;
            _terrainFild.terrainData.size = new Vector3(64f, _terrainFild.terrainData.size.y, 64f);
        }

        private void Update()
        {
            if(_isMakeGridDone == false)
            {
                return;
            }

            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                return;
            }

            Vector2 mousePos = Input.mousePosition;
            float x = mousePos.x - _rtrRawImage.transform.position.x;
            float y = mousePos.y - _rtrRawImage.transform.position.y;

            x /= (_rtrRawImage.rect.width * 0.5f);
            y /= (_rtrRawImage.rect.height * 0.5f);

            Vector3 cameraPos = _cameraPpreview.transform.position;
            Vector3 shootRayPos = new Vector3(cameraPos.x + (_mapSizeHalf * x), cameraPos.y, cameraPos.z + (_mapSizeHalf * y));

            InputMouseClick(shootRayPos);
            InputMouseWhill();
            CameraMove(cameraPos, x, y);
        }

        private void InputMouseClick(Vector3 shootRayPos)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(shootRayPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, ~_layMask_ground))
                {
                    _selectObject = hit.collider.gameObject;
                    _textSelectObjInfoName.text = _selectObject.name;

                    _objSelectInfo.gameObject.SetActive(true);
                }
            }

            if (_selectObject == null)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(shootRayPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, _layMask_ground))
                {
                    if(_selectObject.tag.Equals(_startPoint))
                    {
                        _selectObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                    }
                    else
                    {
                        _selectObject.transform.position = hit.point;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(shootRayPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, _layMask_ground))
                {
                    if (_selectObject.tag.Equals(_startPoint))
                    {
                        _selectObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                    }
                    else
                    {
                        _selectObject.transform.position = hit.point;
                    }

                    _selectObject = null;
                }
            }
        }

        private void InputMouseWhill()
        {
            float value = Input.mouseScrollDelta.y;

            if(_cameraPpreview.orthographicSize < 5)
            {
                _cameraPpreview.orthographicSize = 5;
                return;
            }

            if(_cameraPpreview.orthographicSize > _mapSizeHalf)
            {
                _cameraPpreview.orthographicSize = _mapSizeHalf;
                return;
            }

            value *= (_mapSize / 265f);
            _cameraPpreview.orthographicSize -= value;
        }

        private void CameraMove(Vector3 cameraPos, float x, float y)
        {
            if (cameraPos.x + _cameraPpreview.orthographicSize < _mapSize)
            {
                if (x >= 0.95f && x <= 1f)
                {
                    cameraPos.x += 10f * Time.deltaTime;
                }
            }

            if (cameraPos.x - _cameraPpreview.orthographicSize > 0)
            {
                if (x <= -0.95f && x >= -1f)
                {
                    cameraPos.x -= 10f * Time.deltaTime;
                }
            }

            if (cameraPos.z + _cameraPpreview.orthographicSize < _mapSize)
            {
                if (y >= 0.95f && y <= 1f)
                {
                    cameraPos.z += 10f * Time.deltaTime;
                }
            }

            if (cameraPos.z - _cameraPpreview.orthographicSize > 0)
            {
                if (y <= -0.95f && y >= -1f)
                {
                    cameraPos.z -= 10f * Time.deltaTime;
                }
            }

            if (cameraPos.x + _cameraPpreview.orthographicSize > _mapSize)
            {
                if (cameraPos.x != _mapSizeHalf)
                {
                    cameraPos.x -= _cameraPpreview.orthographicSize - (_mapSize - cameraPos.x);
                }
            }

            if (cameraPos.x - _cameraPpreview.orthographicSize < 0)
            {
                if (cameraPos.x != _mapSizeHalf)
                {
                    cameraPos.x += _cameraPpreview.orthographicSize - cameraPos.x;
                }
            }

            if (cameraPos.z + _cameraPpreview.orthographicSize > _mapSize)
            {
                if (cameraPos.z != _mapSizeHalf)
                {
                    cameraPos.z -= _cameraPpreview.orthographicSize - (_mapSize - cameraPos.z);
                }
            }

            if (cameraPos.z - _cameraPpreview.orthographicSize < 0)
            {
                if (cameraPos.z != _mapSizeHalf)
                {
                    cameraPos.z += _cameraPpreview.orthographicSize - cameraPos.z;
                }
            }

            _cameraPpreview.transform.position = cameraPos;
        }

        private void UnSelectObject()
        {
            _objSelectInfo.gameObject.SetActive(false);
            _textSelectObjInfoName.text = string.Empty;
            _selectObject = null;
        }

        private void DestroyObject()
        {
            _objSelectInfo.gameObject.SetActive(false);
            _textSelectObjInfoName.text = string.Empty;

            Destroy(_selectObject);
            _selectObject = null;
        }

        private void InstantiateResources(eResourceType type)
        {
            GameObject newObj = new GameObject();
            Destroy(newObj);

            switch (type)
            {
                case eResourceType.Mineral:
                    newObj = Instantiate(_objMineralPrefab, _trMinaralParant);
                    newObj.name = _resources + "_" + 0 + "_" + _inputQuantity.text;
                    break;

                case eResourceType.Gas:
                    newObj = Instantiate(_objGasPrefab, _trGanParant);
                    newObj.name = _resources + "_" + 1 + "_" + _inputQuantity.text;
                    break;
            }

            newObj.tag = _resources;

            if (Physics.Raycast(new Vector3(_mapSizeHalf, 100f, _mapSizeHalf), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                newObj.transform.position = new Vector3(hit.point.x, hit.point.y + (newObj.transform.localScale.y * 0.5f), hit.point.z);
            }
        }

        private void InstantiateStartPosition(int value)
        {
            Color color = new Color();

            switch (value)
            {
                case 0: // red
                    {
                        if (_isRed == true)
                        {
                            return;
                        }

                        color = Color.red;
                        _isRed = true;
                    }
                    break;

                case 1: // orange
                    {
                        if (_isOrange == true)
                        {
                            return;
                        }

                        color = new Color(1.0f, 0.64f, 0f);
                        _isOrange = true;
                    }
                    break;

                case 2: // yellow
                    {
                        if (_isYellow == true)
                        {
                            return;
                        }

                        color = Color.yellow;
                        _isYellow = true;
                    }
                    break;

                case 3: // green
                    {
                        if (_isGreen == true)
                        {
                            return;
                        }

                        color = Color.green;
                        _isGreen = true;
                    }
                    break;

                case 4: // blue
                    {
                        if (_isBlue == true)
                        {
                            return;
                        }

                        color = Color.blue;
                        _isBlue = true;
                    }
                    break;

                case 5: // purple
                    {
                        if (_isPurple == true)
                        {
                            return;
                        }

                        color = new Color(180f / 255f, 0f, 180f / 255f);
                        _isPurple = true;
                    }
                    break;

                case 6: // black
                    {
                        if (_isBlack == true)
                        {
                            return;
                        }

                        color = Color.black;
                        _isBlack = true;
                    }
                    break;
            }

            GameObject newStartPosition = Instantiate(_objStartPositionPrefab, _trStartPositionParant);
            newStartPosition.name = _startPoint + "_" + value + "_" + _colorCount;
            newStartPosition.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

            if (Physics.Raycast(new Vector3(_mapSize, 100f, _mapSize), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                newStartPosition.transform.position = new Vector3(hit.point.x, hit.point.y + (newStartPosition.transform.localScale.y * 0.5f), hit.point.z);
            }

            if (newStartPosition.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.color = color;

                _colorCount++;
            }
            else
            {
                Debug.LogError("_objStartPositionPrefab does not contain a SpriteRenderer component.");
            }
        }

        private void ResizeingMap(float value)
        {
            if(_beforeValue == value)
            {
                return;
            }

            _beforeValue = value;
            _isMakeGridDone = false;

            DestryObjs(_trHillParant);
            DestryObjs(_trMinaralParant);
            DestryObjs(_trGanParant);
            DestryObjs(_trStartPositionParant);

            _mapSize = value * 256;

            if(value == 0)
            {
                _mapSize = 64;
            }

            _mapSizeHalf = _mapSize * 0.5f;

            _terrainFild.terrainData.size = new Vector3(_mapSize, _terrainFild.terrainData.size.y, _mapSize);

            _cameraPpreview.orthographicSize = _mapSize * 0.5f;
            _cameraPpreview.transform.position = new Vector3(_mapSize * 0.5f, _cameraPpreview.transform.position.y, _mapSize * 0.5f);

            _objects.transform.localScale = new Vector3(_mapSize / 256f, 1, _mapSize / 256f);

            if(_mapData == null)
            {
                _mapData = new MapData();
            }

            if (_coGrid != null)
            {
                StopCoroutine(_coGrid);
            }

            _coGrid = StartCoroutine(Co_Grid((int)_mapSize, false));
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

            _isRed = false;
            _isOrange = false;
            _isYellow = false;
            _isGreen = false;
            _isBlue = false;
            _isPurple = false;
            _isBlack = false;

            _colorCount = 0;
        }

        IEnumerator Co_Grid(int lenght, bool isGenerate)
        {
            _buttonGenerate.gameObject.SetActive(false);
            _objLoading.gameObject.SetActive(true);

            _mapData.nodes = new Node[lenght, lenght];

            for (int y = 0; y < lenght; y++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    _mapData.nodes[x, y] = new Node();

                    _mapData.nodes[x, y].x = x;
                    _mapData.nodes[x, y].y = y;

                    CheckNode(ref _mapData.nodes[x, y], isGenerate);
                }

                yield return null;
            }

            if(isGenerate == false)
            {
                InitializeGrid(lenght);

                _isMakeGridDone = true;

                _coGrid = null;

                _buttonGenerate.gameObject.SetActive(true);
                _objLoading.gameObject.SetActive(false);

                yield break;
            }

            yield return null;

            GameObject[] objResources = GameObject.FindGameObjectsWithTag(_resources);
            GameObject[] objStartColor = GameObject.FindGameObjectsWithTag(_startPoint);

            for (int i = 0; i < objResources.Length; i++)
            {
                int x = Mathf.FloorToInt(objResources[i].transform.position.x);
                int y = Mathf.FloorToInt(objResources[i].transform.position.y);

                _mapData.nodes[x, y].resource.type = (eResourceType)int.Parse(objResources[i].name.Split("_")[1]);
                _mapData.nodes[x, y].resource.quantity = int.Parse(objResources[i].name.Split("_")[2]);

            }

            for (int i = 0; i < objStartColor.Length; i++)
            {
                int x = Mathf.FloorToInt(objResources[i].transform.position.x);
                int y = Mathf.FloorToInt(objResources[i].transform.position.y);

                _mapData.nodes[x, y].startPosition.playerColor = (ePlayerColor)int.Parse(objResources[i].name.Split("_")[1]);
                _mapData.nodes[x, y].startPosition.team = int.Parse(objResources[i].name.Split("_")[2]);
            }

            yield return null;

            FileGenerate(_mapData);
            InitializeGrid(lenght);

            _isMakeGridDone = true;

            _coGrid = null;

            _buttonGenerate.gameObject.SetActive(true);
            _objLoading.gameObject.SetActive(false);
        }

        private void InitializeGrid(int value)
        {
            
        }

        private void  FileGenerate(MapData mapData)
        {
            string directory = EditorUtility.SaveFolderPanel("Export map data", "", mapData.name);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(mapData);

            string usc2FilePath = string.Empty;
            string textFilePath = directory + "/" + mapData.name + ".txt";
            string objFilePath = directory + "/" + "MapData" + ".txt";

            // text file
            var textfile = File.CreateText(textFilePath);
            textfile.Close();

            using (StreamWriter sw = new StreamWriter(textFilePath))
            {
                sw.Write(json);

                sw.Flush();
                sw.Close();
            }

            // terrain Data file

            //var objfile = File.CreateText(objFilePath);
            //objfile.Close();

            var guids = AssetDatabase.FindAssets(_terrainFild.terrainData.name, new[] { "Assets" });
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            assetPath = Application.dataPath + "/" + assetPath.Split("/")[1];
            objFilePath = directory + "/" + _terrainFild.terrainData.name + ".asset";

            File.Copy(assetPath, objFilePath, true);

            // zip
            DirectoryInfo di = new DirectoryInfo(directory + "/" + mapData.name);

            if (di.Exists == false)
            {
                di.Create();
                File.Move(textFilePath, directory + "/" + mapData.name + "/" + mapData.name + ".txt");
                File.Move(objFilePath, directory + "/" + mapData.name + "/" + _terrainFild.terrainData.name + ".asset");

                ZipFile.CreateFromDirectory(di.FullName, di.FullName + ".USC2");
                usc2FilePath = di.FullName + "USC2";

                di.Delete(true);

                Debug.Log("Done!!  save path : " + usc2FilePath);
            }
            else
            {
                Debug.LogError("False");
            }
        }

        private void CheckNode(ref Node node, bool isGenerate)
        {
            bool detected = false;

            node.resource = new Resource();
            node.startPosition = new StartPosition();
            node.topographic = new Topographic();

            Vector3 pos = new Vector3(node.x, 100f, node.y);

            if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                if(isGenerate == true)
                {
                    node.topographic.height = hit.point.y;

                    if (hit.collider.gameObject.tag.Equals(_resources))
                    {
                        detected = true;
                    }

                    if (hit.collider.gameObject.tag.Equals(_startPoint))
                    {
                        detected = true;
                    }

                    if (detected == true)
                    {
                        node.topographic.isWalkable = false;
                        return;
                    }
                }

                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 0.5f, NavMesh.AllAreas))
                {
                    node.topographic.isWalkable = true;
                    node.topographic.height = navHit.position.y;

                    if(navHit.position.y >= 1f)
                    {
                        node.topographic.isHill = true;
                    }
                }
            }
        }

        private void Generate()
        {
            _mapData = new MapData();

            if(_inputName.text.Length == 0)
            {
                Debug.LogError("The map name cannot be empty.");
                return;
            }

            if (_inputMaxPlayer.text.Length == 0)
            {
                Debug.LogError("The player cannot be empty.");
                return;
            }

            if (_inputMaxPlayer.text.Equals("0"))
            {
                Debug.LogError("The player cannot be zero.");
                return;
            }

            if (int.Parse(_inputMaxPlayer.text) > 6)
            {
                Debug.LogError("There cannot be more than 6 players.");
                return;
            }

            _mapData.name = _inputName.text;
            _mapData.description = _inputDescription.text;
            _mapData.maker = _inputMaker.text;
            _mapData.version = _inputVersion.text;
            _mapData.maxPlayer = int.Parse(_inputMaxPlayer.text);

            _mapData.mapSizeX = (int)_mapSize;
            _mapData.mapSizeY = (int)_mapSize;

            NavMeshSurface surface = _terrainFild.gameObject.GetComponentInChildren<NavMeshSurface>();
            if (surface.navMeshData == null)
            {
                surface.RemoveData();
                surface.BuildNavMesh();
            }

            if (_coGrid != null)
            {
                StopCoroutine(_coGrid);
            }

            _coGrid = StartCoroutine(Co_Grid((int)_terrainFild.terrainData.size.x, true));
        }
    }
}
