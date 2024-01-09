using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eUpgrade
{
    Non = -1,
    Attack,
    Defence,
    Shild
}

public struct Upgrade
{
    public eUpgrade upgrade;
    public eRace brood;
    public eUnitType unitType;
    public eUnitSizeType unitSizeType;
}

public interface IUnit
{
    public void Move(Vector3 targetPos);
    public void Stop();
    public void Attack(Vector3 targetPos);

    public void Attack(GameObject tamplate);

    public void Patrol(Vector3 targetPos);

    public void Hold();

    public void Custom_1();

    public void Custom_2();

    public void Custom_3();

    public void Custom_4();
}

public class InGameManager : MonoBehaviour, ISubject
{
    private static InGameManager _instance = null;

    [SerializeField]
    private CanvasScaler _canvasScaler = null;

    [SerializeField]
    private Camera _mainCamera = null;

    [SerializeField]
    private GameObject _objCameraArm = null;

    [SerializeField, Range(0.00f, 0.10f)]
    private float _cameraMoveSpeed = 0.04f;

    [SerializeField]
    private RectTransform _rttBox = null;

    [SerializeField]
    private GameObject _background = null;

    [SerializeField]
    private GameConsole _gameConsole = null;

    [SerializeField]
    private InGameResources _inGameResources = null;

    [SerializeField]
    private MapManager _mapManager = null;

    [SerializeField]
    private InputManager _inputManager = null;

    [SerializeField, Header("TEST")]
    private List<ObjectTamplate> _testUnitObjs = new List<ObjectTamplate>();

    [SerializeField]
    private List<ObjectTamplate> _testBuildingObjs = new List<ObjectTamplate>();

    private int _mineral = 0;
    private int _gas = 0;
    private int _maxPopulation = 0;
    private int _population = 0;

    private int layerMask_UI = 0;
    private int layerMask_Ground = 0;

    private Vector2 _leftMouseDownPos;
    private Vector2 _leftMouseHoldPos;

    private Dictionary<eObject, List<ObjectTamplate>> _objects = new Dictionary<eObject, List<ObjectTamplate>>();

    [SerializeField] // test
    private List<ObjectTamplate> _selectObjects = new List<ObjectTamplate>();
    private List<Vector3> _eventPos = new List<Vector3>(5);

    private Rect _rectSelectionBox;

    private bool _isStart = false;

    #region GetSet

    public static InGameManager instance
    {
        get
        { 
            if(_instance == null)
            {
                _instance = new InGameManager();
            }

            return _instance;
        }
    }

    public Camera mainCamera
    {
        get { return _mainCamera; }
    }

    public Vector2 cameraArm
    {
        set 
        {
            _objCameraArm.transform.position = new Vector3(value.x, _objCameraArm.transform.position.y, value.y);
        }
    }

    public bool isStart
    {
        get { return _isStart; }
    }

    public InputManager inputManager
    {
        get { return _inputManager; }
    }

    public int selectObjectCount
    {
        get { return _selectObjects.Count; }
    }

    public eFriendIdentification firstObjectFriendIdentificationType
    {
        get { return _selectObjects[0].friendIdentificationType; }
    }

    #endregion

    void Start()
    {
        _instance = this;

        _background.SetActive(true);

        _gameConsole.Initialize(Order);
        _inGameResources.Initialize();
        _mapManager.Initialize(IsReady);
        _inputManager.Initialize();

        GameManager.instance.toolManager.InstantiateObjectPool();

        _objects[eObject.Building] = new List<ObjectTamplate>();
        _objects[eObject.Unit] = new List<ObjectTamplate>();

        _canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);

        layerMask_UI = LayerMask.NameToLayer("UI");
        layerMask_Ground = LayerMask.NameToLayer("Ground");

        _rttBox.gameObject.SetActive(false);

        _mapManager.InstantiateMap();

