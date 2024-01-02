using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InterfaceManager : MonoBehaviour
{
    private static InterfaceManager _instance = null;

    [SerializeField]
    private GameObject _background = null;

    [SerializeField]
    private PopUp _popUp = null;

    [SerializeField]
    private Windows _windows = null;

    [SerializeField]
    private Transform _cursorParant = null;

    #region GetSet

    public static InterfaceManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new InterfaceManager();
            }

            return _instance;
        }
    }

    #endregion

    void Start()
    {
        _instance = this;

        _popUp.Initialize();
        _windows.Initialize(InterfaceBackgroundActivation);
        
        if(GameManager.instance.TEST_MODE == false)
        {
            Cursor.visible = false;
        }
        GameManager.instance.ChangeCursorParant(_cursorParant);

        _background.SetActive(false);
    }

    private void InterfaceBackgroundActivation(bool isOn)
    {
        _background.SetActive(isOn);
    }

    #region Popup

    public void OpenYesOrNoPopup(string title, string description, Action onCancelCallback, Action onConfirmCallback, string cancel = null, string confirm = null)
    {
        InterfaceBackgroundActivation(true);

        _popUp.SetYesOrNo(title, description, onCancelCallback, onConfirmCallback, cancel == null ? string.Empty : cancel, confirm == null ? string.Empty : confirm);
    }

    public void OpenOneButton(string title, string description, Action onCancelCallback, string cancel = null)
    {
        InterfaceBackgroundActivation(true);

        _popUp.SetOneButton(title, description, onCancelCallback, cancel == null ? string.Empty : cancel);
    }

    public void ClosePopUp()
    {
        _popUp.ClosePopUp();
        InterfaceBackgroundActivation(false);
    }

    #endregion

    #region Windows

    public void OpenSettings()
    {
        _windows.OpenSettings();
    }

    #endregion
}
