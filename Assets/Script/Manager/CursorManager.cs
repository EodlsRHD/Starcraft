using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public enum eCursorType
{
    Non = -1,
    Defult,
    SelectBox,
    Denied,
    Move,
    Point,
    PointAttack,
    BuildingSelect
}

public enum eFriendIdentification
{
    Non = -1,
    My,
    Alliance,
    Enemy
}

public enum eDirection
{
    Non = -1,
    Up,
    LeftUp,
    Left,
    LeftDown,
    Down,
    RightDown,
    Right,
    RightUp
}

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Transform _trCursorParant = null;

    [SerializeField]
    private GameObject _prefabCursor = null;

    [Space(10)]

    [SerializeField]
    private List<Sprite> _defult = null;

    [SerializeField]
    private List<Sprite> _select = null;

    [SerializeField]
    private List<Sprite> _denied = null;

    [Header("Move")]

    [SerializeField]
    private List<Sprite> _moveUp = null;

    [SerializeField]
    private List<Sprite> _moveLeftUp = null;

    [SerializeField]
    private List<Sprite> _moveLeft = null;

    [SerializeField]
    private List<Sprite> _moveLeftDown = null;

    [SerializeField]
    private List<Sprite> _moveDown = null;

    [SerializeField]
    private List<Sprite> _moveRightDown = null;

    [SerializeField]
    private List<Sprite> _moveRight = null;

    [SerializeField]
    private List<Sprite> _moveRightUp = null;

    [Header("point")]

    [SerializeField]
    private List<Sprite> _pointMe = null;

    [SerializeField]
    private List<Sprite> _pointAlliance = null;

    [SerializeField]
    private List<Sprite> _pointEnemy = null;

    [Header("point Attack")]

    [SerializeField]
    private List<Sprite> _pointAttackMe = null;

    [SerializeField]
    private List<Sprite> _pointAttackAlliance = null;

    [SerializeField]
    private List<Sprite> _pointAttackEnemy = null;

    [Header("Building Select")]

    [SerializeField]
    private List<Sprite> _buildingSelectMe = null;

    [SerializeField]
    private List<Sprite> _buildingSelectAlliance = null;

    [SerializeField]
    private List<Sprite> _buildingSelectEnemy = null;

    private List<Sprite> _currentSprits = new List<Sprite>();

    private Image _imageCursor = null;

    private CancellationTokenSource disableCancellation = new CancellationTokenSource();

    private bool _isStart = false;

    private eCursorType _currentCursorType = eCursorType.Non;
    private eFriendIdentification _currentFriendIdentification = eFriendIdentification.Non;
    private eDirection _currentDirection = eDirection.Non;

    private eCursorType _beforeCursorType = eCursorType.Non;
    private eFriendIdentification _beforeFriendIdentification = eFriendIdentification.Non;
    private eDirection _beforeDirection = eDirection.Non;

    public void Initialize()
    {
        InstantiateCursor();
    }

    public void ChangeParant(Transform tr)
    {
        if(_imageCursor == null)
        {
            InstantiateCursor();
        }

        _imageCursor.gameObject.transform.SetParent(tr);
    }

    private void InstantiateCursor()
    {
        GameObject obj = Instantiate(_prefabCursor, _trCursorParant);
        _imageCursor = obj.GetComponent<Image>();

        ChangeCursor(eCursorType.Defult);

        Cursor().Forget();
    }

    private void SaveCurrentCursor(eCursorType cursorType)
    {
        _currentCursorType = cursorType;
    }

    private void SaveCurrentCursor(eCursorType cursorType, eFriendIdentification friendIdentification)
    {
        _currentCursorType = cursorType;
        _currentFriendIdentification = friendIdentification;
    }

    private void SaveCurrentCursor(eCursorType cursorType, eDirection direction)
    {
        _currentCursorType = cursorType;
        _currentDirection = direction;
    }

    private void SaveBeforeCursor()
    {
        _beforeCursorType = _currentCursorType;
        _beforeFriendIdentification = _currentFriendIdentification;
        _beforeDirection = _currentDirection;
    }

    public void BeforeCursor()
    {
        if(_beforeFriendIdentification != eFriendIdentification.Non)
        {
            ChangeCursor(_beforeCursorType, _beforeFriendIdentification);
            return;
        }

        if (_beforeDirection != eDirection.Non)
        {
            ChangeCursor(_beforeCursorType, _beforeDirection);
            return;
        }

        if(_beforeCursorType == eCursorType.Non)
        {
            ChangeCursor(eCursorType.Defult);
            return;
        }

        ChangeCursor(_beforeCursorType);
    }

    public void DefultCursor()
    {
        _isStart = false;
        _currentSprits.Clear();

        SaveBeforeCursor();
        SaveCurrentCursor(eCursorType.Defult);

        _currentFriendIdentification = eFriendIdentification.Non;
        _currentDirection = eDirection.Non;

        _currentSprits.AddRange(_defult);

        _isStart = true;
    }

    public void ChangeCursor(eCursorType type)
    {
        if (_currentCursorType == eCursorType.SelectBox)
        {
            return;
        }

        if (_currentCursorType == type)
        {
            return;
        }

        _isStart = false;
        _currentSprits.Clear();

        SaveBeforeCursor();
        SaveCurrentCursor(type);

        switch (type)
        {
            case eCursorType.Defult:
                _currentSprits.AddRange(_defult);
                break;

            case eCursorType.SelectBox:
                _currentSprits.AddRange(_select);
                break;

            case eCursorType.Denied:
                _currentSprits.AddRange(_denied);
                break;
        }

        _isStart = true;
    }

    public void ChangeCursor(eCursorType type, eFriendIdentification friendIdentification)
    {
        if (InGameManager.instance.inputManager.isLeftMouseClick == true)
        {
            return;
        }

        _isStart = false;
        _currentSprits.Clear();

        SaveBeforeCursor();
        SaveCurrentCursor(type, friendIdentification);

        switch (type)
        {
            case eCursorType.Point:
                {
                    switch(friendIdentification)
                    {
                        case eFriendIdentification.My:
                            _currentSprits.AddRange(_pointMe);
                            break;

                        case eFriendIdentification.Alliance:
                            _currentSprits.AddRange(_pointAlliance);
                            break;

                        case eFriendIdentification.Enemy:
                            _currentSprits.AddRange(_pointEnemy);
                            break;
                    }
                }
                break;

            case eCursorType.PointAttack:
                {
                    switch (friendIdentification)
                    {
                        case eFriendIdentification.My:
                            _currentSprits.AddRange(_pointAttackMe);
                            break;

                        case eFriendIdentification.Alliance:
                            _currentSprits.AddRange(_pointAttackAlliance);
                            break;

                        case eFriendIdentification.Enemy:
                            _currentSprits.AddRange(_pointAttackEnemy);
                            break;
                    }
                }
                break;

            case eCursorType.BuildingSelect:
                {
                    switch (friendIdentification)
                    {
                        case eFriendIdentification.My:
                            _currentSprits.AddRange(_buildingSelectMe);
                            break;

                        case eFriendIdentification.Alliance:
                            _currentSprits.AddRange(_buildingSelectAlliance);
                            break;

                        case eFriendIdentification.Enemy:
                            _currentSprits.AddRange(_buildingSelectEnemy);
                            break;
                    }
                }
                break;
        }

        _isStart = true;
    }

    public void ChangeCursor(eCursorType type, eDirection direction)
    {
        if (InGameManager.instance.inputManager.isLeftMouseClick == true)
        {
            return;
        }

        if (_currentDirection == direction)
        {
            return;
        }

        _isStart = false;
        _currentSprits.Clear();

        SaveBeforeCursor();
        SaveCurrentCursor(type, direction);

        if (type == eCursorType.Move)
        {
            switch(direction)
            {
                case eDirection.Up:
                    _currentSprits.AddRange(_moveUp);
                    break;

                case eDirection.LeftUp:
                    _currentSprits.AddRange(_moveLeftUp);
                    break;

                case eDirection.Left:
                    _currentSprits.AddRange(_moveLeft);
                    break;

                case eDirection.LeftDown:
                    _currentSprits.AddRange(_moveLeftDown);
                    break;

                case eDirection.Down:
                    _currentSprits.AddRange(_moveDown);
                    break;

                case eDirection.RightDown:
                    _currentSprits.AddRange(_moveRightDown);
                    break;

                case eDirection.Right:
                    _currentSprits.AddRange(_moveRight);
                    break;

                case eDirection.RightUp:
                    _currentSprits.AddRange(_moveRightUp);
                    break;
            }
        }

        _isStart = true;
    }

    private async UniTaskVoid Cursor()
    {
        while(true)
        {
            await UniTask.Yield();

            if (_isStart == false)
            {
                continue;
            }

            if(_imageCursor == null)
            {
                continue;
            }

            if(_currentSprits.Count == 0)
            {
                continue;
            }

            for (int i = 0; i < _currentSprits.Count; i++)
            {
                if (_imageCursor == null)
                {
                    continue;
                }

                _imageCursor.sprite = _currentSprits[i];

                await UniTask.Delay(TimeSpan.FromSeconds(0.1f), false, PlayerLoopTiming.Update, disableCancellation.Token);
            }

            InfiniteLoopDetector.Run();
        }
    }

    private void Update()
    {
        if(_imageCursor != null)
        {
            _imageCursor.transform.position = Input.mousePosition;
        }
    }

    public void Quit()
    {
        if(disableCancellation.Token == null)
        {
            return;
        }

        disableCancellation.Cancel();
        disableCancellation.Dispose();

        disableCancellation = new CancellationTokenSource();
    }
}