        if (GameManager.instance.TEST_MODE == true)
        {
            foreach (var item in _testBuildingObjs)
            {
                _objects[eObject.Building].Add(item);
            }

            foreach (var item in _testUnitObjs)
            {
                _objects[eObject.Unit].Add(item);
            }
        }
    }

    private void IsReady()
    {
        SetInput();

        _background.SetActive(false);

        _isStart = true;
    }

    private void SetInput()
    {
        // Mouse
        _inputManager.SetMouseKey(eMouseInput.Left, eClickType.Down, () =>
        {
            if (_inputManager.isOrder_Patrol == true || _inputManager.isOrder_Attack == true)
            {
                _leftMouseDownPos = Vector2.zero;
                _leftMouseHoldPos = Vector2.zero;
                return;
            }

            _leftMouseDownPos = Input.mousePosition;
            _leftMouseHoldPos = Input.mousePosition;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.TryGetComponent(out ObjectTamplate tamplate))
                {
                    if(_inputManager.isPress_L_Ctrl == true)
                    {
                        _rectSelectionBox = new Rect();

                        _rectSelectionBox.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                        _rttBox.sizeDelta = new Vector2(Screen.width, Screen.height);

                        _rectSelectionBox.xMin = 0;
                        _rectSelectionBox.xMax = Screen.width;

                        _rectSelectionBox.yMin = 0;
                        _rectSelectionBox.yMax = Screen.height;

                        foreach (var item in _selectObjects)
                        {
                            item.SetSelection(false);
                        }

                        _selectObjects.Clear();

                        for (int i = 0; i < 16; i++)
                        {
                            if (i > _objects[eObject.Unit].Count - 1)
                            {
                                break;
                            }

                            if (_rectSelectionBox.Contains(_mainCamera.WorldToScreenPoint(_objects[eObject.Unit][i].transform.position)))
                            {
                                if (_objects[eObject.Unit][i].friendIdentificationType == eFriendIdentification.My)
                                {
                                    if(tamplate.GetData().key.Equals(_objects[eObject.Unit][i].GetData().key))
                                    {
                                        _selectObjects.Add(_objects[eObject.Unit][i]);
                                        _objects[eObject.Unit][i].SetSelection(true);
                                    }
                                }
                            }
                        }
                    }
                    else if(_inputManager.isPress_L_Shift == false)
                    {
                        foreach (var item in _selectObjects)
                        {
                            item.SetSelection(false);
                        }

                        _selectObjects.Clear();

                        _selectObjects.Add(tamplate);
                        tamplate.SetSelection(true);
                    }

                }
            }
        });

        _inputManager.SetMouseKey(eMouseInput.Left, eClickType.Hold, () =>
        {
            if (_inputManager.isOrder_Patrol == true || _inputManager.isOrder_Attack == true)
            {
                _leftMouseDownPos = Vector2.zero;
                _leftMouseHoldPos = Vector2.zero;
                return;
            }

            _leftMouseHoldPos = Input.mousePosition;

            if (Vector2.Distance(_leftMouseHoldPos, _leftMouseDownPos) > 1f)
            {
                _rttBox.gameObject.SetActive(true);
            }

            DrawBox();
        });

        _inputManager.SetMouseKey(eMouseInput.Left, eClickType.Up, () =>
        {
            if(_inputManager.isOrder_Patrol == true || _inputManager.isOrder_Attack == true)
            {
                _leftMouseDownPos = Vector2.zero;
                _leftMouseHoldPos = Vector2.zero;

                _rectSelectionBox = new Rect();
                _rttBox.gameObject.SetActive(false);
                return;
            }

            if(_rttBox.gameObject.activeSelf == true)
            {
                DrawSelection();
                SelectUnit();

                _rttBox.gameObject.SetActive(false);
            }

            _leftMouseDownPos = Vector2.zero;
            _leftMouseHoldPos = Vector2.zero;
            _rttBox.sizeDelta = Vector2.zero;
            _rttBox.position = Vector3.zero;

        });

        _inputManager.SetMouseKey(eMouseInput.Right, eClickType.Down, () =>
        {
            if (_inputManager.isOrder_Patrol == true || _inputManager.isOrder_Attack == true)
            {
                return;
            }

            if (_selectObjects.Count == 0)
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Ground))
            {
                if (_selectObjects.Count == 1)
                {
                    if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Alliance)
                    {
                        return;
                    }

                    if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Enemy)
                    {
                        return;
                    }
                }

                foreach (var u in _selectObjects)
                {
                    u.Move(hit.point);
                }
            }
        });

        // keyboard
        _inputManager.SetKeyboardKey(KeyCode.M, eClickType.Down, eOrder.Non, () =>
        {
            _gameConsole.Order(eOrder.Move);
        });

        _inputManager.SetKeyboardKey(KeyCode.S, eClickType.Down, eOrder.Non, () =>
        {
            _gameConsole.Order(eOrder.Stop);
        });

        _inputManager.SetKeyboardKey(KeyCode.A, eClickType.Down, eOrder.Attack, () =>
        {
            _gameConsole.Order(eOrder.Attack);
        });

        _inputManager.SetKeyboardKey(KeyCode.P, eClickType.Down, eOrder.Patrol, () =>
        {
            _gameConsole.Order(eOrder.Patrol);
        });

        _inputManager.SetKeyboardKey(KeyCode.H, eClickType.Down, eOrder.Non, () =>
        {
            _gameConsole.Order(eOrder.Hold);
        });

        _inputManager.SetKeyboardKey(KeyCode.LeftShift, eClickType.Down, eOrder.Non, () =>
        {

        });

        _inputManager.SetKeyboardKey(KeyCode.LeftControl, eClickType.Down, eOrder.Non, () =>
        {
            
        });

        _inputManager.SetMove(eDirection.Up, () => { _objCameraArm.transform.position += Vector3.forward * _cameraMoveSpeed; });

        _inputManager.SetMove(eDirection.Down, () => { _objCameraArm.transform.position += Vector3.back * _cameraMoveSpeed; });

        _inputManager.SetMove(eDirection.Left, () => { _objCameraArm.transform.position += Vector3.left * _cameraMoveSpeed; });

        _inputManager.SetMove(eDirection.Right, () => { _objCameraArm.transform.position += Vector3.right * _cameraMoveSpeed; });
    }

    private void DrawBox()
    {
        if (_inputManager.isOrder_Patrol == true || _inputManager.isOrder_Attack == true)
        {
            return;
        }

        if(_rttBox.gameObject.activeSelf == false)
        {
            return;
        }

        Vector2 boxStart = _leftMouseDownPos;
        Vector2 boxEnd = _leftMouseHoldPos;

        Vector2 boxCenter = (boxStart + boxEnd) * 0.5f;
        _rttBox.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        _rttBox.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        if(Vector2.Distance(_leftMouseHoldPos, _leftMouseDownPos) < 1f)
        {
            return;
        }

        _rectSelectionBox = new Rect();

        if (_leftMouseHoldPos.x < _leftMouseDownPos.x)
        {
            _rectSelectionBox.xMin = _leftMouseHoldPos.x;
            _rectSelectionBox.xMax = _leftMouseDownPos.x;
        }
        else
        {
            _rectSelectionBox.xMin = _leftMouseDownPos.x;
            _rectSelectionBox.xMax = _leftMouseHoldPos.x;
        }

        if (_leftMouseHoldPos.y < _leftMouseDownPos.y)
        {
            _rectSelectionBox.yMin = _leftMouseHoldPos.y;
            _rectSelectionBox.yMax = _leftMouseDownPos.y;
        }
        else
        {
            _rectSelectionBox.yMin = _leftMouseDownPos.y;
            _rectSelectionBox.yMax = _leftMouseHoldPos.y;
        }
    }

    private void SelectUnit()
    {
        List<ObjectTamplate> select = new List<ObjectTamplate>();

        bool isUnit = false;
        bool isMy = false;

        bool shift = _inputManager.isPress_L_Shift;
        bool ctrl = _inputManager.isPress_L_Ctrl;

        if (shift == true)
        {
            int count = 16 - _selectObjects.Count;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (i > _objects[eObject.Unit].Count - 1)
                    {
                        break;
                    }

                    if (_rectSelectionBox.Contains(_mainCamera.WorldToScreenPoint(_objects[eObject.Unit][i].transform.position)))
                    {
                        if (_objects[eObject.Unit][i].friendIdentificationType == eFriendIdentification.My)
                        {
                            isUnit = true;
                            select.Add(_objects[eObject.Unit][i]);
                        }
                    }
                }
            }
        }
        else if (ctrl == true)
        {

        }
        else
        {
            for (int i = 0; i < 16; i++)
            {
                if (i > _objects[eObject.Unit].Count - 1)
                {
                    break;
                }

                if (_rectSelectionBox.Contains(_mainCamera.WorldToScreenPoint(_objects[eObject.Unit][i].transform.position)))
                {
                    if (_objects[eObject.Unit][i].friendIdentificationType == eFriendIdentification.My)
                    {
                        isUnit = true;
                        isMy = true;
                        select.Add(_objects[eObject.Unit][i]);
                    }

                    if (isMy == true)
                    {
                        continue;
                    }

                    if (_objects[eObject.Unit][i].friendIdentificationType == eFriendIdentification.Enemy)
                    {
                        isUnit = true;
                        select.Add(_objects[eObject.Unit][i]);

                        break;
                    }
                    else if (_objects[eObject.Unit][i].friendIdentificationType == eFriendIdentification.Alliance)
                    {
                        isUnit = true;
                        select.Add(_objects[eObject.Unit][i]);

                        break;
                    }
                }
            }
        }

        if(select.Count > 0)
        {
            if(shift == true)
            {
                foreach (var item in _selectObjects)
                {
                    item.SetSelection(false);
                }

                _selectObjects.Clear();
            }
            else if (ctrl == true)
            {

            }
            else
            {
                foreach (var item in _selectObjects)
                {
                    item.SetSelection(false);
                }

                _selectObjects.Clear();
            }

            _selectObjects.AddRange(select);

            foreach (var one in _selectObjects)
            {
                one.SetSelection(true);
            }

            _gameConsole.SetInformation(_selectObjects, ObjectTamplateUpdateKey);
            _gameConsole.SetControlPanal(_selectObjects);
        }

        if (isUnit == true)
        {
            return;
        }

        if(shift == true)
        {
            return;
        }

        foreach (var building in _objects[eObject.Building])
        {
            if(building == null)
            {
                continue;
            }

            if (_rectSelectionBox.Contains(_mainCamera.WorldToScreenPoint(building.transform.position)))
            {
                _selectObjects.Add(building);
                break;
            }
        }
    }

    private void ObjectTamplateUpdateKey(ObjectData data)
    {
        if(data.objType == eObject.Non)
        {
            return;
        }

        foreach (var one in _objects[data.objType])
        {
            if(data._id.Equals(one.GetData()._id))
            {
                one.UpdateData(data);
            }
        }
    }

    private void Order(eOrder order)
    {
        foreach (var u in _selectObjects)
        {
            switch (order)
            {
                case eOrder.Move:
                    {
                        if (_selectObjects.Count == 1)
                        {
                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Alliance)
                            {
                                return;
                            }

                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Enemy)
                            {
                                return;
                            }
                        }

                        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Ground))
                        {
                            foreach (var u1 in _selectObjects)
                            {
                                u1.Move(hit.point);
                            }
                        }
                    }
                    break;

                case eOrder.Stop:
                    {
                        if (_selectObjects.Count == 1)
                        {
                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Alliance)
                            {
                                return;
                            }

                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Enemy)
                            {
                                return;
                            }
                        }

                        foreach (var u1 in _selectObjects)
                        {
                            u1.Stop();
                        }
                    }
                    break;

                case eOrder.Attack:
                    {
                        _leftMouseDownPos = Vector2.zero;
                        _leftMouseHoldPos = Vector2.zero;

                        if (_selectObjects.Count == 1)
                        {
                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Alliance)
                            {
                                return;
                            }

                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Enemy)
                            {
                                return;
                            }
                        }

                        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Ground))
                        {
                            foreach (var u1 in _selectObjects)
                            {
                                u1.Attack(hit.point);
                            }
                        }
                    }
                    break;

                case eOrder.Patrol:
                    {
                        _leftMouseDownPos = Vector2.zero;
                        _leftMouseHoldPos = Vector2.zero;

                        if (_selectObjects.Count == 1)
                        {
                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Alliance)
                            {
                                return;
                            }

                            if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Enemy)
                            {
                                return;
                            }
                        }

                        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Ground))
                        {
                            foreach (var u1 in _selectObjects)
                            {
                                u1.Patrol(hit.point);
                            }
                        }
                    }
                    break;

                case eOrder.Hold:
                    if (_selectObjects.Count == 1)
                    {
                        if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Alliance)
                        {
                            return;
                        }

                        if (_selectObjects[0].friendIdentificationType == eFriendIdentification.Enemy)
                        {
                            return;
                        }
                    }

                    foreach (var u1 in _selectObjects)
                    {
                        u1.Hold();
                    }
                    break;

                case eOrder.Custom_1:
                    u.Custom_1();
                    break;

                case eOrder.Custom_2:
                    u.Custom_2();
                    break;

                case eOrder.Custom_3:
                    u.Custom_3();
                    break;

                case eOrder.Custom_4:
                    u.Custom_4();
                    break;
            }
        }
    }

    public void SelectAdd(ObjectTamplate tamplate)
    {
        if (tamplate.GetData().objType == eObject.Building)
        {
            _selectObjects.Clear();
            _selectObjects.Add(tamplate);

            return;
        }

        if (_selectObjects.Count >= 16)
        {
            return;
        }

        _selectObjects.Add(tamplate);
    }

    public void SelectRemove(ObjectTamplate tamplate)
    {
        if (_selectObjects.Count == 1)
        {
            return;
        }

        _selectObjects.Remove(tamplate);
    }

    public void UpdateMineral(bool isPlus, int updateValue, Action<bool> onResult)
    {
        if (isPlus == true)
        {
            _mineral += updateValue;

            onResult?.Invoke(true);
        }
        else if (isPlus == false)
        {
            if(_mineral - updateValue < 0)
            {
                onResult?.Invoke(false);

                return;
            }

            _mineral -= updateValue;
        }

        UpdateResources();
    }

    public void UpdateGas(bool isPlus, int updateValue, Action<bool> onResult)
    {
        if (isPlus == true)
        {
            _gas += updateValue;

            onResult?.Invoke(true);
        }
        else if (isPlus == false)
        {
            if (_gas - updateValue < 0)
            {
                onResult?.Invoke(false);

                return;
            }

            _gas -= updateValue;
        }

        UpdateResources();
    }

    public void UpdateMaxPopulation(bool isPlus, int updateValue)
    {
        if(isPlus == true)
        {
            if(_maxPopulation + updateValue >= 200)
            {
                _maxPopulation = 200;
                UpdateResources();

                return;
            }

            _maxPopulation += updateValue;
        }
        else if(isPlus == false)
        {
            if(_maxPopulation - updateValue <= 0)
            {
                _maxPopulation = 0;
                UpdateResources();

                return;
            }

            _maxPopulation -= updateValue;
        }

        UpdateResources();
    }

    public void UpdatePopulation(int population, Action<bool> onResult)
    {
        if(_population <= population)
        {
            return;
        }

        if(_population + population > _maxPopulation)
        {
            onResult?.Invoke(false);

            return;
        }

        _population += population;

        onResult?.Invoke(true);
        UpdateResources();
    }

    private void UpdateResources()
    {
        _inGameResources.SetResources(_mineral, _gas, _population, _maxPopulation);
    }

    public void Upgrade(Upgrade upgrade)
    {
        for (int i = 0; i < _objects[eObject.Unit].Count; i++)
        {
            IObserver data = _objects[eObject.Unit][i];

            if (upgrade.brood == data.BroodType() && upgrade.unitType == data.UnitType() && upgrade.unitSizeType == data.UnitSizeType())
            {
                _objects[eObject.Unit][i].SetUpgrade(upgrade.upgrade);
            }

            _gameConsole.Upgrade(_objects[eObject.Unit][i]);
        }
    }

    public void Add(eObject type, IObserver data)
    {
        //_objects[type].Add(data);
    }

    public void Remove(eObject type, IObserver data)
    {
        //_objects[type].Remove(data);
    }

    public void Notify(eObject type)
    {
        //for (int i = 0; i < _objects[type].Count; i++)
        //{
        //    _objects[type][i].UpdateData();
        //}
    }
}
