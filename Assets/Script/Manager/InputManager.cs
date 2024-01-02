using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum eClickType
{
    Non = -1,
    Down,
    Hold,
    Up
}

public enum eMouseInput
{
    Non = -1,
    Left,
    Right,
    Wheel
}

public class InputManager : MonoBehaviour
{
    private const string _tagBuilding = "Building";
    private const string _tagUnit = "Unit";

    private Vector2 _mousePosition;

    public bool isOrder_Move = false;
    public bool isOrder_Stop = false;
    public bool isOrder_Attack = false;
    public bool isOrder_Hold = false;
    public bool isOrder_Patrol = false;

    public bool _isOrdering = false;

    [Space(10)]

    #region is Press

    public bool isLeftMouseClick = false;
    public bool isRightMouseClick = false;
    public bool isWhillMouseClick = false;

    public bool osPress_F2 = false;
    public bool osPress_F3 = false;
    public bool osPress_F4 = false;

    public bool osPress_1 = false;
    public bool osPress_2 = false;
    public bool osPress_3 = false;
    public bool osPress_4 = false;
    public bool osPress_5 = false;
    public bool osPress_6 = false;
    public bool osPress_7 = false;
    public bool osPress_8 = false;
    public bool osPress_9 = false;
    public bool osPress_0 = false;

    public bool isPress_Q = false;
    public bool isPress_W = false;
    public bool isPress_E = false;
    public bool isPress_R = false;
    public bool isPress_T = false;
    public bool isPress_Y = false;
    public bool isPress_U = false;
    public bool isPress_I = false;
    public bool isPress_O = false;
    public bool isPress_P = false;

    public bool isPress_A = false;
    public bool isPress_S = false;
    public bool isPress_D = false;
    public bool isPress_F = false;
    public bool isPress_G = false;
    public bool isPress_H = false;
    public bool isPress_J = false;
    public bool isPress_K = false;
    public bool isPress_L = false;

    public bool isPress_Z = false;
    public bool isPress_X = false;
    public bool isPress_C = false;
    public bool isPress_V = false;
    public bool isPress_B = false;
    public bool isPress_N = false;
    public bool isPress_M = false;

    public bool isPress_Space = false;
    public bool isPress_L_Shift = false;
    public bool isPress_L_Ctrl = false;

    #endregion

    [Space(10)]

    #region Action

    private Action _onMouseL_Down;
    private Action _onMouseL_Hold;
    private Action _onMouseL_Up;

    private Action _onMouseR_Down;
    private Action _onMouseR_Hold;
    private Action _onMouseR_Up;

    private Action _onMouseW;

    private Action _on_Down_F2;
    private Action _on_Down_F3;
    private Action _on_Down_F4;

    private Action _on_Hold_F2;
    private Action _on_Hold_F3;
    private Action _on_Hold_F4;

    private Action _on_Up_F2;
    private Action _on_Up_F3;
    private Action _on_Up_F4;

    private Action _on_Down_Alpha1;
    private Action _on_Down_Alpha2;
    private Action _on_Down_Alpha3;
    private Action _on_Down_Alpha4;
    private Action _on_Down_Alpha5;
    private Action _on_Down_Alpha6;
    private Action _on_Down_Alpha7;
    private Action _on_Down_Alpha8;
    private Action _on_Down_Alpha9;
    private Action _on_Down_Alpha0;

    private Action _on_Hold_Alpha1;
    private Action _on_Hold_Alpha2;
    private Action _on_Hold_Alpha3;
    private Action _on_Hold_Alpha4;
    private Action _on_Hold_Alpha5;
    private Action _on_Hold_Alpha6;
    private Action _on_Hold_Alpha7;
    private Action _on_Hold_Alpha8;
    private Action _on_Hold_Alpha9;
    private Action _on_Hold_Alpha0;

    private Action _on_Up_Alpha1;
    private Action _on_Up_Alpha2;
    private Action _on_Up_Alpha3;
    private Action _on_Up_Alpha4;
    private Action _on_Up_Alpha5;
    private Action _on_Up_Alpha6;
    private Action _on_Up_Alpha7;
    private Action _on_Up_Alpha8;
    private Action _on_Up_Alpha9;
    private Action _on_Up_Alpha0;

    private Action _on_Down_Q;
    private Action _on_Down_W;
    private Action _on_Down_E;
    private Action _on_Down_R;
    private Action _on_Down_T;
    private Action _on_Down_Y;
    private Action _on_Down_U;
    private Action _on_Down_I;
    private Action _on_Down_O;
    private Action _on_Down_P;

