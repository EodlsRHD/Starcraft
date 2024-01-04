using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using System;

// RULE

// 1. naming
//  Camel
//  private variable name --> _aaBb
//  public variable name --> aaBb
//  button use function name --> OnAaBb

// 2. GetSet
//  #region Get Set ~~~ #endregion
//  just get, set value --> get,set { ~~~ } one line

// 3. if
//  braces is parentheses next line
//  ex) if()
//      {
//
//      }

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    [SerializeField, Header("Manager")]
    private ToolManager _toolManager = null;

    [SerializeField]
    private CursorManager _cursorManager = null;

    [SerializeField, Header("TEST MODE")]
    private bool test_mode = false;

    private bool _isLobyFirstLoadDone = false;

    private eScene _currentScene = eScene.Non;

    private MapData _mapdata = null;

    #region Get Set

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    public ToolManager toolManager
    {
        get { return _toolManager; }
    }

    public bool isLobyFirstLoadDone
    {
        get { return _isLobyFirstLoadDone; }
        set { _isLobyFirstLoadDone = value; }
    }

    public eScene currentScene
    {
        get { return _currentScene; }
    }

    public  MapData currentMapdata
    {
        get { return _mapdata; }
        set { _mapdata = value; }
    }

    public bool TEST_MODE
    {
        get { return test_mode; }
    }

    #endregion

    void Awake()
    {
        _instance = this;

        _toolManager.Initialize(ChangeScene);
        _cursorManager.Initialize();

        DontDestroyOnLoad(this.gameObject);

        //Debug.unityLogger.logEnabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)eScene.Entry);
    }

    private void ChangeScene(eScene scene)
    {
        _currentScene = scene;
    }

    public void ChangeCursorParant(Transform tr)
    {
        _cursorManager.ChangeParant(tr);
    }

    public void BeforeCursor()
    {
        _cursorManager.BeforeCursor();
    }

    public void DefultCursor()
    {
        _cursorManager.DefultCursor();
    }

    public void ChangeCursor(eCursorType type)
    {
        _cursorManager.ChangeCursor(type);
    }

    public void ChangeCursor(eCursorType type, eFriendIdentification friendIdentification)
    {
        _cursorManager.ChangeCursor(type, friendIdentification);
    }

    public void ChangeCursor(eCursorType type, eDirection direction)
    {
        _cursorManager.ChangeCursor(type, direction);
    }

    public void GameQuit()
    {
        Debug.Log("Game Quit");

        _cursorManager.Quit();

        //Application.Quit();
    }
}