    private Action _on_Hold_Q;
    private Action _on_Hold_W;
    private Action _on_Hold_E;
    private Action _on_Hold_R;
    private Action _on_Hold_T;
    private Action _on_Hold_Y;
    private Action _on_Hold_U;
    private Action _on_Hold_I;
    private Action _on_Hold_O;
    private Action _on_Hold_P;

    private Action _on_Up_Q;
    private Action _on_Up_W;
    private Action _on_Up_E;
    private Action _on_Up_R;
    private Action _on_Up_T;
    private Action _on_Up_Y;
    private Action _on_Up_U;
    private Action _on_Up_I;
    private Action _on_Up_O;
    private Action _on_Up_P;

    private Action _on_Down_A;
    private Action _on_Down_S;
    private Action _on_Down_D;
    private Action _on_Down_F;
    private Action _on_Down_G;
    private Action _on_Down_H;
    private Action _on_Down_J;
    private Action _on_Down_K;
    private Action _on_Down_L;

    private Action _on_Hold_A;
    private Action _on_Hold_S;
    private Action _on_Hold_D;
    private Action _on_Hold_F;
    private Action _on_Hold_G;
    private Action _on_Hold_H;
    private Action _on_Hold_J;
    private Action _on_Hold_K;
    private Action _on_Hold_L;

    private Action _on_Up_A;
    private Action _on_Up_S;
    private Action _on_Up_D;
    private Action _on_Up_F;
    private Action _on_Up_G;
    private Action _on_Up_H;
    private Action _on_Up_J;
    private Action _on_Up_K;
    private Action _on_Up_L;

    private Action _on_Down_Z;
    private Action _on_Down_X;
    private Action _on_Down_C;
    private Action _on_Down_V;
    private Action _on_Down_B;
    private Action _on_Down_N;
    private Action _on_Down_M;

    private Action _on_Hold_Z;
    private Action _on_Hold_X;
    private Action _on_Hold_C;
    private Action _on_Hold_V;
    private Action _on_Hold_B;
    private Action _on_Hold_N;
    private Action _on_Hold_M;

    private Action _on_Up_Z;
    private Action _on_Up_X;
    private Action _on_Up_C;
    private Action _on_Up_V;
    private Action _on_Up_B;
    private Action _on_Up_N;
    private Action _on_Up_M;

    private Action _onSpace;
    private Action _onShift_L;
    private Action _onCtrl_L;

    #endregion

    private Action _onOrderAttack = null;
    private Action _onOrderPatrol = null;

    private Rect _side_Up;
    private Rect _side_Down;
    private Rect _side_Left;
    private Rect _side_Right;

    private bool _in_Up = false;
    private bool _in_Down = false;
    private bool _in_Left = false;
    private bool _in_Right = false;

    private Action _on_SideMove_Up = null;
    private Action _on_SideMove_Down = null;
    private Action _on_SideMove_Left = null;
    private Action _on_SideMove_Right = null;


    public void Initialize()
    {
        _side_Up = new Rect();
        _side_Down = new Rect();
        _side_Left = new Rect();
        _side_Right = new Rect();

        _side_Up.xMin = 0;
        _side_Up.xMax = Screen.width;
        _side_Up.yMin = Screen.height * 0.95f;
        _side_Up.yMax = Screen.height;

        _side_Down.xMin = 0;
        _side_Down.xMax = Screen.width;
        _side_Down.yMin = 0;
        _side_Down.yMax = Screen.height * 0.05f;

        _side_Left.xMin = 0;
        _side_Left.xMax = Screen.width * 0.05f;
        _side_Left.yMin = 0;
        _side_Left.yMax = Screen.height;

        _side_Right.xMin = Screen.width * 0.95f;
        _side_Right.xMax = Screen.width;
        _side_Right.yMin = 0;
        _side_Right.yMax = Screen.height;
    }

    private void Update()
    {
        if(InGameManager.instance.isStart == false)
        {
            return;
        }

        _mousePosition = Input.mousePosition;

        Input_Function();
        Input_Number();
        Input_Q_P();
        Input_A_L();
        Input_Z_M();
        InputSpecial();
        Input_Mouse();


        SideArrow(_mousePosition);
        ChackMousePoint(_mousePosition);

        _in_Up = false;
        _in_Down = false;
        _in_Left = false;
        _in_Right = false;
    }

    private void SideArrow(Vector2 mousePosition)
    {
        bool detected = false;

        if (_side_Up.Contains(mousePosition))
        {
            _in_Up = true;

            detected = true;
        }

        if (_side_Down.Contains(mousePosition))
        {
            _in_Down = true;

            detected = true;
        }

        if (_side_Left.Contains(mousePosition))
        {
            _in_Left = true;

            detected = true;
        }

        if (_side_Right.Contains(mousePosition))
        {
            _in_Right = true;

            detected = true;
        }

        if (detected == false)
        {
            GameManager.instance.DefultCursor();
            return;
        }

        if (_in_Up == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.Up);
            _on_SideMove_Up?.Invoke();
        }
        else if (_in_Down == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.Down);
            _on_SideMove_Down?.Invoke();
        }
        else if (_in_Left == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.Left);
            _on_SideMove_Left?.Invoke();
        }
        else if (_in_Right == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.Right);
            _on_SideMove_Right?.Invoke();
        }
        if (_in_Up == true && _in_Left == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.LeftUp);
            _on_SideMove_Up?.Invoke();
            _on_SideMove_Left?.Invoke();
        }
        else if (_in_Up == true && _in_Right == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.RightUp);
            _on_SideMove_Up?.Invoke();
            _on_SideMove_Right?.Invoke();
        }
        else if (_in_Down == true && _in_Left == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.LeftDown);
            _on_SideMove_Down?.Invoke();
            _on_SideMove_Left?.Invoke();
        }
        else if (_in_Down == true && _in_Right == true)
        {
            GameManager.instance.ChangeCursor(eCursorType.Move, eDirection.RightDown);
            _on_SideMove_Down?.Invoke();
            _on_SideMove_Right?.Invoke();
        }
    }

    private void ChackMousePoint(Vector2 mousePosition)
    {
        if (_in_Up == true || _in_Down == true || _in_Left == true || _in_Right == true)
        {
            return;
        }

        Ray ray = InGameManager.instance.mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) == false)
        {
            return;
        }

        ChangeCursor(hit);
    }

    private void ChangeCursor(RaycastHit hit)
    {
        hit.collider.gameObject.TryGetComponent(out ObjectTamplate tamplate);

        Key();
        
        if (hit.collider.gameObject.tag.Equals(_tagBuilding) || hit.collider.gameObject.tag.Equals(_tagUnit))
        {
            if (isOrder_Patrol == true)
            {
                GameManager.instance.ChangeCursor(eCursorType.Point, tamplate.friendIdentificationType);
            }
            else if (isOrder_Attack == true)
            {
                GameManager.instance.ChangeCursor(eCursorType.PointAttack, tamplate.friendIdentificationType);
            }
            else
            {
                GameManager.instance.ChangeCursor(eCursorType.BuildingSelect, tamplate.friendIdentificationType);
            }
        }
        else
        {
            if (isRightMouseClick == true)
            {
                if(isOrder_Patrol == true || isOrder_Attack == true)
                {
                    _isOrdering = false;
                    GameManager.instance.DefultCursor();
                    return;
                }

                if(InGameManager.instance.selectObjectCount > 0)
                {
                    if (InGameManager.instance.firstObjectFriendIdentificationType == eFriendIdentification.My)
                    {
                        GameManager.instance.ChangeCursor(eCursorType.Point, eFriendIdentification.My);
                        return;
                    }
                }
                else
                {
                    GameManager.instance.ChangeCursor(eCursorType.Point, eFriendIdentification.My);
                    return;
                }
            }

            if (isOrder_Patrol == true)
            {
                _isOrdering = true;
                
                GameManager.instance.ChangeCursor(eCursorType.Point, eFriendIdentification.Alliance);
            }
            else if (isOrder_Attack == true)
            {
                _isOrdering = true;
                GameManager.instance.ChangeCursor(eCursorType.PointAttack, eFriendIdentification.Alliance);
            }
            else
            {
                _isOrdering = false;
                GameManager.instance.DefultCursor();
            }

            if (isLeftMouseClick == true)
            {
                if(_isOrdering == false)
                {
                    GameManager.instance.ChangeCursor(eCursorType.SelectBox);
                    return;
                }
            }
        }
    }

    private void Key()
    {
        if (InGameManager.instance.selectObjectCount > 0)
        {
            if (InGameManager.instance.firstObjectFriendIdentificationType == eFriendIdentification.Alliance || InGameManager.instance.firstObjectFriendIdentificationType == eFriendIdentification.Enemy)
            {
                return;
            }
        }

        if (isPress_P == true)
        {
            isOrder_Patrol = true;
            isOrder_Attack = false;
        }

        if (isPress_A == true)
        {
            isOrder_Attack = true;
            isOrder_Patrol = false;
        }
    }

    private void Input_Mouse()
    {
        if (Input.GetMouseButtonDown(0)) // L
        {
            if(isOrder_Patrol == true)
            {
                _onOrderPatrol?.Invoke();
                return;
            }

            if (isOrder_Attack == true)
            {
                _onOrderAttack?.Invoke();
                return;
            }

            isLeftMouseClick = true;

            _onMouseL_Down?.Invoke();
        }

        if (Input.GetMouseButton(0)) // L Hold
        {
            _onMouseL_Hold?.Invoke();
        }

        if (Input.GetMouseButtonUp(0)) // L Up
        {
            isOrder_Patrol = false;
            isOrder_Attack = false;

            isLeftMouseClick = false;
            _onMouseL_Up?.Invoke();
        }

        if (Input.GetMouseButtonDown(1)) // R Down
        {
            isOrder_Patrol = false;
            isOrder_Attack = false;

            isRightMouseClick = true;
            _onMouseR_Down?.Invoke();
        }

        if(Input.GetMouseButton(1)) // R Hold
        {
            _onMouseR_Hold?.Invoke();
        }

        if (Input.GetMouseButtonUp(1)) // R Up
        {
            isRightMouseClick = false;
            _onMouseR_Up?.Invoke();
        }

        if (Input.GetMouseButtonDown(2)) // H Down
        {
            isWhillMouseClick = false;
            _onMouseW?.Invoke();
        }

        if (Input.GetMouseButtonUp(2)) // H Up
        {
            isWhillMouseClick = true;
            _onMouseW?.Invoke();
        }
    }

    private void Input_Function()
    {
        InputKeyboard(KeyCode.F2, ref osPress_F2, _on_Down_F2, _on_Hold_F2, _on_Up_F2);
        InputKeyboard(KeyCode.F3, ref osPress_F3, _on_Down_F3, _on_Hold_F3, _on_Up_F3);
        InputKeyboard(KeyCode.F4, ref osPress_F4, _on_Down_F4, _on_Hold_F4, _on_Up_F4);
    }

    private void Input_Number()
    {
        InputKeyboard(KeyCode.Alpha1, ref osPress_1, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha2, ref osPress_2, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha3, ref osPress_3, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha4, ref osPress_4, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha5, ref osPress_5, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha6, ref osPress_6, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha7, ref osPress_7, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha8, ref osPress_8, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha9, ref osPress_9, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
        InputKeyboard(KeyCode.Alpha0, ref osPress_0, _on_Down_Alpha1, _on_Hold_Alpha1, _on_Up_Alpha1);
    }

    private void Input_Q_P()
    {
        InputKeyboard(KeyCode.Q, ref isPress_Q, _on_Down_Q, _on_Hold_Q, _on_Up_Q);
        InputKeyboard(KeyCode.W, ref isPress_W, _on_Down_W, _on_Hold_W, _on_Up_W);
        InputKeyboard(KeyCode.E, ref isPress_E, _on_Down_E, _on_Hold_E, _on_Up_E);
        InputKeyboard(KeyCode.R, ref isPress_R, _on_Down_R, _on_Hold_R, _on_Up_R);
        InputKeyboard(KeyCode.T, ref isPress_T, _on_Down_T, _on_Hold_T, _on_Up_T);
        InputKeyboard(KeyCode.Y, ref isPress_Y, _on_Down_Y, _on_Hold_Y, _on_Up_Y);
        InputKeyboard(KeyCode.U, ref isPress_U, _on_Down_U, _on_Hold_U, _on_Up_U);
        InputKeyboard(KeyCode.I, ref isPress_I, _on_Down_I, _on_Hold_I, _on_Up_I);
        InputKeyboard(KeyCode.O, ref isPress_O, _on_Down_O, _on_Hold_O, _on_Up_O);
        InputKeyboard(KeyCode.P, ref isPress_P, _on_Down_P, _on_Hold_P, _on_Up_P);
    }

    private void Input_A_L()
    {
        InputKeyboard(KeyCode.A, ref isPress_A, _on_Down_A, _on_Hold_A, _on_Up_A);
        InputKeyboard(KeyCode.S, ref isPress_S, _on_Down_S, _on_Hold_S, _on_Up_S);
        InputKeyboard(KeyCode.D, ref isPress_D, _on_Down_D, _on_Hold_D, _on_Up_D);
        InputKeyboard(KeyCode.F, ref isPress_F, _on_Down_F, _on_Hold_F, _on_Up_F);
        InputKeyboard(KeyCode.G, ref isPress_G, _on_Down_G, _on_Hold_G, _on_Up_G);
        InputKeyboard(KeyCode.H, ref isPress_H, _on_Down_H, _on_Hold_H, _on_Up_H);
        InputKeyboard(KeyCode.J, ref isPress_J, _on_Down_J, _on_Hold_J, _on_Up_J);
        InputKeyboard(KeyCode.K, ref isPress_K, _on_Down_K, _on_Hold_K, _on_Up_K);
        InputKeyboard(KeyCode.L, ref isPress_L, _on_Down_L, _on_Hold_L, _on_Up_L);
    }

    private void Input_Z_M()
    {
        InputKeyboard(KeyCode.Z, ref isPress_Z, _on_Down_Z, _on_Hold_Z, _on_Up_Z);
        InputKeyboard(KeyCode.X, ref isPress_X, _on_Down_X, _on_Hold_X, _on_Up_X);
        InputKeyboard(KeyCode.C, ref isPress_C, _on_Down_C, _on_Hold_C, _on_Up_C);
        InputKeyboard(KeyCode.V, ref isPress_V, _on_Down_V, _on_Hold_V, _on_Up_V);
        InputKeyboard(KeyCode.B, ref isPress_B, _on_Down_B, _on_Hold_B, _on_Up_B);
        InputKeyboard(KeyCode.N, ref isPress_N, _on_Down_N, _on_Hold_N, _on_Up_N);
        InputKeyboard(KeyCode.M, ref isPress_M, _on_Down_M, _on_Hold_M, _on_Up_M);
    }

    private void InputSpecial()
    {
        InputKeyboard(KeyCode.Space, ref isPress_Space, _onSpace, null, null);
        InputKeyboard(KeyCode.LeftShift, ref isPress_L_Shift, _onShift_L, null, null);
        InputKeyboard(KeyCode.LeftControl, ref isPress_L_Ctrl, _onCtrl_L, null, null);
    }

    private void InputKeyboard(KeyCode code, ref bool isPress, Action onDownCallback, Action onHoldCallback, Action onUpCallback)
    {
        if(Input.GetKeyDown(code))
        {
            isPress = true;
            onDownCallback?.Invoke();
        }

        if (Input.GetKey(code))
        {
            onHoldCallback?.Invoke();
        }

        if (Input.GetKeyUp(code))
        {
            isPress = false;
            onUpCallback?.Invoke();
        }
    }

    public void SetMouseKey(eMouseInput mouse, eClickType type, Action onCallback)
    {
        if(onCallback == null)
        {
            return;
        }

        switch(mouse)
        {
            case eMouseInput.Left:
                switch (type)
                {
                    case eClickType.Down:
                        _onMouseL_Down = onCallback;
                        break;

                    case eClickType.Hold:
                        _onMouseL_Hold = onCallback;
                        break;

                    case eClickType.Up:
                        _onMouseL_Up = onCallback;
                        break;
                }
                break;

            case eMouseInput.Right:
                switch(type)
                {
                    case eClickType.Down:
                        _onMouseR_Down = onCallback;
                        break;

                    case eClickType.Hold:
                        _onMouseR_Hold = onCallback;
                        break;

                    case eClickType.Up:
                        _onMouseR_Up = onCallback;
                        break;
                }
                break;

            case eMouseInput.Wheel:
                _onMouseW = onCallback;
                break;
        }
    }

    public void RemoveMouseKey(eMouseInput mouse, eClickType type)
    {
        switch (mouse)
        {
            case eMouseInput.Left:
                switch (type)
                {
                    case eClickType.Down:
                        _onMouseL_Down = null;
                        break;

                    case eClickType.Hold:
                        _onMouseL_Hold = null;
                        break;

                    case eClickType.Up:
                        _onMouseL_Up = null;
                        break;
                }
                break;

            case eMouseInput.Right:
                switch (type)
                {
                    case eClickType.Down:
                        _onMouseR_Down = null;
                        break;

                    case eClickType.Hold:
                        _onMouseR_Hold = null;
                        break;

                    case eClickType.Up:
                        _onMouseR_Up = null;
                        break;
                }
                break;

            case eMouseInput.Wheel:
                _onMouseW = null;
                break;
        }
    }

    public void SetKeyboardKey(KeyCode code, eClickType type, eOrder order, Action onCallback)
    {
        if(onCallback == null)
        {
            return;
        }

        if(order != eOrder.Non)
        {

            switch (code)
            {
                case KeyCode.A:
                    _onOrderAttack = onCallback;
                    break;

                case KeyCode.P:
                    _onOrderPatrol = onCallback;
                    break;
            }

            return;
        }

        switch(code)
        {
            case KeyCode.F2:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_F2, ref _on_Hold_F2, ref _on_Up_F2);
                break;

            case KeyCode.F3:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_F3, ref _on_Hold_F2, ref _on_Up_F2);
                break;

            case KeyCode.F4:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_F4, ref _on_Hold_F2, ref _on_Up_F2);
                break;

            case KeyCode.Alpha1:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha1, ref _on_Hold_Alpha1, ref _on_Up_Alpha1);
                break;

            case KeyCode.Alpha2:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha2, ref _on_Hold_Alpha2, ref _on_Up_Alpha2);
                break;

            case KeyCode.Alpha3:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha3, ref _on_Hold_Alpha3, ref _on_Up_Alpha3);
                break;

            case KeyCode.Alpha4:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha4, ref _on_Hold_Alpha4, ref _on_Up_Alpha4);
                break;

            case KeyCode.Alpha5:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha5, ref _on_Hold_Alpha5, ref _on_Up_Alpha5);
                break;

            case KeyCode.Alpha6:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha6, ref _on_Hold_Alpha6, ref _on_Up_Alpha6);
                break;

            case KeyCode.Alpha7:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha7, ref _on_Hold_Alpha7, ref _on_Up_Alpha7);
                break;

            case KeyCode.Alpha8:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha8, ref _on_Hold_Alpha8, ref _on_Up_Alpha8);
                break;

            case KeyCode.Alpha9:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha9, ref _on_Hold_Alpha9, ref _on_Up_Alpha9);
                break;

            case KeyCode.Alpha0:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Alpha0, ref _on_Hold_Alpha0, ref _on_Up_Alpha0);
                break;

            case KeyCode.Q:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Q, ref _on_Hold_Q, ref _on_Up_Q);
                break;

            case KeyCode.W:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_W, ref _on_Hold_W, ref _on_Up_W);
                break;

            case KeyCode.E:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_E, ref _on_Hold_E, ref _on_Up_E);
                break;

            case KeyCode.R:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_R, ref _on_Hold_R, ref _on_Up_R);
                break;

            case KeyCode.T:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_T, ref _on_Hold_T, ref _on_Up_T);
                break;

            case KeyCode.Y:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Y, ref _on_Hold_Y, ref _on_Up_Y);
                break;

            case KeyCode.U:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_U, ref _on_Hold_U, ref _on_Up_U);
                break;

            case KeyCode.I:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_I, ref _on_Hold_I, ref _on_Up_I);
                break;

            case KeyCode.O:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_O, ref _on_Hold_O, ref _on_Up_O);
                break;

            case KeyCode.P:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_P, ref _on_Hold_P, ref _on_Up_P);
                break;

            case KeyCode.A:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_A, ref _on_Hold_A, ref _on_Up_A);
                break;

            case KeyCode.S:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_S, ref _on_Hold_S, ref _on_Up_S);
                break;

            case KeyCode.D:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_D, ref _on_Hold_D, ref _on_Up_D);
                break;

            case KeyCode.F:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_F, ref _on_Hold_F, ref _on_Up_F);
                break;

            case KeyCode.G:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_G, ref _on_Hold_G, ref _on_Up_G);
                break;

            case KeyCode.H:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_H, ref _on_Hold_H, ref _on_Up_H);
                break;

            case KeyCode.J:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_J, ref _on_Hold_J, ref _on_Up_J);
                break;

            case KeyCode.K:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_K, ref _on_Hold_K, ref _on_Up_K);
                break;

            case KeyCode.L:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_L, ref _on_Hold_L, ref _on_Up_L);
                break;

            case KeyCode.Z:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_Z, ref _on_Hold_Z, ref _on_Up_Z);
                break;

            case KeyCode.X:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_X, ref _on_Hold_X, ref _on_Up_X);
                break;

            case KeyCode.C:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_C, ref _on_Hold_C, ref _on_Up_C);
                break;

            case KeyCode.V:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_V, ref _on_Hold_V, ref _on_Up_V);
                break;

            case KeyCode.B:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_B, ref _on_Hold_B, ref _on_Up_B);
                break;

            case KeyCode.N:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_N, ref _on_Hold_N, ref _on_Up_N);
                break;

            case KeyCode.M:
                SetKeyBoardKey_(type, onCallback, ref _on_Down_M, ref _on_Hold_M, ref _on_Up_M);
                break;

            case KeyCode.Space:
                _onSpace = onCallback;
                break;

            case KeyCode.LeftShift:
                _onShift_L = onCallback;
                break;

            case KeyCode.LeftControl:
                _onCtrl_L = onCallback;
                break;
        }
    }

    private void SetKeyBoardKey_(eClickType type, Action onCallback, ref Action onInputDownCallback, ref Action onInputHoldCallback, ref Action onInputUpCallback)
    {
        switch (type)
        {
            case eClickType.Down:
                onInputDownCallback = onCallback;
                break;

            case eClickType.Hold:
                onInputHoldCallback = onCallback;
                break;

            case eClickType.Up:
                onInputUpCallback = onCallback;
                break;
        }
    }

    public void RemoveKeyboardKey(KeyCode code, eClickType type)
    {
        switch (code)
        {
            case KeyCode.F2:
                RemoveKeyboardKey_(type, ref _on_Down_F2, ref _on_Hold_F2, ref _on_Up_F2);
                break;

            case KeyCode.F3:
                RemoveKeyboardKey_(type, ref _on_Down_F3, ref _on_Hold_F2, ref _on_Up_F2);
                break;

            case KeyCode.F4:
                RemoveKeyboardKey_(type, ref _on_Down_F4, ref _on_Hold_F2, ref _on_Up_F2);
                break;

            case KeyCode.Alpha1:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha1, ref _on_Hold_Alpha1, ref _on_Up_Alpha1);
                break;

            case KeyCode.Alpha2:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha2, ref _on_Hold_Alpha2, ref _on_Up_Alpha2);
                break;

            case KeyCode.Alpha3:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha3, ref _on_Hold_Alpha3, ref _on_Up_Alpha3);
                break;

            case KeyCode.Alpha4:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha4, ref _on_Hold_Alpha4, ref _on_Up_Alpha4);
                break;

            case KeyCode.Alpha5:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha5, ref _on_Hold_Alpha5, ref _on_Up_Alpha5);
                break;

            case KeyCode.Alpha6:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha6, ref _on_Hold_Alpha6, ref _on_Up_Alpha6);
                break;

            case KeyCode.Alpha7:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha7, ref _on_Hold_Alpha7, ref _on_Up_Alpha7);
                break;

            case KeyCode.Alpha8:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha8, ref _on_Hold_Alpha8, ref _on_Up_Alpha8);
                break;

            case KeyCode.Alpha9:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha9, ref _on_Hold_Alpha9, ref _on_Up_Alpha9);
                break;

            case KeyCode.Alpha0:
                RemoveKeyboardKey_(type, ref _on_Down_Alpha0, ref _on_Hold_Alpha0, ref _on_Up_Alpha0);
                break;

            case KeyCode.Q:
                RemoveKeyboardKey_(type, ref _on_Down_Q, ref _on_Hold_Q, ref _on_Up_Q);
                break;

            case KeyCode.W:
                RemoveKeyboardKey_(type, ref _on_Down_W, ref _on_Hold_W, ref _on_Up_W);
                break;

            case KeyCode.E:
                RemoveKeyboardKey_(type, ref _on_Down_E, ref _on_Hold_E, ref _on_Up_E);
                break;

            case KeyCode.R:
                RemoveKeyboardKey_(type, ref _on_Down_R, ref _on_Hold_R, ref _on_Up_R);
                break;

            case KeyCode.T:
                RemoveKeyboardKey_(type, ref _on_Down_T, ref _on_Hold_T, ref _on_Up_T);
                break;

            case KeyCode.Y:
                RemoveKeyboardKey_(type, ref _on_Down_Y, ref _on_Hold_Y, ref _on_Up_Y);
                break;

            case KeyCode.U:
                RemoveKeyboardKey_(type, ref _on_Down_U, ref _on_Hold_U, ref _on_Up_U);
                break;

            case KeyCode.I:
                RemoveKeyboardKey_(type, ref _on_Down_I, ref _on_Hold_I, ref _on_Up_I);
                break;

            case KeyCode.O:
                RemoveKeyboardKey_(type, ref _on_Down_O, ref _on_Hold_O, ref _on_Up_O);
                break;

            case KeyCode.P:
                RemoveKeyboardKey_(type, ref _on_Down_P, ref _on_Hold_P, ref _on_Up_P);
                break;

            case KeyCode.A:
                RemoveKeyboardKey_(type, ref _on_Down_A, ref _on_Hold_A, ref _on_Up_A);
                break;

            case KeyCode.S:
                RemoveKeyboardKey_(type, ref _on_Down_S, ref _on_Hold_S, ref _on_Up_S);
                break;

            case KeyCode.D:
                RemoveKeyboardKey_(type, ref _on_Down_D, ref _on_Hold_D, ref _on_Up_D);
                break;

            case KeyCode.F:
                RemoveKeyboardKey_(type, ref _on_Down_F, ref _on_Hold_F, ref _on_Up_F);
                break;

            case KeyCode.G:
                RemoveKeyboardKey_(type, ref _on_Down_G, ref _on_Hold_G, ref _on_Up_G);
                break;

            case KeyCode.H:
                RemoveKeyboardKey_(type, ref _on_Down_H, ref _on_Hold_H, ref _on_Up_H);
                break;

            case KeyCode.J:
                RemoveKeyboardKey_(type, ref _on_Down_J, ref _on_Hold_J, ref _on_Up_J);
                break;

            case KeyCode.K:
                RemoveKeyboardKey_(type, ref _on_Down_K, ref _on_Hold_K, ref _on_Up_K);
                break;

            case KeyCode.L:
                RemoveKeyboardKey_(type, ref _on_Down_L, ref _on_Hold_L, ref _on_Up_L);
                break;

            case KeyCode.Z:
                RemoveKeyboardKey_(type, ref _on_Down_Z, ref _on_Hold_Z, ref _on_Up_Z);
                break;

            case KeyCode.X:
                RemoveKeyboardKey_(type, ref _on_Down_X, ref _on_Hold_X, ref _on_Up_X);
                break;

            case KeyCode.C:
                RemoveKeyboardKey_(type, ref _on_Down_C, ref _on_Hold_C, ref _on_Up_C);
                break;

            case KeyCode.V:
                RemoveKeyboardKey_(type, ref _on_Down_V, ref _on_Hold_V, ref _on_Up_V);
                break;

            case KeyCode.B:
                RemoveKeyboardKey_(type, ref _on_Down_B, ref _on_Hold_B, ref _on_Up_B);
                break;

            case KeyCode.N:
                RemoveKeyboardKey_(type, ref _on_Down_N, ref _on_Hold_N, ref _on_Up_N);
                break;

            case KeyCode.M:
                RemoveKeyboardKey_(type, ref _on_Down_M, ref _on_Hold_M, ref _on_Up_M);
                break;

            case KeyCode.Space:
                _onSpace = null;
                break;

            case KeyCode.LeftShift:
                _onShift_L = null;
                break;
        }
    }

    private void RemoveKeyboardKey_(eClickType type, ref Action onInputDownCallback, ref Action onInputHoldCallback, ref Action onInputUpCallback)
    {
        switch (type)
        {
            case eClickType.Down:
                onInputDownCallback = null;
                break;

            case eClickType.Hold:
                onInputHoldCallback = null;
                break;

            case eClickType.Up:
                onInputUpCallback = null;
                break;
        }
    }

    public void SetMove(eDirection dir, Action onCallback)
    {
        switch(dir)
        {
            case eDirection.Up:
                _on_SideMove_Up = onCallback;
                break;

            case eDirection.Down:
                _on_SideMove_Down = onCallback;
                break;

            case eDirection.Left:
                _on_SideMove_Left = onCallback;
                break;

            case eDirection.Right:
                _on_SideMove_Right = onCallback;
                break;
        }
    }
}